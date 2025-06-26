using System.Collections.Generic;
using UnityEngine;

public class StepTracker : MonoBehaviour
{
    public static StepTracker Instance { get; private set; }

    [SerializeField] private ToDoListController toDoListController;

    public List<StepData> StepInfo { get; private set; } = new();
    private List<IEndOfTutoToDoList> subscribers = new();
    private int currentStepIndex = 0;

    public static void SubscribeToEndOfTuto(IEndOfTutoToDoList subscriber)
    {
        if (Instance == null) return;
        Instance.subscribers.Add(subscriber);
    }
    
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
        StepInfo.Add(new StepData(StepType.AddBone, "- mettre un os dans le chaudron"));
        StepInfo.Add(new StepData(StepType.CutWatermelon, "- couper une pasteque avec le couteau"));
        StepInfo.Add(new StepData(StepType.PutSlice, "- mettre la tranche dans le chaudron"));
        StepInfo.Add(new StepData(StepType.StirMixture, "- touiller la mixture avec la cuillere"));
        StepInfo.Add(new StepData(StepType.FillFlask, "- mettre la potion dans une fiole vide"));
        StepInfo.Add(new StepData(StepType.AddWatermelon, "- mettre une pasteque dans le chaudron"));
        StepInfo.Add(new StepData(StepType.StirMixture, "- touiller le liquide avec la cuillere"));
        StepInfo.Add(new StepData(StepType.PullLever, "- tirer la poignee pour vider le chaudron"));
        StepInfo.Add(new StepData(StepType.FindRecipe, "- trouver une recette de potion"));
    }

    public void StepCompleted(StepType step)
    {
        if (currentStepIndex >= StepInfo.Count) return;

        StepData data = StepInfo[currentStepIndex];
        if (data.stepType == step)
        {
            toDoListController.UpdateToDoList();
            currentStepIndex++;
            if (currentStepIndex >= StepInfo.Count)
            {
                foreach (var subscriber in subscribers)
                {
                    subscriber.OnTutoToDoListIsCompleted();
                }
            }
        }
    }

    public int GetCurrentStepIndex() => currentStepIndex;
}