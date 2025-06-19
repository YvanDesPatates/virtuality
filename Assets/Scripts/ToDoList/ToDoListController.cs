using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToDoListController : MonoBehaviour
{
    public static ToDoListController Instance { get; private set; }

    [SerializeField] private TextMeshPro firstToDo;
    [SerializeField] private TextMeshPro secondToDo;
    [SerializeField] private TextMeshPro thirdToDo;

    private List<TextMeshPro> toDoPapers;

    private int currentBlockStart = 0;
    private int localStrikeIndex = 0;


    private void Awake()
    {
        Instance = this;
    }


    private IEnumerator Start()
    {
        toDoPapers = new List<TextMeshPro> { firstToDo, secondToDo, thirdToDo };

        yield return new WaitForSeconds(2f);
        UpdateDisplayedTasks();
    }

    public void UpdateToDoList()
    {
        if (StepTracker.Instance.GetCurrentStepIndex() >= StepTracker.Instance.StepInfo.Count)
        {
            // supprimer les bouts de papier
            return;
        }

        // Strike through completed task
        string currentText = toDoPapers[localStrikeIndex].text;
        currentText = RemoveStrikeThroughTags(currentText);
        toDoPapers[localStrikeIndex].text = "<s>" + currentText + "</s>";
        localStrikeIndex++;

        if (localStrikeIndex >= 3)
        {
            localStrikeIndex = 0;
            currentBlockStart += 3;
            StartCoroutine(DelayBeforeNextBlock());
        }
    }


    private IEnumerator DelayBeforeNextBlock()
    {
        yield return new WaitForSeconds(1f);
        UpdateDisplayedTasks();
    }


    private void UpdateDisplayedTasks()
    {
        StartCoroutine(SequentialFadeInTasks());
    }


    private IEnumerator SequentialFadeInTasks()
{
    for (int i = 0; i < toDoPapers.Count; i++)
    {
        int taskIndex = currentBlockStart + i;

        if (taskIndex < StepTracker.Instance.StepInfo.Count)
        {
            StepType step = (StepType)taskIndex;
            string description = StepTracker.Instance.StepInfo[step].Description;
            
            TextMeshPro tmp = toDoPapers[i];
            tmp.alpha = 0f;
            tmp.text = description;

            yield return StartCoroutine(FadeInTMP(tmp, 1f));
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            toDoPapers[i].text = "";
        }
    }
}


    private IEnumerator FadeInTMP(TextMeshPro tmp, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            tmp.alpha = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }
        tmp.alpha = 1f;
    }


    private string RemoveStrikeThroughTags(string input)
    {
        return input.Replace("<s>", "").Replace("</s>", "");
    }

}
