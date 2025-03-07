using UnityEngine;

public class CalrdonRotationAnimationTrigger : MonoBehaviour
{
    private static readonly int CaldronRotationTrigger = Animator.StringToHash("rotate");

    [Tooltip("the rotation player has to give to trigger the rotation animation")]
    [SerializeField] private float rotationThreshold = 20f;
    [Space]
    [SerializeField] private CaldronMerger caldronMerger;
    [SerializeField] private Transform caldronTransform;
    [SerializeField] private DistanceReleaseGrabInteractable grabInteractable;
    [SerializeField] private Animator animator;

    private bool _animationIsRunning = false;
    private bool _animationHasStartAndIsNoMoreAtTheBeginning = false;

    private void Update()
    {
        if (_animationIsRunning)
        {
            grabInteractable.DetachInteractor();
        }
        
        if ( !_animationIsRunning && caldronTransform.localRotation.x > rotationThreshold/100)
        {
            caldronMerger.OnRotationAnimationStart();
            animator.SetBool(CaldronRotationTrigger, true);
            _animationHasStartAndIsNoMoreAtTheBeginning = false;
            _animationIsRunning = true;
        }

        if (_animationIsRunning && transform.localRotation.x > 0.3)
        {
            _animationHasStartAndIsNoMoreAtTheBeginning = true;
            caldronMerger.OnRotationMaxAngleReached();
        }
        
        // stop the animation, otherwise it's stuck in a loop
        if ( _animationIsRunning && _animationHasStartAndIsNoMoreAtTheBeginning && transform.rotation.x == 0)
        {
            animator.SetBool(CaldronRotationTrigger, false);
            caldronMerger.OnRotationAnimationEnd(OnRotationFullyFinished);
        }
    }

    private void OnRotationFullyFinished()
    {
        _animationIsRunning = false;
    }
}
