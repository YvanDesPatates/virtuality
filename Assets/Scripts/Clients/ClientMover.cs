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

        // Position cible sur le plan XZ (même hauteur que l'objet)
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        // Déplacement fluide sur le plan horizontal
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Direction sans tenir compte de la hauteur
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Ignore la différence de hauteur
        direction = direction.normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Vérifie si le client est arrivé (en 3D, mais la distance reste précise)
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            currentTargetIndex++;
            if (currentTargetIndex >= targets.Length)
            {
                if (animator != null)
                    animator.SetTrigger("IdleBreak");

                enabled = false; // Arrêter les updates
            }
        }
    }
}
