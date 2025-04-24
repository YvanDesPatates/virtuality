using UnityEngine;

public class SpatulaRespawnScript : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Caldron_Spatula"))
        {
            other.GetComponent<Transform>().position = spawnPoint.position;
        }
    }
}
