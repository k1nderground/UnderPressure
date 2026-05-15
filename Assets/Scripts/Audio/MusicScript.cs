using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicScript : MonoBehaviour
{
    private AudioSource src;
    [SerializeField] AudioClip[] sounds;

    void Start(){
        src = GetComponent<AudioSource>();

        if(SceneManager.GetActiveScene().name == "Game"){
            src.clip = sounds[0];
            src.Play();
        }

        if(SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Shop"){
            src.clip = sounds[1];
            src.Play();
        }
    }

}
