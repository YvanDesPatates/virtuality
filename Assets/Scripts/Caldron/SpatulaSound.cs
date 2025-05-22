using UnityEngine;

public class SpatulaSound : MonoBehaviour
{
    [SerializeField] private AudioSource spatulaEnterWaterSound;
    [SerializeField] private AudioSource spatulaMoveWaterSound;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caldron_Spatula"))
        {
            spatulaEnterWaterSound.Play();
            spatulaMoveWaterSound.Play();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Caldron_Spatula"))
        {
            spatulaMoveWaterSound.Stop();
        }
    }
}
