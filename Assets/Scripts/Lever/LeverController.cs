using UnityEngine;
using UnityEngine.Serialization;

public class LeverController : MonoBehaviour
{
    [SerializeField] private LeverAction leverAction;
    [Space]
    [SerializeField] private LeverGrabInteractable grabInteractable;
    [SerializeField] private float maxGlobalZPostion;
    [SerializeField] private AudioSource lever_pulled_sound;
    [SerializeField] private AudioSource lever_pushed_sound;

    private bool handleBarReachedMaxAngle = false;
    private float _minGlobalZPostion;

    public void Start()
    {
        grabInteractable.SetMaxGlobalZPostion(maxGlobalZPostion);
        _minGlobalZPostion = transform.position.z;
    }

    public void Update()
    {
        if (!handleBarReachedMaxAngle && transform.position.z >= maxGlobalZPostion)
        {
            handleBarReachedMaxAngle = true;
            lever_pulled_sound.Play();
        }

        if (handleBarReachedMaxAngle && transform.position.z <= _minGlobalZPostion)
        {
            handleBarReachedMaxAngle = false;
            leverAction.LeverWasPulled();
            StepTracker.Instance.StepCompleted(StepType.PullLever);
            lever_pushed_sound.Play();
        }
    }
}
