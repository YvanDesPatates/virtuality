using UnityEngine;

public class IngredientsDestructor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {
            Destroy(other.gameObject);
        }
    }
}
