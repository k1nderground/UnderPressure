using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FireScript : MonoBehaviour
{
    [SerializeField] PointSystem pointSystem;
    [SerializeField] int speed;
    [SerializeField] bool isGoing = false;

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            pointSystem.endGame();
            SceneManager.LoadScene("Lose");
        }
    }

    void FixedUpdate(){
        if(isGoing){
        Vector3 vec = new Vector3(0, 0, -1);
        transform.position += vec*speed*Time.deltaTime;
        }
    }

    void Start(){
        StartCoroutine(init());
    }

    IEnumerator init(){
        yield return new WaitForSeconds(3);
        isGoing = true;
    }
}
