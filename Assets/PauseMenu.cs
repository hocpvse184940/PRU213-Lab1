using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    // ĐẶT ĐÚNG 2 GIÁ TRỊ NÀY TRONG INSPECTOR
    [SerializeField] string mainMenuSceneName = "MainMenu"; // đúng hệt tên file scene
    [SerializeField] int mainMenuBuildIndex = 0;            // index của MainMenu trong Build Settings

    public void Pause()
    {
        if (pauseMenu) pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (pauseMenu) pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void Home()
    {
        Time.timeScale = 1f;

        // Thử load theo TÊN nếu scene đã add vào Build Settings:
        if (!string.IsNullOrEmpty(mainMenuSceneName) &&
            Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
            return;
        }

        // Fallback: load theo BUILD INDEX
        if (mainMenuBuildIndex >= 0 && mainMenuBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(mainMenuBuildIndex);
            return;
        }

        Debug.LogError("Không tìm thấy scene MainMenu. Hãy kiểm tra Build Settings và tên scene.");
    }
}
