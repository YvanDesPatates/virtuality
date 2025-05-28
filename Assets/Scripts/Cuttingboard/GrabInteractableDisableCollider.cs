using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabInteractableDisableCollider : XRGrabInteractable
{
    [SerializeField] private List<Collider> collidersToDisable = new();
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        foreach (var interactableCollider in collidersToDisable)
        {
            interactableCollider.enabled = false;
        }
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        foreach (var interactableCollider in collidersToDisable)
        {
            interactableCollider.enabled = true;
        }
    }
}
