using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabInteractableReturnToPoint : XRGrabInteractable
{
    [SerializeField] private Transform returnPoint;
    [SerializeField] private float attractionSpeed = 1f;
    [Tooltip("higher the value is, less the direction of the ingredient will be straight to the point")]
    [SerializeField] private float fluctuationCoefficient = 0.2f;
    
    private bool _isReturning = false;

    [SerializeField] private ToDoListController toDoListController;
    

    // Update is called once per frame
    void Update()
    {
        if ( ! _isReturning) return;
        
        
        float fluctuationX = Mathf.Sin(Time.time * 2f) * fluctuationCoefficient;
        float fluctuationZ = Mathf.Cos(Time.time * 2f) * fluctuationCoefficient;
        Vector3 fluctuatedPosition = returnPoint.position + new Vector3(fluctuationX, 0, fluctuationZ);

        if (Vector3.Distance(transform.position, returnPoint.position) < fluctuationCoefficient)
        {
            fluctuatedPosition = returnPoint.position;
        }

        transform.position = Vector3.Lerp(transform.position, fluctuatedPosition,
            Time.deltaTime * attractionSpeed);
        
        // Rotation douce vers l'orientation neutre
        if (transform is not null)
        {
            Quaternion targetRotation = Quaternion.identity;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * attractionSpeed);
        }
    }
    
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        _isReturning = true;
    }
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        StepTracker.Instance.StepCompleted(StepType.FindRecipe);
        toDoListController.HasFoundRecipe();
        
        _isReturning = true;
    }
}
