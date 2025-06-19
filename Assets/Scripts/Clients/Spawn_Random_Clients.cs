using UnityEngine;
using System.Linq;

public class Spawn_Random_Clients : MonoBehaviour, I_ClientPlaceIsFreeEventReceiver
{
    public Transform[] spawnPoints;
    public GameObject clientPrefab;
    public float spawnInterval = 3f;
    public Transform clientPathParent;

    public float ClientNumber = 4f;

    private float timer = 0f;


    void Start()
    {
        ClientPlaceIsFreeSingleton.Subscribe(this);
    }

    void Update()
    {

    }

    public void OnClientPlaceIsFree(ClientPlaceToTakeElixir clientPlace)
    {
        Debug.Log("Client place is free, spawning client.");
        SpawnClient();
    }

    void SpawnClient()
    {
        if (spawnPoints.Length == 0 || clientPrefab == null)
        {
            Debug.LogWarning("No spawn points or client prefab assigned.");
            return;
        }

        Transform spawnPoint = spawnPoints[1];
        GameObject newClient = Instantiate(clientPrefab, spawnPoint.position, Quaternion.identity);

        ClientMover mover = newClient.GetComponent<ClientMover>();
        if (mover != null && clientPathParent != null)
        {
            Transform[] targets = clientPathParent.GetComponentsInChildren<Transform>()
                                                  .Where(t => t != clientPathParent)
                                                  .ToArray();

            Debug.Log($"Found {targets.Length} targets for client movement.");
            mover.targets = targets;
        }
    }
}
