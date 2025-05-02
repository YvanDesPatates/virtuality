using UnityEngine;

public class Frog : MonoBehaviour
{
    public float crouchTime = 0.3f;
    public float jumpForce = 10f;
    public float forwardForce = 5f; // Force horizontale vers l’avant
    public float cooldownBetweenJumps = 1f; // Temps avant de pouvoir resauter
    public Vector3 crouchScale = new Vector3(1, 0.5f, 1);

    private Vector3 originalScale;
    private Rigidbody rb;
    private bool isCrouching = false;
    private bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f && canJump && !isCrouching)
        {
            StartCoroutine(FrogJumpRoutine());
        }
    }

    System.Collections.IEnumerator FrogJumpRoutine()
    {
        isCrouching = true;
        canJump = false;

        // Reset la vélocité Y pour éviter les rebonds
        rb.linearVelocity = new Vector3(0f, 0f, 0f);

        // Appliquer le saut vertical + une poussée vers l’avant
        Vector3 jumpDirection = (transform.forward + Vector3.up).normalized;
        rb.AddForce(jumpDirection * jumpForce + transform.forward * forwardForce, ForceMode.Impulse);

        isCrouching = false;

        // Cooldown avant de pouvoir resauter
        yield return new WaitForSeconds(cooldownBetweenJumps);
        canJump = true;
    }
}
