using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class toDoListController : MonoBehaviour
{

    [SerializeField] private TextMeshPro firstToDo;
    [SerializeField] private TextMeshPro secondToDo;
    [SerializeField] private TextMeshPro thirdToDo;

    private List<TextMeshPro> toDoPapers;

    string[] toDoList = {
        "- mettre une pasteque dans le chaudron",
        "- mettre un os dans le chaudron",
        "- touiller la mixture avec la cuillere",
        "- mettre la potion dans une fiole vide",
        "- tirer la poignée pour vider le chaudron",
        "- couper une pastèque avec le couteau" };

    private int stepIndex = 0;
    private int currentBlockStart = 0;
    private int localStrikeIndex = 0;


    private IEnumerator Start()
    {
        toDoPapers = new List<TextMeshPro> { firstToDo, secondToDo, thirdToDo };

        yield return new WaitForSeconds(1.5f);
        DelayBeforeNextBlock();
        UpdateDisplayedTasks();
    }

    public void UpdateToDoList()
    {
        if (stepIndex >= toDoList.Length)
        {
            // supprimer les bouts de papier
            return;
        }

        // Strike through completed task
        string currentText = toDoPapers[localStrikeIndex].text;
        currentText = RemoveStrikeThroughTags(currentText);
        toDoPapers[localStrikeIndex].text = "<s>" + currentText + "</s>";

        stepIndex++;
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

        if (taskIndex < toDoList.Length)
        {
            TextMeshPro tmp = toDoPapers[i];
            tmp.alpha = 0f;
            tmp.text = toDoList[taskIndex];
            yield return StartCoroutine(FadeInTMP(tmp, 1f)); // Fade + attente
            yield return new WaitForSeconds(0.2f); // Petit délai entre chaque
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
