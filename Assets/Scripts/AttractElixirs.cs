using UnityEngine;

public class AttractElixirs : AbstractGrabEventReceiver
{
    [SerializeField] private float attractionSpeed = 1f;
    [Tooltip("higher the value is, less the direction of the ingredient will be straight to the point")]
    [SerializeField] private float fluctuationCoefficient = 0.2f;
    [SerializeField] private Transform positionToAttractTo;

    /// <summary>
    /// if actualGrabInteractable is not null, it means that an ingredient
    /// is currently being attracted or on position on the cutting board
    /// </summary>
    private PersonalizedGrabInteractable actualIngredientGrabInteractable;
    private Rigidbody actualIngredientRigidbody;
    private Transform actualIngredientTransform;

    /// <summary>
    /// Destroy the current elixir and return its IngredientType.
    /// </summary>
    /// <returns> the IngredientType of the elixir, null if there is no current elixir</returns>
    public IngredientType? TakeElixir()
    {
        if (actualIngredientGrabInteractable is null) return null;
        IngredientType ingredientType = actualIngredientTransform.gameObject.GetComponent<AbstractIngredient>().GetIngredientType();
        var elixirToDestroy = actualIngredientTransform.gameObject;
        ReleaseIngredient();
        Destroy(elixirToDestroy);
        return ingredientType;
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
}