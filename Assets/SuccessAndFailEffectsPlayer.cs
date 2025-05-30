using UnityEngine;
using UnityEngine.Serialization;

public class SuccessAndFailEffectsPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource successSound;
    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private AudioSource failSound;
    [SerializeField] private ParticleSystem failParticle;
    [SerializeField] private ParticleSystem failPermanentSmokeParticle;

    public void PlaySuccessSoundAndEffects()
    {
        successSound.Play();
        successParticle.Play(true);
    }

    public void PlayFailSoundAndEffects(bool playPermanentSmoke = false)
    {
        failSound.Play();
        failParticle.Play(true);
        if (playPermanentSmoke)
        {
            failPermanentSmokeParticle.Play(true);
        }
    }

    public void StopSuccessEffects()
    {
        successParticle.Pause(true);
    }
    
    public void StopFailEffects()
    {
        failPermanentSmokeParticle.Pause(false);
    }
}
