using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterScript : MonoBehaviour
{
    [SerializeField] PointSystem pointSystem;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pointSystem.endGame();
            SceneManager.LoadScene("Lose");
        }
    }
}
