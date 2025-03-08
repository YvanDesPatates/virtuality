using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PersonalizedGrabInteractable: XRGrabInteractable
{
    private IXRSelectInteractor _cachedInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        GetComponent<Rigidbody>().isKinematic = false;
        _cachedInteractor = args.interactorObject;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        GetComponent<Rigidbody>().isKinematic = true;
        _cachedInteractor = null;
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
}
