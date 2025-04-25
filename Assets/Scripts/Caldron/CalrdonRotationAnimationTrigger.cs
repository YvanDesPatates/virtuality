using UnityEngine;

public class CalrdonRotationAnimationTrigger : LeverAction
{
    [SerializeField] private CaldronMerger caldronMerger;
    [SerializeField] private float maxLocalXRotation;
    [SerializeField] private float rotationSpeed = 2f;

    private bool _animationIsRunning = false;
    private bool _maxXRotationWasReached = false;
    private float _minXLocalRotation;

    public override void LeverWasPulled()
    {
        _minXLocalRotation = caldronMerger.transform.localRotation.eulerAngles.x;
        StartAnimation();
    }

    private void Update()
    {
        if (_animationIsRunning)
        {
            float currentXRotation = caldronMerger.transform.localRotation.eulerAngles.x;

            if (!_maxXRotationWasReached)
            {
                // Aller vers maxLocalXRotation
                Quaternion targetRotation = Quaternion.Euler(maxLocalXRotation, 0, 0);
                caldronMerger.transform.localRotation = Quaternion.RotateTowards(
                    caldronMerger.transform.localRotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

                if (Mathf.Abs(currentXRotation - maxLocalXRotation) < 0.1f)
                {
                    caldronMerger.OnRotationMaxAngleReached();
                    _maxXRotationWasReached = true;
                }
            }
            else
            {
                // Retourner à minLocalXRotation
                Quaternion targetRotation = Quaternion.Euler(_minXLocalRotation, 0, 0);
                caldronMerger.transform.localRotation = Quaternion.RotateTowards(
                    caldronMerger.transform.localRotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

                if (Mathf.Abs(currentXRotation - _minXLocalRotation) < 0.1f)
                {
                    OnRotationFullyFinished();
                }
            }
        }
    }

    private void StartAnimation()
    {
        if (!_animationIsRunning)
        {
            _animationIsRunning = true;
        }
    }

    private void OnRotationFullyFinished()
    {
        caldronMerger.transform.localRotation = Quaternion.Euler(_minXLocalRotation, 0, 0);
        _maxXRotationWasReached = false;
        _animationIsRunning = false;
    }
}