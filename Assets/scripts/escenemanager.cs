using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Nivel01");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}