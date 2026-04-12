using UnityEngine;

public class CoinPickupScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
        PointSystem.addCoin();
        Destroy(gameObject);
        }
    }
}
