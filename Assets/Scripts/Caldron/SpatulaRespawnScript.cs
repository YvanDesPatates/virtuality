using UnityEngine;

public class SpatulaRespawnScript : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Caldron_Spatula"))
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            other.GetComponent<Transform>().position = spawnPoint.position;
            rigidbody.isKinematic = false;
        }
    }
}
