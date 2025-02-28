using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DistanceReleaseGrabInteractable: XRGrabInteractable
{
    [SerializeField] private float maxGrabDistance = 0.25f;
    private IXRSelectInteractor _cachedInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _cachedInteractor = args.interactorObject;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _cachedInteractor = null;
    }

    private void Update()
    {
        if (_cachedInteractor != null)
        {
            if (Vector3.Distance(_cachedInteractor.transform.position, transform.position) <= maxGrabDistance)
            {
                interactionManager.SelectExit(_cachedInteractor, this);
            }
        }
    }

    public void DetachInteractor()
    {
        if (_cachedInteractor != null)
        {
            interactionManager.SelectExit(_cachedInteractor, this);
        }
    }
}
