using UnityEngine;

public class CuttableIngredient : MonoBehaviour
{
    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector. Don't change it.")]
    public string Notes = "This component should be on the same gameObject that the colliders of the ingredient";
    
    private CuttingBoardController cuttingBoard;

    public void SetupCuttingBoard(CuttingBoardController cuttingBoard)
    {
        this.cuttingBoard = cuttingBoard;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (cuttingBoard is not null && other.CompareTag("Knife"))
        {
            cuttingBoard.OnIngredientCutted(this);
        }
    }
}
