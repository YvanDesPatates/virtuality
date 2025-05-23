using UnityEngine;

public class FrogJump : MonoBehaviour
{
    public float jumpForceMax = 3f;
    public float jumpForceMin = 1f;
    public float forwardForceMax = 3f;
    public float forwardForceMin = 1f;
    public float jumpCooldown = 5f;

    private bool isInFrogZone = true;
    private Rigidbody rb;
    private bool canJump = true;
    private Vector3 lastFrogZoneCenter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!canJump) return;

        if (isInFrogZone)
        {
            // Saut dans une direction aléatoire
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            Jump(randomDirection);
        }
        else
        {
            // Saut vers le centre de la zone quittée
            Vector3 toZoneCenter = (lastFrogZoneCenter - transform.position).normalized;
            Jump(toZoneCenter);
        }

        // Reviens dans la zone : reset le flag
        if (Vector3.Distance(transform.position, lastFrogZoneCenter) < 2f)
        {
            isInFrogZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Frog_Zone"))
        {
            isInFrogZone = false;
            lastFrogZoneCenter = other.bounds.center;
        }
    }

    void Jump(Vector3 direction)
    {
        // Ne saute que si la grenouille touche le sol
        if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            return;

        // Réinitialise la vitesse
        rb.linearVelocity = Vector3.zero;

        // Forces aléatoires
        float jumpForce = Random.Range(jumpForceMin, jumpForceMax);
        float forwardForce = Random.Range(forwardForceMin, forwardForceMax);

        // Appliquer les forces
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.AddForce(direction * forwardForce, ForceMode.Impulse);

        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    void ResetJump()
    {
        canJump = true;
    }
}
