using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public float respawnInvulerability = 3.0f;
    public int lives = 3;
    public int score = 0;
    [SerializeField] private int highscore = 0; // show in inspector but cant change
    public Heart heartManager;
    public TextMeshProUGUI scoreText;

    // KHAI BÁO CÁC BIẾN MỚI CHO MÀN HÌNH GAME OVER
    public GameObject gameOverPanel; // Panel chính của màn hình Game Over
    public TextMeshProUGUI finalScoreText; // Text hiển thị điểm số cuối cùng
    public TextMeshProUGUI highscoreText;  // Text hiển thị điểm cao nhất
    //public GameObject pausePanel; // Panel tạm dừng game (nếu có để tắt khi game over)



    private void Awake()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        this.scoreText.text = "" + this.score;

        // Đảm bảo màn hình Game Over ẩn khi game bắt đầu
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        // Đảm bảo màn hình Pause cũng ẩn khi game bắt đầu
        //if (pausePanel != null)
        //{
        //    pausePanel.SetActive(false);
        //}

        Time.timeScale = 1f; // Đảm bảo game chạy bình thường khi bắt đầu
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
        // Cập nhật text UI
        this.scoreText.text = " " + this.score;
    }



    // HÀM RESTART GAME KHI NHẤN NÚT
    public void RestartGame()
    {
        // Đảm bảo game tiếp tục chạy
        Time.timeScale = 1f;

        // Ẩn màn hình Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Đặt lại các biến trò chơi về trạng thái ban đầu
        this.lives = 3;
        this.score = 0;
        this.scoreText.text = " " + this.score; // Cập nhật Text Score trên màn hình HUD
        this.heartManager.UpdateHearts(this.lives); // Cập nhật lại UI trái tim (nếu có)

        // Hồi sinh người chơi và bắt đầu game mới
        Respawn();
    }

    // HÀM QUIT GAME KHI NHẤN NÚT
    public void QuitGame()
    {
        // Đảm bảo game tiếp tục chạy trước khi thoát (đề phòng Time.timeScale = 0)
        Time.timeScale = 1f;

        // Thoát ứng dụng
        Application.Quit();

        // Dòng dưới đây chỉ hoạt động trong Unity Editor để dừng chế độ Play
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }



    public void PlayerDie()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

        // Call a method to update the hearts
        this.heartManager.UpdateHearts(this.lives);

        //if (this.lives <= 0)
        //{
        //    GameOver();
        //}
        //Invoke(nameof(Respawn), this.respawnTime);

        if (this.lives <= 0)
        {
            // Khi hết mạng, gọi hàm GameOver
            GameOver();
        }
        else
        {
            // Chỉ hồi sinh nếu vẫn còn mạng
            Invoke(nameof(Respawn), this.respawnTime);
        }
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
        // Vô hiệu hóa người chơi để không thể điều khiển hoặc va chạm nữa
        this.player.gameObject.SetActive(false);

        // Dừng game hoàn toàn bằng cách đặt Time.timeScale về 0
        Time.timeScale = 0f;

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


        // Hiển thị màn hình Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Cập nhật các Text UI của màn hình Game Over
        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + this.score;
        }
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + highscore;
        }

        //// Đảm bảo màn hình Pause tắt nếu đang bật (nếu có)
        //if (pausePanel != null && pausePanel.activeSelf)
        //{
        //    pausePanel.SetActive(false);
        //}


        //this.lives = 3;
        //this.score = 0;
        //this.scoreText.text = " " + this.score;

        //Invoke(nameof(Respawn), this.respawnTime);
    }

    private void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
        highscore = 0;
    }
}
