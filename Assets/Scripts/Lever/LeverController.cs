using UnityEngine;
using UnityEngine.Serialization;

public class LeverController : MonoBehaviour
{
    [SerializeField] private LeverAction leverAction;
    [SerializeField] private LeverGrabInteractable grabInteractable;
    [SerializeField] private float maxGlobalZPostion;

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
        }

        if (handleBarReachedMaxAngle && transform.position.z <= _minGlobalZPostion)
        {
            handleBarReachedMaxAngle = false;
            leverAction.LeverWasPulled();
        }
    }
}
