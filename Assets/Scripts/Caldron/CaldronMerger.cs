using System.Collections;
using UnityEngine;

public class CaldronMerger : MonoBehaviour
{
    [Tooltip("number of half turn the spatula has to do before merging the ingredients")]
    [SerializeField] private int nbHalfTurnToMerge = 6;
    [Space]
    [SerializeField] private SpatulaDetection spatulaDetection;
    [SerializeField] private Transform caldronTransform;
    [SerializeField] private AudioSource waterEmptyingSound;
    [SerializeField] private SuccessAndFailEffectsPlayer successAndFailEffects;
    [SerializeField] private ParticleSystem bubblesParticle;
    private ParticleSystem.MainModule particle;
    public CaldronShaderController caldronShaderController;
    
    private readonly IngredientList _ingredients = new();
    private RecipesManager _recipesManager;
    private GameObject _recipeResult;
    private bool _caldronIsOccupiedByBadRecipe = false;
    
    private void Awake()
    {
        _recipesManager = Util.FindObjectOfTypeOrLogError<RecipesManager>();
        spatulaDetection.InitNbHalfTurnsToMerge(nbHalfTurnToMerge);
        StopBubbles(5);
    }

    /// <summary>
    /// when called, all the ingredients are merged into the corresponding recipe result, if there is one.
    /// </summary>
    public void FinishRecipe()
    {
        if (_recipeResult is not null || _ingredients.IsEmpty()) return;
        
        var ingredientList = _ingredients.AddIngredient(IngredientType.Caldron);
        var ingredientResult = _recipesManager.GetRecipeResult(ingredientList);
        if (ingredientResult is not null)
        {
            OnRecipeSuccess(ingredientResult);
        }
        else
        {
            OnRecipeFail();
        }
    }

    public void OnRotationMaxAngleReached()
    {
        Empty();
    }

    /**
     * trigger this event when the spatula has made one more turn in the caldron when stirring.
     */
    public void OnSpatulaTurnedOneMore()
    {
        particle = bubblesParticle.main;
        particle.maxParticles = particle.maxParticles + 5;
    }

    /// <summary>
    /// each time an ingredient enters the caldron, it is added to the list of ingredients.
    /// if there is a recipe result waiting to be transferred in a bottle, recipe is set to null.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Empty_Bottle"))
        {
            ReplaceEmptyBottleWithLastRecipe(other.gameObject);
            return;
        }

        if (CaldronIsNotAvailable()) return;

        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {   
            PersonalizedGrabInteractable personalizedGrabInteractable = other.GetComponent<PersonalizedGrabInteractable>();
            if (personalizedGrabInteractable is not null && personalizedGrabInteractable.IsGrabbed()) return;
            
            spatulaDetection.ResetNbHalfTurns();
            caldronShaderController.OnIngredientAdded();
            if (_ingredients.IsEmpty())
            {
                particle = bubblesParticle.main;
                particle.maxParticles = 2;
            }
            _ingredients.AddIngredient(abstractIngredient.GetIngredientType());
            _recipeResult = null;
            successAndFailEffects.StopSuccessEffects();
            successAndFailEffects.StopFailEffects();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {
            spatulaDetection.ResetNbHalfTurns();
            _ingredients.RemoveIngredient(abstractIngredient.GetIngredientType());
        }
    }

    private void OnRecipeSuccess(GameObject ingredientResult)
    {
        _recipeResult = ingredientResult;
        successAndFailEffects.PlaySuccessSoundAndEffects();
        _ingredients.Clear();
    }
    
    private void OnRecipeFail()
    {
        _ingredients.Clear();
        _caldronIsOccupiedByBadRecipe = true;
        successAndFailEffects.PlayFailSoundAndEffects(true);
    }

    private void Empty()
    {
        _ingredients.Clear();
        _recipeResult = null;
        _caldronIsOccupiedByBadRecipe = false;
        waterEmptyingSound.Play();
        StopBubbles();
        caldronShaderController.OnCaldronEmptied();
        successAndFailEffects.StopFailEffects();
        successAndFailEffects.StopSuccessEffects();
    }

    /// <summary>
    /// fill a bottle with the last recipe result. Set the recipe result to null because a recipe fill only one bottle.
    /// </summary>
    private void ReplaceEmptyBottleWithLastRecipe(GameObject emptyBottle)
    {
        if (_recipeResult is null) return;
        var grabInteractable = emptyBottle.GetComponent<PersonalizedGrabInteractable>();
        if (grabInteractable is null) return;

        var position = emptyBottle.transform.position;
        var rotation = emptyBottle.transform.rotation;
        var interactor = grabInteractable.DetachInteractor();
        Destroy(emptyBottle);
        var recipeResult = Instantiate(_recipeResult, position, rotation);
        recipeResult.GetComponent<PersonalizedGrabInteractable>().AttachInteractor(interactor);

        _recipeResult = null;
        caldronShaderController.OnCaldronEmptied();
        StopBubbles();
        successAndFailEffects.StopSuccessEffects();
    }

    private bool CaldronIsNotAvailable()
    {
        return _recipeResult is not null || _caldronIsOccupiedByBadRecipe;
    }
    
    private IEnumerator StartBubblesAfterDelay(float delayInS)
    {
        yield return new WaitForSeconds(delayInS);
        bubblesParticle.Play();
    }

    private void StopBubbles(int delayInS = 6)
    {
        bubblesParticle.Stop(true);
        particle = bubblesParticle.main;
        particle.maxParticles = 2;
        StartCoroutine(StartBubblesAfterDelay(delayInS));
    }
}
