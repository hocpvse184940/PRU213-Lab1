using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;

    [Header("Spawn shape")]
    public float trajectoryVariance = 15f;
    public float spawnDistance = 15f;
    public int spawnAmount = 1;

    [Header("Difficulty over time")]
    public float baseSpawnInterval = 2.0f;   // giây lúc đầu
    public float minSpawnInterval = 0.4f;   // không thấp hơn
    public float spawnAccelPerSecond = 0.02f;  // mỗi giây giảm bấy nhiêu giây

    public float baseAsteroidSpeed = 2.5f;   // tốc độ lúc đầu
    public float speedGainPerSecond = 0.15f;  // mỗi giây tăng bấy nhiêu tốc độ
    public float maxAsteroidSpeed = 12f;    // không vượt quá

    float _nextSpawnAt;

    void Start()
    {
        _nextSpawnAt = Time.time + baseSpawnInterval;
    }

    void Update()
    {
        float t = Time.timeSinceLevelLoad; // tổng thời gian đã chơi

        // Tính interval & speed hiện tại
        float currentInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - spawnAccelPerSecond * t);
        float currentSpeed = Mathf.Min(maxAsteroidSpeed, baseAsteroidSpeed + speedGainPerSecond * t);

        // (tùy chọn) log để bạn thấy rõ khó tăng
        if (Time.frameCount % 30 == 0)
            Debug.Log($"[Difficulty] t={t:F1}s | interval={currentInterval:F2}s | speed={currentSpeed:F2}");

        if (Time.time >= _nextSpawnAt)
        {
            _nextSpawnAt = Time.time + currentInterval;
            Spawn(currentSpeed);
        }
    }

    private void Spawn(float moveSpeed)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDir = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPoint = transform.position + spawnDir;
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rot = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rot);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            // Bay từ ngoài vào trong
            Vector2 flyDir = (rot * -spawnDir);
            asteroid.SetTrajectory(flyDir, moveSpeed);
        }
    }
}
