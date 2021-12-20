using UnityEngine;
using UnityEngine.SceneManagement;

public class scenesLoader :MonoBehaviour
{
    public void LoadMainMenu()
    {
        
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}