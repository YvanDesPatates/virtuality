using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToDoListController : MonoBehaviour
{

    [SerializeField] private TextMeshPro firstToDo;
    [SerializeField] private TextMeshPro secondToDo;
    [SerializeField] private TextMeshPro thirdToDo;

    private List<TextMeshPro> toDoPapers;

    private int currentBlockStart = 0;
    private int localStrikeIndex = 0;


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
            return;
        }

        // Strike through completed task
        string currentText = toDoPapers[localStrikeIndex].text;
        currentText = RemoveStrikeThroughTags(currentText);
        StartCoroutine(AnimateStrike(toDoPapers[localStrikeIndex]));
        localStrikeIndex++;

        if (localStrikeIndex >= 3)
        {
            localStrikeIndex = 0;
            currentBlockStart += 3;
            StartCoroutine(DelayBeforeNextBlock());
        }
    }


    private void UpdateDisplayedTasks()
    {
        StartCoroutine(SequentialFadeInTasks());
    }


    private IEnumerator DelayBeforeNextBlock()
    {
        yield return new WaitForSeconds(1f);
        UpdateDisplayedTasks();
    }


    private IEnumerator SequentialFadeInTasks()
    {
        for (int i = 0; i < toDoPapers.Count; i++)
        {
            int taskIndex = currentBlockStart + i;

            if (taskIndex < StepTracker.Instance.StepInfo.Count)
            {
                string description = StepTracker.Instance.StepInfo[taskIndex].description;

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
    

    private IEnumerator AnimateStrike(TextMeshPro tmp)
    {
        string original = RemoveStrikeThroughTags(tmp.text);
        for (int i = 0; i <= original.Length; i++)
        {
            string partialStrike = "<s>" + original.Substring(0, i) + "</s>" + original.Substring(i);
            tmp.text = partialStrike;
            yield return new WaitForSeconds(0.04f);
        }
    }

}
