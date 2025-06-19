using System.Collections;
using UnityEngine;

public class fakespawner : MonoBehaviour, I_ClientPlaceIsFreeEventReceiver
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClientPlaceIsFreeSingleton.Subscribe(this);
    }
    

    public void OnClientPlaceIsFree(ClientPlaceToTakeElixir clientPlace)
    {
        StartCoroutine(SpawnClient(clientPlace));
    }
    

    private IEnumerator SpawnClient(ClientPlaceToTakeElixir clientPlace)
    {
        yield return new WaitForSeconds(5);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var client = Instantiate(sphere, clientPlace.GetPositionWhereClientHasToGo());
        var clientScript = client.AddComponent<fakeClient>();
        clientScript.clientPlace = clientPlace;
    }
}
