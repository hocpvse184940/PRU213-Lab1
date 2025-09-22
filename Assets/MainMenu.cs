using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Asteroids");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!"); 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
            Application.Quit(); // Thoát app khi build ra .exe/.apk
#endif
    }
}
