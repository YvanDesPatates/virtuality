using UnityEngine;

/// <summary>
/// Destroys the ingredient when it enters the trigger and is not grabbed.
/// </summary>
public class IngredientsDestructor : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {
            PersonalizedGrabInteractable grabInteractable = other.GetComponent<PersonalizedGrabInteractable>();
            if (grabInteractable is not null && !grabInteractable.IsGrabbed())
            {
                Destroy(other.gameObject);
            }
        }
    }
}
