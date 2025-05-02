using UnityEngine;
using System.Collections;

public class CaldronShaderController : MonoBehaviour
{

    [SerializeField] private Material caldronLiquidMaterial;
    [SerializeField] private float duration = 2f;

    void Start()
    {
        ResetCaldronShader();
    }

    public void OnIngredientAdded()
    {
        if(caldronLiquidMaterial.GetFloat("_Alpha") == 0){
            StartCoroutine(FadeAlpha(0f, 100f, 2f));
        }
    }

    public void OnCaldronEmptied()
    {
        ResetCaldronShader();
    }

    public void OnStirringMovement()
    {
        caldronLiquidMaterial.SetFloat("_Speed", 2);
    }

    private void ResetCaldronShader()
    {
        caldronLiquidMaterial.SetFloat("_Alpha", 0);
        caldronLiquidMaterial.SetFloat("_Speed", 0.5f);
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
