using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class in charge of attracting elixirs to the client's place and being an interface with client object.
/// </summary>
public class ClientPlaceToTakeElixir : AbstractGrabEventReceiver
{
    [SerializeField] private float attractionSpeed = 1f;
    [Tooltip("higher the value is, less the direction of the ingredient will be straight to the point")]
    [SerializeField] private float fluctuationCoefficient = 0.2f;
    [SerializeField] private Transform positionToAttractTo;
    [SerializeField] private Transform positionWhereClientHasToGo;
    [SerializeField] private SuccessAndFailEffectsPlayer successAndFailEffects;

    /// <summary>
    /// if actualGrabInteractable is not null, it means that an ingredient
    /// is currently being attracted or on position on the cutting board
    /// </summary>
    private PersonalizedGrabInteractable actualIngredientGrabInteractable;
    private Rigidbody actualIngredientRigidbody;
    private Transform actualIngredientTransform;
    
    private bool elixirIsReady;
    private List<ElixirIsReadySubscriber> _subscribers = new();

    /// <summary>
    /// Destroy the current elixir and play a success or failed effect if the elixir is the same type as passed in parameter
    /// </summary>
    /// <returns> true if the actual elixir has the same ingredientType as passed in parameter, false if types are different or if ther is no actual elixir.</returns>
    public bool TakeElixir(IngredientType ingredientType)
    {
        if (actualIngredientGrabInteractable is null) return false;
        
        IngredientType actualingredientType = actualIngredientTransform.gameObject.GetComponent<AbstractIngredient>().GetIngredientType();
        bool isSameType = actualingredientType == ingredientType;
        var elixirToDestroy = actualIngredientTransform.gameObject;
        ReleaseIngredient();
        Destroy(elixirToDestroy);
        
        ClientPlaceIsFreeSingleton.OnClientPlaceIsFree(this);

        if (isSameType)
        {
            successAndFailEffects.PlaySuccessEffectsCoroutine(5);
        }
        else
        {
            successAndFailEffects.PlayFailEffectsCoroutine(5);
        }
        
        return isSameType;
    }

    public void Subscribe(ElixirIsReadySubscriber subscriber)
    {
        _subscribers.RemoveAll(subscriber => subscriber is null);
        _subscribers.Add(subscriber);
        if (elixirIsReady)
        {
            subscriber.OnElixirIsReady();
        }
        else
        {
            subscriber.OnElixirIsNotReady();
        }
    }
    
    public Transform GetPositionWhereClientHasToGo()
    {
        return positionWhereClientHasToGo;
    }

    private void Start()
    {
        ClientPlaceIsFreeSingleton.OnClientPlaceIsFree(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (actualIngredientGrabInteractable is not null) return;
        
        var grabInteractable = other.GetComponent<PersonalizedGrabInteractable>();
        var ingredient = other.GetComponent<AbstractIngredient>();
        if (grabInteractable is not null && !grabInteractable.IsGrabbed() && ingredient is not null && ingredient.IsElixir())
        {
            AttractNewElixir(other.gameObject);
        }
    }

    private void Update()
    {
        // Déplacement de l'ingrédient vers la position cible avec fluctuation
        if (actualIngredientRigidbody is not null && actualIngredientTransform.position != positionToAttractTo.position)
        {
            float fluctuationX = Mathf.Sin(Time.time * 2f) * fluctuationCoefficient;
            float fluctuationZ = Mathf.Cos(Time.time * 2f) * fluctuationCoefficient;
            Vector3 fluctuatedPosition = positionToAttractTo.position + new Vector3(fluctuationX, 0, fluctuationZ);

            if (Vector3.Distance(actualIngredientTransform.position, positionToAttractTo.position) < fluctuationCoefficient)
            {
                fluctuatedPosition = positionToAttractTo.position;
                elixirIsReady = true;
                NotifySubscribers();
            }

            actualIngredientTransform.position = Vector3.Lerp(actualIngredientTransform.position, fluctuatedPosition,
                Time.deltaTime * attractionSpeed);
        }

        // Rotation douce vers l'orientation neutre
        if (actualIngredientTransform is not null)
        {
            Quaternion targetRotation = Quaternion.identity; // Orientation neutre
            actualIngredientTransform.rotation = Quaternion.Lerp(actualIngredientTransform.rotation, targetRotation, Time.deltaTime * attractionSpeed);
        }
    }

    private void AttractNewElixir(GameObject ingredientToGrab)
    {
        actualIngredientGrabInteractable = ingredientToGrab.GetComponent<PersonalizedGrabInteractable>();
        actualIngredientRigidbody = ingredientToGrab.GetComponent<Rigidbody>();
        actualIngredientTransform = ingredientToGrab.GetComponent<Transform>();
        
        actualIngredientGrabInteractable.SubscribeToGrabEvents(this);
        actualIngredientRigidbody.isKinematic = true;
    }
    
    private void ReleaseIngredient()
    {
        actualIngredientGrabInteractable = null;
        actualIngredientRigidbody = null;
        actualIngredientTransform = null;
        elixirIsReady = false;
        NotifySubscribers();
    }

    public override void OnGrabExit(PersonalizedGrabInteractable interactable)
    {
        interactable.UnsubscribeToGrabEvents(this);
        interactable.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public override void OnGrabEnter(PersonalizedGrabInteractable interactable)
    {
        if (actualIngredientGrabInteractable == interactable)
        {
            ReleaseIngredient();
        }
    }

    private void NotifySubscribers()
    {
        _subscribers.RemoveAll(subscriber => subscriber is null);
        
        foreach (var subscriber in _subscribers)
        {
            if (elixirIsReady)
            {
                subscriber.OnElixirIsReady();
            }
            else
            {
                subscriber.OnElixirIsNotReady();
            }
        }
    }

}