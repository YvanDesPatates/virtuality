using UnityEngine;

public static class Util
{
    public static T FindObjectOfTypeOrLogError<T>() where T : Object
    {
        var found = Object.FindObjectOfType<T>();
        if (found is null)
        {
            Debug.LogError($"{typeof(T).Name} not found in the scene");
        }
        return found;
    }
    
    public static T GetComponentOrLogError<T>(GameObject gameObject) where T : Component
    {
        var found = gameObject.GetComponent<T>();
        if (found is null)
        {
            Debug.LogError($"{typeof(T).Name} not found in the GameObject {gameObject.name}");
        }
        return found;
    }
}