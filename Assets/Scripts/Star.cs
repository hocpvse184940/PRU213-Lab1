using UnityEngine;

public class StarPickup : MonoBehaviour
{
    public int scoreValue = 100; //Star score can be change in Unity Inspector
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.StarCollected(scoreValue);
            }
            Destroy(gameObject);
        }
    }
}
