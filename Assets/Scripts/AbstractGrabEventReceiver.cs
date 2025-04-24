using UnityEngine;

public abstract class AbstractGrabEventReceiver : MonoBehaviour
{
    public abstract void OnGrabExit();
    
    public abstract void OnGrabEnter();
}
