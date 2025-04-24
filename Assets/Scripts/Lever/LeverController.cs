using System;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] private Transform handleBar;
    [SerializeField] private LeverAction leverAction;
    
    private bool _animationIsRunning = false;
    private bool handleBarReachedMaxAngle = false;

    public void Start()
    {
        StartAnimation();
    }
    
    public void StartAnimation()
    {
        _animationIsRunning = true;
        if (leverAction is not null)
        {
            leverAction.LeverWasPulled();
        }
    }

    public void Update()
    {
        if (_animationIsRunning && ! handleBarReachedMaxAngle)
        {
            Quaternion targetRotation = Quaternion.Euler(45, 0, 0);
            handleBar.localRotation = Quaternion.Slerp(handleBar.localRotation, targetRotation, Time.deltaTime * 2);
            if (handleBar.localRotation == targetRotation)
            {
                handleBarReachedMaxAngle = true;
            }
        }

        if (_animationIsRunning && handleBarReachedMaxAngle)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            handleBar.localRotation = Quaternion.Slerp(handleBar.localRotation, targetRotation, Time.deltaTime * 2);
            if (handleBar.localRotation == targetRotation)
            {
                _animationIsRunning = false;
                handleBarReachedMaxAngle = false;
            }
        }
    }
}
