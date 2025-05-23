using System;
using UnityEngine;

public class AttractIngredients : MonoBehaviour
{
    [SerializeField] private float attractionSpeed = 1f;
    [SerializeField] private Transform positionToAttractTo;

    /// <summary>
    /// if actualGrabInteractable is not null, it means that an ingredient
    /// is currently being attracted or on position on the cutting board
    /// </summary>
    private PersonalizedGrabInteractable actualIngredientGrabInteractable;
    private Rigidbody actualIngredientRigidbody;
    private Transform actualIngredientTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (actualIngredientGrabInteractable is not null) return;
        
        var grabInteractable = other.GetComponent<PersonalizedGrabInteractable>();
        if (grabInteractable != null && !grabInteractable.IsGrabbed())
        {
            AttractNewIngredient(other.gameObject);
        }
    }

    private void Update()
    {
        if (actualIngredientRigidbody is not null)
        {
            actualIngredientTransform.position = Vector3.Lerp(actualIngredientTransform.position, positionToAttractTo.position,
                Time.deltaTime * attractionSpeed);
        }
    }

    private void AttractNewIngredient(GameObject ingredientToGrab)
    {
        actualIngredientGrabInteractable = ingredientToGrab.GetComponent<PersonalizedGrabInteractable>();
        actualIngredientRigidbody = ingredientToGrab.GetComponent<Rigidbody>();
        actualIngredientTransform = ingredientToGrab.GetComponent<Transform>();
        
        actualIngredientRigidbody.isKinematic = true;
    }
}