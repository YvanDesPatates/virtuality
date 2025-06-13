using System.Collections;
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
        successParticle.gameObject.SetActive(false);
        successParticle.gameObject.SetActive(true);
        successParticle.Pause(true);
    }
    
    public void StopFailEffects()
    {
        failPermanentSmokeParticle.gameObject.SetActive(false);
        failPermanentSmokeParticle.gameObject.SetActive(true);
        failPermanentSmokeParticle.Pause(true);
    }

    public Coroutine PlaySuccessEffectsCoroutine(float durationInSec)
    {
        return StartCoroutine(SuccessEffectsCoroutine(durationInSec));
    }

    public Coroutine PlayFailEffectsCoroutine(float durationInSec)
    {
        return StartCoroutine(FailEffectCoroutine(durationInSec));
    }
    
    private IEnumerator SuccessEffectsCoroutine(float durationInSec)
    {
        PlaySuccessSoundAndEffects();
        yield return new WaitForSeconds(durationInSec);
        StopSuccessEffects();
    }

    private IEnumerator FailEffectCoroutine(float durationInSec)
    {
        PlayFailSoundAndEffects(true);
        yield return new WaitForSeconds(durationInSec);
        StopFailEffects();
    }
}
