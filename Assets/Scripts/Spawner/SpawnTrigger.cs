using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    private bool used = false;

    [HideInInspector] public GameObject previousTerrain;
    [HideInInspector] public SpawnerScript spawner;

    public void Init(SpawnerScript spawnerScript, GameObject prev)
    {
        spawner = spawnerScript;
        previousTerrain = prev;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (other.CompareTag("Player"))
        {
            used = true;

            spawner.SpawnTerrains(3);

            if (previousTerrain != null)
                Destroy(previousTerrain);
        }
    }
}