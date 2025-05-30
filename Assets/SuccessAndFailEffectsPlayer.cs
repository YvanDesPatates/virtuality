using UnityEngine;
using UnityEngine.Serialization;

public class SuccessAndFailEffectsPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource successSound;
    [SerializeField] private GameObject successParticle;
    [SerializeField] private AudioSource failSound;
    [SerializeField] private GameObject failParticle;
    
    public void PlaySuccessSoundAndEffects()
    {
        successSound.Play();
        successParticle.SetActive(true);
    }
    
    public void PlayFailSoundAndEffects()
    {
        failSound.Play();
        failParticle.SetActive(true);
    }
    
    public void StopSuccessEffects()
    {
        successParticle.SetActive(false);
    }
    
    public void StopFailEffects()
    {
        failParticle.SetActive(false);
    }
}
