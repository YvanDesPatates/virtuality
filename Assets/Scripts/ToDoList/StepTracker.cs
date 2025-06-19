using System.Collections.Generic;
using UnityEngine;

public class StepTracker : MonoBehaviour
{
    public static StepTracker Instance { get; private set; }

    [SerializeField] private ToDoListController toDoListController;

    public Dictionary<StepType, StepData> StepInfo { get; private set; } = new();
    private int currentStepIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        InitStepInfo();
    }

    private void InitStepInfo()
    {
        StepInfo[StepType.AddBone] = new StepData(0, "- mettre un os dans le chaudron");
        StepInfo[StepType.CutWatermelon] = new StepData(1, "- couper une pastèque avec le couteau");
        StepInfo[StepType.PutSlice] = new StepData(2, "- mettre la tranche dans le chaudron");
        StepInfo[StepType.StirMixture] = new StepData(3, "- touiller la mixture avec la cuillère");
        StepInfo[StepType.FillFlask] = new StepData(4, "- mettre la potion dans une fiole vide");
        StepInfo[StepType.AddWatermelon] = new StepData(5, "- mettre une pasteque dans le chaudron");
        StepInfo[StepType.StirMixture] = new StepData(6, "- touiller le liquide avec la cuillère");
        StepInfo[StepType.PullLever] = new StepData(7, "- tirer la poignée pour vider le chaudron");
        StepInfo[StepType.FindRecipe] = new StepData(8, "- trouver une recette de potion");
    }

    public void StepCompleted(StepType step)
    {
        if (StepInfo.TryGetValue(step, out var data) && data.Index == currentStepIndex)
        {
            Debug.Log("Step validé : " + step);
            toDoListController.UpdateToDoList();
            currentStepIndex++;
        }
        else
        {
            Debug.LogWarning("Step ignoré ou hors ordre : " + step);
        }
    }

    public int GetCurrentStepIndex() => currentStepIndex;
}