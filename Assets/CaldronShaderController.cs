using UnityEngine;
using System.Collections;

public class CaldronShaderController : MonoBehaviour
{

    [SerializeField] private Material caldronLiquidMaterial;
    [SerializeField] private float duration = 2f;

    void Start()
    {
        caldronLiquidMaterial.SetFloat("_Alpha", 0);
    }

    public void OnIngredientAdded()
    {
        StartCoroutine(FadeAlpha(0f, 100f, 2f));
    }

    IEnumerator FadeAlpha(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            t = Mathf.Pow(t, 2);

            float alpha = Mathf.Lerp(startValue, endValue, t);

            caldronLiquidMaterial.SetFloat("_Alpha", alpha);

            yield return null;
        }

        caldronLiquidMaterial.SetFloat("_Alpha", endValue);
    }

    public void OnStirringMovement()
    {
        
    }
}
