using UnityEngine;
using TMPro;

public class toDoListController : MonoBehaviour
{

    [SerializeField] private TextMeshPro firstToDo;
    [SerializeField] private TextMeshPro secondToDo;
    [SerializeField] private TextMeshPro thirdToDo;

    string[] toDoList = {
        "- mettre une pasteque dans le chaudron",
        "- mettre un os dans le chaudron",
        "- touiller la mixture avec la cuillere",
        "- mettre la potion dans une fiole vide",
        "- tirer la poignée pour vider le chaudron",
        "- couper une pastèque avec le couteau" };


    void Start()
    {
        firstToDo.text = toDoList[0];
        secondToDo.text = toDoList[1];
        thirdToDo.text = toDoList[2];
    }

}
