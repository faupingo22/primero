using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UIManager uiManager;
    private int totalEnemies;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        totalEnemies = FindObjectsOfType<EnemyHealth>().Length;
        Debug.Log($"Nivel iniciado con {totalEnemies} enemigos.");

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EnemyDefeated()
    {
        totalEnemies--;
        if (totalEnemies <= 0)
        {
            Victory();
        }
    }

    public void PlayerDied()
    {
        if (uiManager != null)
        {
            uiManager.ShowDefeatScreen();
        }
    }

    void Victory()
    {
        Debug.Log("VICTORIA!");
        if (uiManager != null)
        {
            uiManager.ShowVictoryScreen();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
