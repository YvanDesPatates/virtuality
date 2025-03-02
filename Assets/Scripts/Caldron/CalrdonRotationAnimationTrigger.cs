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

    private void Update()
    {
        // stop the animation, otherwise it's stuck in a loop
        if ( _animationIsRunning && transform.rotation.x == 0)
        {
            animator.SetBool(CaldronRotationTrigger, false);
            _animationIsRunning = false;
            caldronMerger.OnRotationAnimationEnd();
        }
        if ( !_animationIsRunning && caldronTransform.localRotation.x > rotationThreshold/100)
        {
            grabInteractable.DetachInteractor();
            caldronMerger.OnRotationAnimationStart();
            animator.SetBool(CaldronRotationTrigger, true);
            _animationIsRunning = true;
        }
    }
}
