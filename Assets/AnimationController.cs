using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem fireBreathParticle;

    [SerializeField] private AudioSource fireBreathSound;
    [SerializeField] private AudioSource flappingSound;
    [SerializeField] private AudioSource landingSound;
    [SerializeField] private AudioSource idleSound;
    [SerializeField] private AudioSource walkingSound;

    private List<Func<IEnumerator>> animationMethods;
    private Vector3 minimumPosition;

    void Start()
    {
        minimumPosition = transform.position;
        animationMethods = new List<Func<IEnumerator>> {
            PlayFlyingAnimation,
            PlaySittingAnimation,
            PlayRearAnimation,
            PlayScratchAnimation,
            PlayWhipTailLeftAnimation,
            PlayWhipTailRightAnimation,
            PlayBreatheFireAnimation
        };
        StartCoroutine(MainAnimationLoop());
    }

    // make sure the dragon does not go under its origin position after an animation
    void Update()
    {
        Vector3 pos = transform.position;

        pos.y = Mathf.Max(pos.y, minimumPosition.y);
        pos.x = Mathf.Max(pos.x, minimumPosition.x);

        transform.position = pos;
    }

    // All animation methods must come back to idle before playing another one
    private IEnumerator MainAnimationLoop()
    {
        while (true)
        {
            int index = UnityEngine.Random.Range(0, animationMethods.Count);
            yield return StartCoroutine(animationMethods[index]());

            animator.CrossFade("Idle", 0.5f);
            idleSound.Play();
            yield return new WaitForSeconds(3f);
            idleSound.Stop();
        }
    }

    private IEnumerator PlayBreatheFireAnimation()
    {
        animator.CrossFade("Breathe Fire", 0.5f);
        fireBreathParticle.Play();
        fireBreathSound.Play();
        yield return new WaitForSeconds(2f);
        fireBreathParticle.Stop();
        fireBreathSound.Stop();
    }

    private IEnumerator PlaySittingAnimation()
    {
        animator.CrossFade("Sit", 0.5f);
        yield return new WaitForSeconds(1.2f);
        landingSound.Play();

        animator.CrossFade("Sitting", 0.5f);
        yield return new WaitForSeconds(4f);

        animator.CrossFade("Rise", 0.5f);
        yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator PlayFlyingAnimation()
    {
        animator.CrossFade("Idle Takeoff", 0.5f);
        flappingSound.Play();
        yield return new WaitForSeconds(2.2f);

        animator.CrossFade("Fly Idle", 0.5f);
        yield return new WaitForSeconds(2f);

        animator.CrossFade("Idle Landing", 0.5f);
        flappingSound.Stop();
        yield return new WaitForSeconds(2.15f);
        landingSound.Play();
    }

    private IEnumerator PlayRearAnimation()
    {
        walkingSound.Play();
        animator.CrossFade("Attack 2", 0.5f);
        yield return new WaitForSeconds(2.15f);
        walkingSound.Stop();
    }

    private IEnumerator PlayWhipTailLeftAnimation()
    {
        idleSound.Play();
        animator.CrossFade("Tail Whip L", 0.5f);
        yield return new WaitForSeconds(2.15f);
        idleSound.Stop();
    }

    private IEnumerator PlayWhipTailRightAnimation()
    {
        idleSound.Play();
        animator.CrossFade("Tail Whip R", 0.5f);
        yield return new WaitForSeconds(2.15f);
        idleSound.Stop();
    }

    private IEnumerator PlayScratchAnimation()
    {
        idleSound.Play();
        animator.CrossFade("Idle Break", 0.5f);
        yield return new WaitForSeconds(5.20f);
        idleSound.Stop();
    }
}
