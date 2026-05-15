using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MainMenuScript : MonoBehaviour
{
    public void Start(){
        if (SceneManager.GetActiveScene().name == "Lose")
        {
            YG2.InterstitialAdvShow();
        }
    }

    public void StartGame()
    {
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