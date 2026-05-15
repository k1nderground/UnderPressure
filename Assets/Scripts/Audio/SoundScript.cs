using UnityEngine;

public class SoundScript : MonoBehaviour
{
    private AudioSource src;
    [SerializeField] AudioClip[] sounds;

    void Start(){
        src = GetComponent<AudioSource>();
    }

    public void Play(int i){
        src.PlayOneShot(sounds[i]);
    }
}
