using UnityEngine;

public class Dispawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Détruit l'objet après 10 secondes
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
