using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50.0f;       // dùng khi không truyền speed từ spawner
    public float maxLifeTime = 30.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    public void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;
        _rigidBody.mass = this.size;
    }

    // Giữ hàm cũ (fallback)
    public void SetTrajectory(Vector2 direction)
    {
        SetTrajectory(direction, this.speed);
    }

    // NEW: nhận speed tuyệt đối từ Spawner (dễ tăng giảm theo thời gian)
    public void SetTrajectory(Vector2 direction, float newSpeed)
    {
        // đảm bảo hướng chuẩn hoá và đặt vận tốc trực tiếp cho dễ điều khiển
        _rigidBody.linearVelocity = direction.normalized * newSpeed;
        Destroy(this.gameObject, this.maxLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if ((this.size * 0.5f) >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            Object.FindFirstObjectByType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;
        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        // khi tách đôi, dùng tốc độ hiện tại của viên cha (magnitude giữ nguyên)
        var dir = Random.insideUnitCircle.normalized;
        float currentSpeed = _rigidBody != null ? _rigidBody.linearVelocity.magnitude : this.speed;
        half.SetTrajectory(dir, currentSpeed);
    }
}
