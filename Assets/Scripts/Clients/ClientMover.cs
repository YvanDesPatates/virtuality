using UnityEngine;

public class ClientMover : MonoBehaviour
{
    public Transform[] targets;
    public float speed = 1.5f;

    private int currentTargetIndex = 0;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("Walk");
        }
    }

    void Update()
    {

        if (targets.Length == 0)
            return;

        Transform target = targets[currentTargetIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if the client has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentTargetIndex++;
            if (currentTargetIndex >= targets.Length)
            {
                currentTargetIndex = targets.Length; // Go to the final target
                animator.SetTrigger("IdleBreak");
            }
        }
    }
}