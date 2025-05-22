using UnityEngine;

public class SpatulaSound : MonoBehaviour
{
    [SerializeField] private AudioSource spatulaEnterWaterSound;
    [SerializeField] private AudioSource spatulaMoveWaterSound;
    
    private float _spatulaMoveWaterSoundVolume = 1f;

    private void Start()
    {
        _spatulaMoveWaterSoundVolume = spatulaMoveWaterSound.volume;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caldron_Spatula"))
        {
            SpatulaEnterWater();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Caldron_Spatula"))
        {
            SpatulaExitWater();
        }
    }
    
    private void SpatulaEnterWater()
    {
        StopAllCoroutines();
        spatulaEnterWaterSound.Play();
        spatulaMoveWaterSound.volume = _spatulaMoveWaterSoundVolume;
        spatulaMoveWaterSound.Play();
    }
    
    private void SpatulaExitWater()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(spatulaMoveWaterSound, 0.5f));
    }
    
    private System.Collections.IEnumerator FadeOutCoroutine(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
