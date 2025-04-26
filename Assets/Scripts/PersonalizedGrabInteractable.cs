using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PersonalizedGrabInteractable: XRGrabInteractable
{
    [SerializeField] private List<AbstractGrabEventReceiver> grabEventReceivers = new();
        
    private IXRSelectInteractor _cachedInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _cachedInteractor = args.interactorObject;
        
        foreach (var receiver in grabEventReceivers)
        {
            receiver.OnGrabEnter();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _cachedInteractor = null;
        
        foreach (var receiver in grabEventReceivers)
        {
            receiver.OnGrabExit();
        }
    }

    public IXRSelectInteractor DetachInteractor()
    {
        if (_cachedInteractor is not null)
        {
            var interactor = _cachedInteractor;
            interactionManager.SelectExit(_cachedInteractor, this);
            return interactor;
        }
        return null;
    }
    
    public void AttachInteractor(IXRSelectInteractor interactor)
    {
        if (_cachedInteractor is null && interactor is not null)
        {
            interactionManager.SelectEnter(interactor, this);
        }
    }
    
    public bool IsGrabbed()
    {
        return _cachedInteractor != null;
    }
}
