using UnityEngine;

public class LeverController : AbstractGrabEventReceiver
{
    [SerializeField] private GameObject handleBar;
    [SerializeField] private LeverAction leverAction;
    [SerializeField] private PersonalizedGrabInteractable grabInteractable;

    private Transform _handleBarTransform;
    private Rigidbody _handleBarRb;
    private bool _animationIsRunning = false;
    private bool handleBarReachedMaxAngle = false;

    public void Start()
    {
        _handleBarTransform = handleBar.transform;
        _handleBarRb = Util.GetComponentOrLogError<Rigidbody>(handleBar);
    }

    public void Update()
    {
        if (! _animationIsRunning && _handleBarTransform.localEulerAngles.x > 3)
        {
            StartAnimation();
        }
        
        if (_animationIsRunning && ! handleBarReachedMaxAngle)
        {
            Quaternion targetRotation = Quaternion.Euler(45, 0, 0);
            _handleBarTransform.localRotation = Quaternion.Slerp(_handleBarTransform.localRotation, targetRotation, Time.deltaTime * 2);
            if (_handleBarTransform.localRotation == targetRotation)
            {
                handleBarReachedMaxAngle = true;
                if (leverAction is not null)
                {
                    leverAction.LeverWasPulled();
                }
            }
        }

        if (_animationIsRunning && handleBarReachedMaxAngle)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            _handleBarTransform.localRotation = Quaternion.Slerp(_handleBarTransform.localRotation, targetRotation, Time.deltaTime * 2);
            if (_handleBarTransform.localRotation == targetRotation)
            {
                StopAnimation();
            }
        }
    }

    public override void OnGrabExit()
    {
        if ( ! _animationIsRunning)
        {
            _handleBarRb.isKinematic = true;
        }
    }

    public override void OnGrabEnter()
    {
        if ( ! _animationIsRunning)
        {
            _handleBarRb.isKinematic = false;
        }
    }

    private void StartAnimation()
    {
        grabInteractable.DetachInteractor();
        grabInteractable.enabled = false;
        _handleBarRb.isKinematic = false;
        _animationIsRunning = true;
    }

    private void StopAnimation()
    {
        _animationIsRunning = false;
        handleBarReachedMaxAngle = false;
        grabInteractable.enabled = true;
        _handleBarRb.isKinematic = true;
    }
}
