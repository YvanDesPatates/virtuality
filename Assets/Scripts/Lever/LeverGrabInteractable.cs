using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LeverGrabInteractable : XRBaseInteractable
{
    private float _maxGlobalZPostion = 45f;
    private float _minGlobalZPostion;
    private bool _isGrabbed = false;
    private IXRSelectInteractor _grabbingInteractor;
    private float _zOffset;

    private void Start()
    {
        _minGlobalZPostion = transform.position.z;
    }
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _isGrabbed = true;
        _grabbingInteractor = args.interactorObject;
        _zOffset = _grabbingInteractor.transform.position.z - transform.position.z;
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _isGrabbed = false;
        _grabbingInteractor = null;
    }

    private void LateUpdate()
    {
        if (_isGrabbed)
        {
            Vector3 interactorPosition = _grabbingInteractor.transform.position;
            float zPositionTarget = Mathf.Clamp(interactorPosition.z - _zOffset, _minGlobalZPostion, _maxGlobalZPostion);
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, zPositionTarget);
            transform.position = targetPosition;
        }
    }

    public void DetachInteractor()
    {
        if (_grabbingInteractor is not null)
        {
            interactionManager.SelectExit(_grabbingInteractor, this);
        }
    }
    
    public void SetMaxGlobalZPostion(float maxZPosition)
    {
        _maxGlobalZPostion = maxZPosition;
    }
}