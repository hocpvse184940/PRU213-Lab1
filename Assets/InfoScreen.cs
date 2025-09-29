using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoScreen : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";
    public void BackToMenu() => SceneManager.LoadScene(mainMenuScene);
}
