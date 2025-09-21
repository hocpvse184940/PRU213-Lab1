using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public float respawnInvulerability = 3.0f;
    public int lives = 3;
    public int score = 0;
    [SerializeField] private int highscore = 0; // show in inspector but cant change

    private void Awake()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if(asteroid.size < 0.75)
        {
            this.score += 100;
        } else if(asteroid.size < 1.2f)
        {
            this.score += 50;
        } else
        {
            this.score += 25;
        }
    }


    public void PlayerDie()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;
        if(this.lives <= 0)
        {
            GameOver();
        }
        Invoke(nameof(Respawn), this.respawnTime);
    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        this.player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), this.respawnInvulerability);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        //Save player high score
        int savedHighScore = PlayerPrefs.GetInt("Highscore", 0);
        if (this.score > savedHighScore)
        {
            PlayerPrefs.SetInt("Highscore", this.score);
            PlayerPrefs.Save();
            highscore = this.score; //Update Inspector variable value
        }
        else
        {
            highscore = savedHighScore; //Keep current high score 
        }
            this.lives = 3;
        this.score = 0;

        Invoke(nameof(Respawn), this.respawnTime);
    }

    private void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
        highscore = 0;
    }
}
