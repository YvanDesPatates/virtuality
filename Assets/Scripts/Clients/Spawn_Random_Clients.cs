using UnityEngine;
using System.Linq;

public class Spawn_Random_Clients : MonoBehaviour, I_ClientPlaceIsFreeEventReceiver
{
    public Transform[] spawnPoints;
    public GameObject clientPrefab;
    public Transform clientPathParent;

    void Start()
    {
        ClientPlaceIsFreeSingleton.Subscribe(this);
    }

    public void OnClientPlaceIsFree(ClientPlaceToTakeElixir clientPlace)
    {
        SpawnClient(clientPlace.GetPositionWhereClientHasToGo());
    }

    void SpawnClient(Transform finalTargetPosition)
    {
        if (spawnPoints.Length == 0 || clientPrefab == null)
        {
            Debug.LogWarning("No spawn points or client prefab assigned.");
            return;
        }

        // Randomize the spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newClient = Instantiate(clientPrefab, spawnPoint.position, spawnPoint.rotation);

        // Randomize the size of the client
        ramdomizeSizeClient(newClient);
        // Randomize the client appearance
        ramdomizeClient(newClient);

        // Set up the client mover
        ClientMoverSetup(newClient, finalTargetPosition);
    }

    void ramdomizeClient(GameObject client)
    {
        FishManDemoLP fishManDemo = client.GetComponentInChildren<FishManDemoLP>();
        if (fishManDemo != null)
        {
            Debug.Log("Randomizing FishManDemoLP for new client.");
            fishManDemo.Randomize();
        }
    }

    void ramdomizeSizeClient(GameObject client)
    {
        float randomScale = Random.Range(1.2f, 2.2f);
        client.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    void ClientMoverSetup(GameObject client, Transform finalTargetPosition)
    {
        ClientMover mover = client.GetComponent<ClientMover>();
        if (mover != null && clientPathParent != null)
        {
            Transform[] targets = clientPathParent.GetComponentsInChildren<Transform>()
                                                .Where(t => t != clientPathParent)
                                                .Concat(new[] { finalTargetPosition })
                                                .ToArray();

            Debug.Log($"Found {targets.Length} targets for client movement.");
            mover.targets = targets;
        }
    }
}
