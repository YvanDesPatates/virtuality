using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public abstract class AbstractGrabEventReceiver : MonoBehaviour
{
    public abstract void OnGrabExit(PersonalizedGrabInteractable interactable);
    
    public abstract void OnGrabEnter(PersonalizedGrabInteractable interactable);
}
