using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName = "Info";
    public void LoadTarget()
    {
        SceneManager.LoadScene(sceneName);
    }
}
