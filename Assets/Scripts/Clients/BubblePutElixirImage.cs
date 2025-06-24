using System.Collections.Generic;
using UnityEngine;

public class BubblePutElixirImage : MonoBehaviour
{
    [Tooltip("Set same index for elixirTypes and elixirImages")]
    [SerializeField] private List<IngredientType> elixirTypes;
    [Tooltip("Set same index for elixirTypes and elixirImages")]
    [SerializeField] private List<GameObject> elixirImagesPrefabs;

    [Space] [SerializeField] private GameObject parentImage;
    
    public void SetElixirImage(IngredientType elixirType)
    {
        if (elixirTypes.Contains(elixirType))
        {
            int index = elixirTypes.IndexOf(elixirType);
            if (index >= 0 && index < elixirImagesPrefabs.Count)
            {
                var imagePrefab = elixirImagesPrefabs[index];
                var newImage = Instantiate(imagePrefab, parentImage.transform);
            }
            else
            {
                Debug.LogWarning("Elixir type index out of range: " + index);
            }
        }
        else
        {
            Debug.LogWarning("Elixir type not found: " + elixirType);
        }
    }


}
