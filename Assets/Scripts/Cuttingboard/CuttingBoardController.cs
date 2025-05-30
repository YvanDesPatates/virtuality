using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoardController : MonoBehaviour
{
    [Tooltip("The number of knife cuts that the ingredient should receive before being cutted")]
    [SerializeField] private int maxNbKnifeCut = 5;
    [Tooltip("in seconds. If you choose 1, the knife cuts count will be reset every second")]
    [SerializeField] private float maxTimeBetweenCuts = 1.5f;
    [Space]
    [SerializeField] private AttractIngredients attractIngredientsScript;
    [SerializeField] private List<GameObject> cuttingParticles;
    [SerializeField] private SuccessAndFailEffectsPlayer successAndFailEffects;
    
    private int currentNbKnifeCut = 0;
    private RecipesManager recipesManager;
    
    private void Awake()
    {
        recipesManager = Util.FindObjectOfTypeOrLogError<RecipesManager>();
    }
    
    /// <summary>
    /// Trigger this method when the ingredient receive a knife cut.
    /// </summary>
    /// <param name="ingredient">The higher parent of the ingredient that is cutted. Used to destroy the ingredient</param>
    public void OnIngredientCutted(GameObject ingredient)
    {
        StopAllCoroutines();
        currentNbKnifeCut++;
        //activate the next particle in the list
        foreach (var particle in cuttingParticles)
        {
            if ( ! particle.activeSelf)
            {
                particle.SetActive(true);
                break;
            }
        }
        if (currentNbKnifeCut == maxNbKnifeCut)
        {
            CutIngredient(ingredient);
            return;
        }

        StartCoroutine(ResetKnifeCutsCoroutine());
    }
    
    private IEnumerator ResetKnifeCutsCoroutine()
    {
        yield return new WaitForSeconds(maxTimeBetweenCuts);
        ResetNbKnifeCuts();
    }
    
    private void ResetNbKnifeCuts()
    {
        foreach (var particle in cuttingParticles)
        {
            particle.SetActive(false);
        }
        currentNbKnifeCut = 0;
    }

    private void CutIngredient(GameObject ingredient)
    {
        successAndFailEffects.StopSuccessEffects();
        var ingredientTransform = ingredient.transform;
        Destroy(ingredient);
        
        var ingredientList = new IngredientList()
            .AddIngredient(IngredientType.CuttingBoard)
            .AddIngredient(ingredient.GetComponent<AbstractIngredient>().GetIngredientType());
        var ingredientResult = recipesManager.GetRecipeResult(ingredientList);
        if (ingredientResult != null)
        {
            successAndFailEffects.PlaySuccessSoundAndEffects();
            Instantiate(ingredientResult, ingredientTransform.position, Quaternion.identity);
        }else
        {
            successAndFailEffects.PlayFailSoundAndEffects(false);
        }   
        
        ResetNbKnifeCuts();
    }
}
