using UnityEngine;

public class CuttingBoardController : MonoBehaviour
{
    /// <summary>
    /// Trigger this method when the ingredient receive a knife cut.
    /// </summary>
    public void OnIngredientCutted(CuttableIngredient ingredient)
    {
        Debug.Log("CUTTED");
    }
}
