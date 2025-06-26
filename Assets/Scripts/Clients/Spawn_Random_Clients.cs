using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Spawn_Random_Clients : MonoBehaviour, I_ClientPlaceIsFreeEventReceiver, IEndOfTutoToDoList
{
    public Transform[] spawnPoints;
    public GameObject clientPrefab;
    public Transform clientArrivalPath;
    [FormerlySerializedAs("clientDestinationPath")] public Transform clientDeparturePath;
    
    private bool canSpawn = false;
    private HashSet<ClientPlaceToTakeElixir> freeClientPLaces = new();

    void Start()
    {
        ClientPlaceIsFreeSingleton.Subscribe(this);
        StepTracker.SubscribeToEndOfTuto(this);
    }
    
    void Update()
    {
        if (canSpawn && freeClientPLaces.Count > 0)
        {
            ClientPlaceToTakeElixir clientPlace = freeClientPLaces.First();
            freeClientPLaces.Remove(clientPlace);
            SpawnClient(clientPlace);
            StartCoroutine(StartSpawnerCooldown());
        }
    }

    public void OnClientPlaceIsFree(ClientPlaceToTakeElixir clientPlace)
    {
        freeClientPLaces.Add(clientPlace);
    }

    public void OnTutoToDoListIsCompleted()
    {
        canSpawn = true;
    }

    void SpawnClient(ClientPlaceToTakeElixir clientPLace)
    {
        if ( ! canSpawn) return;

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
        ClientControllerSetup(newClient, clientPLace);
    }

    void ramdomizeClient(GameObject client)
    {
        FishManDemoLP fishManDemo = client.GetComponentInChildren<FishManDemoLP>();
        if (fishManDemo != null)
        {
            fishManDemo.Randomize();
        }
    }

    void ramdomizeSizeClient(GameObject client)
    {
        float randomScale = Random.Range(1.2f, 2.2f);
        client.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    void ClientControllerSetup(GameObject client, ClientPlaceToTakeElixir clientPlace)
    {
        Transform finalTargetPosition = clientPlace.GetPositionWhereClientHasToGo();
        ClientController clientController = client.GetComponent<ClientController>();
        clientController.SetPlaceToTakeElixir(clientPlace);

        Transform[] arrivalTargets = clientArrivalPath.GetComponentsInChildren<Transform>()
            .Where(t => t != clientArrivalPath)
            .Concat(new[] { finalTargetPosition })
            .ToArray();

        clientController.SetArrivalPathTargets(arrivalTargets);
        
        Transform[] departureTargets = clientDeparturePath.GetComponentsInChildren<Transform>()
            .Where(t => t != clientDeparturePath)
            .ToArray();
        clientController.SetDeparturePathTargets(departureTargets);
    }

    private IEnumerator StartSpawnerCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(10f);
        canSpawn = true;
    }
    
}