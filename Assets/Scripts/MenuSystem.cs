using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public GameObject CreditsMenu;
    public GameObject PauseMenu;
    public GameObject MainMenuUI;
    public bool isPaused = false;

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadSceneAsync(1);
    }

    public void Ready()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadSceneAsync(3);
    }

    public void PauseGame()
    {
        DisableAllUI();
        PauseMenu.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseEscape()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableAllUI()
    {
        MainMenuUI.SetActive(false);
        PauseMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }

    public void Credits()
    {
        DisableAllUI();
        CreditsMenu.SetActive(true);
    }

    public void ReturnToMenu()
    {
        DisableAllUI();
        MainMenuUI.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stops play mode in Editor
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
                return;

            if (isPaused)
                PauseEscape();

            else
                PauseGame();
        }
    }
}
