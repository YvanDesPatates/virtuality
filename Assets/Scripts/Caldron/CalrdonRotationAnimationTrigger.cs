using UnityEngine;

public class CalrdonRotationAnimationTrigger : MonoBehaviour
{
    private static readonly int CaldronRotationTrigger = Animator.StringToHash("caldron_rotation_trigger");

    [Tooltip("the rotation player has to give to trigger the rotation animation")]
    [SerializeField] private float rotationThreshold = 20f;
    [Space]
    [SerializeField] private CaldronMerger caldronMerger;
    [SerializeField] private Transform caldronTransform;
    [SerializeField] private DistanceReleaseGrabInteractable grabInteractable;
    [SerializeField] private Animator animator;

    private void Update()
    {
        if (caldronTransform.localRotation.x > rotationThreshold/100)
        {
            grabInteractable.DetachInteractor();
            caldronMerger.RotationAnimationStart();
            animator.SetTrigger(CaldronRotationTrigger);
        }
    }
}
