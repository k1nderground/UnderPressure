using UnityEngine;

public class WaterPlayerFolower : MonoBehaviour
{
    [SerializeField] Transform Player;

    void Update()
    {
        Vector3 PlayerPos = new Vector3(Player.position.x, transform.position.y, Player.position.z );
        transform.position = PlayerPos;
    }
}
