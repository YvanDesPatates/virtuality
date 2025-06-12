using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private Animator animator;

    void Start()
    {
        StartCoroutine(MainAnimationLoop());
    }

    // All animation methods must come back to idle before playing another one
    private IEnumerator MainAnimationLoop()
    {
        while (true)
        {
            yield return StartCoroutine(PlayFlyingAnimation());

            animator.Play("Idle");
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator PlaySittingAnimation()
    {
        animator.Play("Sit");
        yield return new WaitForSeconds(1.2f);

        animator.Play("Sitting");
        yield return new WaitForSeconds(3f);

        animator.Play("Rise");
        yield return new WaitForSeconds(1.5f);
    }


    private IEnumerator PlayFlyingAnimation()
    {
        animator.Play("Idle Takeoff");
        yield return new WaitForSeconds(2.2f);

        animator.CrossFade("Fly Idle", 0.05f);
        yield return new WaitForSeconds(4f);

        animator.CrossFade("Idle Landing", 0.05f);
        yield return new WaitForSeconds(2.15f);
    }

}
