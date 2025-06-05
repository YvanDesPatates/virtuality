using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float jumpForceMax = 3f;
    public float jumpForceMin = 1f;
    public float forwardForceMax = 3f;
    public float forwardForceMin = 1f;
    public float jumpCooldown = 5f;

    public GameObject dropPrefab;
    public float dropCooldown = 2f;
    private bool canDrop = true;

    private bool isInFrogZone = true;
    private Rigidbody rb;
    private bool canJump = true;
    private Vector3 lastFrogZoneCenter;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (canJump)
        {
            if (isInFrogZone)
            {
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                //Jump(randomDirection);
            }
            else
            {
                Vector3 toZoneCenter = (lastFrogZoneCenter - transform.position).normalized;
                //Jump(toZoneCenter);
            }

            if (Vector3.Distance(transform.position, lastFrogZoneCenter) < 2f)
            {
                isInFrogZone = true;
            }
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.up * 1.5f, ForceMode.Acceleration);
        }

        if (canDrop && transform.localRotation.x < -0.65 && transform.localRotation.x > -0.9 || canDrop && transform.localRotation.x > 0.65 && transform.localRotation.x < 0.9)
        {
            Debug.Log("Dropping cube");
            DropCube();
            canDrop = false;
            Invoke(nameof(ResetDrop), dropCooldown);
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

        if (animator != null)
        {
            animator.SetTrigger("goAir");
        }

        // Forces aléatoires
        float jumpForce = Random.Range(jumpForceMin, jumpForceMax);
        float forwardForce = Random.Range(forwardForceMin, forwardForceMax);

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        // Appliquer les forces
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.AddForce(direction * forwardForce, ForceMode.Impulse);

        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    void ResetJump()
    {
        animator.SetTrigger("goGround");
        canJump = true;
    }

    void ResetDrop()
    {
        canDrop = true;
    }

    void DropCube()
    {
        GameObject drop = Instantiate(dropPrefab, transform.position + Vector3.down * 0.05f, Quaternion.identity);
        Rigidbody dropRb = drop.GetComponent<Rigidbody>();
    }

}
