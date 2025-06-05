using UnityEngine;

public class KnifeRespawnScript : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Knife"))
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody is null) return;
            
            rigidbody.isKinematic = true;
            other.GetComponent<Transform>().position = spawnPoint.position;
            rigidbody.isKinematic = false;
        }
    }
}
