using System.Collections;
using UnityEngine;

public class CuttingBoardController : MonoBehaviour
{
    [Tooltip("The number of knife cuts that the ingredient should receive before being cutted")]
    [SerializeField] private int maxNbKnifeCut = 5;
    [Tooltip("in seconds. If you choose 1, the knife cuts count will be reset every second")]
    [SerializeField] private float maxTimeBetweenCuts = 1.5f;
    
    private int currentNbKnifeCut = 0;
    
    /// <summary>
    /// Trigger this method when the ingredient receive a knife cut.
    /// </summary>
    /// <param name="ingredient">The higher parent of the ingredient that is cutted. Used to destroy the ingredient</param>
    public void OnIngredientCutted(GameObject ingredient)
    {
        StopAllCoroutines();
        currentNbKnifeCut++;
        if (currentNbKnifeCut == maxNbKnifeCut)
        {
            CutIngredient(ingredient);
            return;
        }

        StartCoroutine(ResetKnifeCutsCoroutine());
    }
    
    private IEnumerator ResetKnifeCutsCoroutine()
    {
        yield return new WaitForSeconds(maxTimeBetweenCuts);
        ResetNbKnifeCuts();
    }
    
    private void ResetNbKnifeCuts()
    {
        currentNbKnifeCut = 0;
    }

    private void CutIngredient(GameObject ingredient)
    {
        Destroy(ingredient);
        ResetNbKnifeCuts();
    }
}
