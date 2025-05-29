using System.Collections;
using UnityEngine;

public class CuttingBoardKnifeSound : MonoBehaviour
{
    [Tooltip("The minimum velocity of the knife to play the sound")]
    [SerializeField] private float velocityKnifeThreshold = 1f;
    [SerializeField] private AudioSource knifeHitAudio;
    
    private bool canPlaySound = true;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound && collision.gameObject.CompareTag("Knife"))
        {
            var knifeRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (knifeRigidbody != null && knifeRigidbody.linearVelocity.magnitude > velocityKnifeThreshold)
            {
                PlayKnifeHitSound();
                StartCoroutine(NewSoundCooldown());
            }
        }
    }
    
    public void PlayKnifeHitSound()
    {
        if (knifeHitAudio != null)
        {
            knifeHitAudio.pitch = Random.Range(0.8f, 1.2f);
            knifeHitAudio.Play();
        }
    }
    
    //coroutine for cooldown of the sound : reset the canPlaySound variable after 0.5 seconds
    private IEnumerator NewSoundCooldown()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(0.2f);
        canPlaySound = true;
    }
    
}
