using System;
using UnityEngine;

public class CuttableIngredient : MonoBehaviour
{
    [TextArea] [Tooltip("Doesn't do anything. Just comments shown in inspector. Don't change it.")]
    public string Notes = "This component should be on the same gameObject that a collider with isTrigger = true.\n " +
                          "This component is used to detect when the ingredient is cutted by a knife.\n";
    [Tooltip("The higher parent object of the ingredient. Used to destroy the ingredient when it is cutted. if null, it is set to the gameObject of this component")]
    [SerializeField] private GameObject parentObject;
    
    private CuttingBoardController cuttingBoard;
    private BeforeDestroyCallback beforeDestroyCallback;
    public delegate void BeforeDestroyCallback();

    private void Awake()
    {
        if (parentObject == null)
        {
            parentObject = gameObject;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (cuttingBoard is not null && other.CompareTag("Knife"))
        {
            cuttingBoard.OnIngredientCutted(parentObject);
        }
    }

    private void OnDestroy()
    {
        beforeDestroyCallback?.Invoke();
    }

    public void SetCuttingBoard(CuttingBoardController cuttingBoard)
    {
        this.cuttingBoard = cuttingBoard;
    }
    
    public void SetBeforeDestroyCallback(BeforeDestroyCallback callback)
    {
        beforeDestroyCallback = callback;
    }
}
