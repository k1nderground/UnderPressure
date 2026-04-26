using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    private bool isPaused;

    void Start(){
        isPaused = false;
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    void Update(){

        if(Input.GetKeyDown(KeyCode.Escape)){
            togglePause();
        }
    }

    public void togglePause(){
        isPaused = !isPaused;

        if(isPaused){
            Time.timeScale = 0;
        }
        else{
            Time.timeScale = 1;
        }

        PausePanel.SetActive(!PausePanel.activeSelf);
    }

    public void ExitToMenu(){
        SceneManager.LoadScene("MainMenu"); 
    }
}
