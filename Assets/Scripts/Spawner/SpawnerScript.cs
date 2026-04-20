using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] terrains;

    private Transform nextSpawnPoint;
    private GameObject lastTerrain;

    private void Start()
    {
        nextSpawnPoint = transform;
        SpawnTerrains(4);
    }

    public void SpawnTerrains(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, terrains.Length);

            GameObject newTerrain = Instantiate(
                terrains[randomIndex],
                nextSpawnPoint.position,
                Quaternion.identity
            );

            SpawnTrigger trigger =
                newTerrain.GetComponentInChildren<SpawnTrigger>();

            if (trigger != null)
                trigger.Init(this, lastTerrain);

            lastTerrain = newTerrain;

            Transform pivot = newTerrain.transform.Find("Pivot");

            if (pivot != null)
                nextSpawnPoint = pivot;
        }
    }
}