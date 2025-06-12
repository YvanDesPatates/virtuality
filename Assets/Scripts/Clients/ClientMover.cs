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
        if (targets.Length == 0 || currentTargetIndex >= targets.Length)
            return;

        Transform target = targets[currentTargetIndex];

        // Déplacement fluide et sûr
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Rotation vers la cible
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Vérifie si le client est arrivé
        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentTargetIndex++;
            if (currentTargetIndex >= targets.Length)
            {
                animator.SetTrigger("IdleBreak");
                enabled = false; // Arrêter les updates
            }
        }
    }
}
