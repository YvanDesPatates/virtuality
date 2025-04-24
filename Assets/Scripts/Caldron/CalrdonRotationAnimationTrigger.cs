using UnityEngine;

public class CalrdonRotationAnimationTrigger : LeverAction
{
    private static readonly int CaldronRotationTrigger = Animator.StringToHash("rotate");
    
    [SerializeField] private CaldronMerger caldronMerger;
    [SerializeField] private Transform caldronTransform;
    [SerializeField] private Animator animator;

    private bool _animationIsRunning = false;
    private bool _animationHasStartAndIsNoMoreAtTheBeginning = false;

    public override void LeverWasPulled()
    {
        StartAnimation();
    }

    private void Update()
    {
        if (_animationIsRunning)
        {
            if (caldronTransform.localRotation.x > 0.3)
            {
                _animationHasStartAndIsNoMoreAtTheBeginning = true;
                caldronMerger.OnRotationMaxAngleReached();
            }

            // stop the animation, otherwise it's stuck in a loop
            if (_animationHasStartAndIsNoMoreAtTheBeginning && caldronTransform.localRotation.x == 0)
            {
                animator.SetBool(CaldronRotationTrigger, false);
                caldronMerger.OnRotationAnimationEnd(OnRotationFullyFinished);
            }
        }
    }

    private void StartAnimation()
    {
        if (!_animationIsRunning)
        {
            caldronMerger.OnRotationAnimationStart();
            animator.SetBool(CaldronRotationTrigger, true);
            _animationHasStartAndIsNoMoreAtTheBeginning = false;
            _animationIsRunning = true;
        }
    }

    private void OnRotationFullyFinished()
    {
        _animationIsRunning = false;
    }
}