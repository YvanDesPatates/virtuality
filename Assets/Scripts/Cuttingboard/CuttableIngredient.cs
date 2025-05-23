using UnityEngine;

public class CuttableIngredient : MonoBehaviour
{
    [TextArea] [Tooltip("Doesn't do anything. Just comments shown in inspector. Don't change it.")]
    public string Notes = "This component should be on the same gameObject that a collider with isTrigger = true.\n " +
                          "This component is used to detect when the ingredient is cutted by a knife.\n";
    
    private CuttingBoardController cuttingBoard;

    public void SetCuttingBoard(CuttingBoardController cuttingBoard)
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
