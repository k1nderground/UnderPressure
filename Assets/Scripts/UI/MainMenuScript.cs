using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MainMenuScript : MonoBehaviour
{

    public void StartGame()
    {
        YG2.InterstitialAdvShow();
        SceneManager.LoadScene("Game");
        
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}