using UnityEngine;
using System.Collections;

public class CaldronShaderController : MonoBehaviour
{

    [SerializeField] private Material caldronLiquidMaterial;
    [SerializeField] private float startLiquidAnimationDuration = 2f;

    private Coroutine resetCoroutine;
    private float normalAnimationSpeed = 0.5f;
    private float stirringAnimationSpeed = 2f;

    void Start()
    {
        ResetCaldronShader();
    }

    public void OnIngredientAdded()
    {
        if(caldronLiquidMaterial.GetFloat("_Alpha") == 0){
            StartCoroutine(FadeAlpha(0f, 100f, startLiquidAnimationDuration));
        }
    }

    public void OnCaldronEmptied()
    {
        ResetCaldronShader();
    }

    public void OnStirringMovement()
    {
        BoostSpeed();

    }

    private void BoostSpeed()
    {
        caldronLiquidMaterial.SetFloat("_Speed", stirringAnimationSpeed);

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        resetCoroutine = StartCoroutine(ReduceSpeedAfterDelay());
    }

    private IEnumerator ReduceSpeedAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        caldronLiquidMaterial.SetFloat("_Speed", normalAnimationSpeed);
        resetCoroutine = null;
    }

    private void ResetCaldronShader()
    {
        caldronLiquidMaterial.SetFloat("_Alpha", 0);
        caldronLiquidMaterial.SetFloat("_Speed", normalAnimationSpeed);
    }

    private IEnumerator FadeAlpha(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            t = Mathf.Pow(t, 10);

            float alpha = Mathf.Lerp(startValue, endValue, t);

            caldronLiquidMaterial.SetFloat("_Alpha", alpha);

            yield return null;
        }

        caldronLiquidMaterial.SetFloat("_Alpha", endValue);
    }
}
