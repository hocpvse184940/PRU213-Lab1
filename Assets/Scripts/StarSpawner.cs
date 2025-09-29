using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab; // Assign your Star prefab in the Inspector
    public float spawnRate = 5.0f;
    public float spawnRadius = 10.0f;
    public int spawnAmount = 1;
    public float lifeTime = 8.0f;
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnRadius;
            Vector3 spawnPoint = this.transform.position + spawnDirection;
           GameObject star =  Instantiate(this.starPrefab, spawnPoint, Quaternion.identity);
            Destroy(star, this.lifeTime);
        }
    }
}
