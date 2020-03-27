using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The Enemy AI controller class. Contains logic to control the enemy movement and interactions.
/// </summary>
public class AIController : MonoBehaviour
{
    //How far can the AI see the player.
    [SerializeField] private float triggerDistance;

    //AI will patrol between these waypoints.
    [SerializeField] private GameObject[] waypoints;

    private enum States { patrolling = 0, chasing = 1, slowed = 2, stunned = 3 };
    private States state;

    private int currentWaypoint;

    //To ensure synchronization of coroutines.
    private bool patrolCoroutines;
    private bool actionCoroutines;

    private Animator animator;
    private NavMeshAgent agent;
    private GameObject player;

    private int health;

    /// <summary>
    /// Initialize private attributes of the AI.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        state = States.patrolling;
        currentWaypoint = 0;
        agent.updateRotation = false;
        health = 100;
        patrolCoroutines = false;
        actionCoroutines = false;
    }

    /// <summary>
    /// FixedUpdate - Called every fixed rate frame.
    /// Contains logic to walk between waypoints and chase the player if the player is seen.
    /// </summary>
    void FixedUpdate()
    {
        if (!animator.GetBool("dead"))
        {
            agent.speed *= (agent.isStopped ? 0f : 1f);
            animator.SetFloat("speed", agent.speed);

            //Patrolling
            if (state == States.patrolling && !patrolCoroutines)
            {
                Vector3 position = waypoints[currentWaypoint].transform.position;

                if (Vector3.Distance(transform.position, position) < 1.0f)
                {
                    agent.speed = 0;
                    StartCoroutine(ChangeWaypoint());
                }
                else
                {
                    if (Vector3.Distance(transform.position, position) > 2.0f)
                        transform.LookAt(position);
                    agent.speed = 1;
                    agent.SetDestination(position);
                }
            }

            Vector3 direction = transform.forward;
            Ray ray = new Ray(transform.position, direction);

            //Raycast to check if it hits the player within the triggerDistance.
            if (Physics.SphereCast(ray, 3.0f, out RaycastHit hit, triggerDistance))
            {
                if (hit.collider.gameObject == player)
                {
                    state = States.chasing;
                    StartCoroutine(Chase());
                }
            }

            //Chasing
            if (state == States.chasing)
            {
                bool playerIsNear = Vector3.Distance(transform.position, player.transform.position) < 2.0f;
                transform.LookAt(player.transform);
                agent.SetDestination(player.transform.position);
                agent.isStopped = playerIsNear;
                if (playerIsNear)
                    animator.SetTrigger("attack");
            }
        }
    }

    /// <summary>
    /// Coroutine to chase the player and stop chasing after 4 seconds.
    /// </summary>
    IEnumerator Chase()
    {
        //Ensure synchronization.
        if (actionCoroutines)
            yield break;

        actionCoroutines = true;

        //If AI is attacking then set speed to 0, otherwise 2.
        agent.speed = animator.GetCurrentAnimatorStateInfo(0).IsName("attack") ? 0f : 2.0f;

        yield return new WaitForSeconds(4.0f);
        
        actionCoroutines = false;
        agent.speed = 1.0f;
        state = States.patrolling;
    }

    /// <summary>
    /// Coroutine which changes the current waypoint to patrol to after 
    /// 5 seconds. The enemy stays idle for those 5 seconds.
    /// </summary>
    IEnumerator ChangeWaypoint()
    {
        //Ensure synchronization.
        if (patrolCoroutines)
            yield break;

        patrolCoroutines = true;

        yield return new WaitForSeconds(5.0f);

        currentWaypoint += 1;
        if (currentWaypoint >= waypoints.Length)
            currentWaypoint = 0;

        patrolCoroutines = false;
    }

    /// <summary>
    /// Public method called by ThrowableManager which stuns the AI.
    /// </summary>
    public void Stun()
    {
        StartCoroutine(PauseAndResume());
    }

    /// <summary>
    /// Referenced by Stun() which stops movement for 5 seconds and
    /// then resumes. 
    /// </summary>
    IEnumerator PauseAndResume()
    {
        Debug.Log("Stunned!");
        animator.SetBool("stun", true);
        agent.isStopped = true;
        States originalState = state;
        state = States.stunned;

        yield return new WaitForSeconds(5.0f);

        Debug.Log("Free");
        animator.SetBool("stun", false);
        agent.isStopped = false;
        state = originalState;
    }

    /// <summary>
    /// Public method called by ThrowableManager to slow the AI movement.
    /// </summary>
    public void Slow()
    {
        StartCoroutine(SlowAndResume());
    }

    /// <summary>
    /// Referenced by Slow() to slow and resume movement after 5 seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator SlowAndResume()
    {
        Debug.Log("Slowed!");
        float originalSpeed = agent.speed;
        //animator.SetBool("slow", true);
        agent.speed /= 2f;
        States originalState = state;
        state = States.slowed;

        yield return new WaitForSeconds(5.0f);

        Debug.Log("Normal");
        //animator.SetBool("slow", false);
        agent.speed = originalSpeed;
        state = originalState;
    }

    /// <summary>
    /// Damage the AI and trigger a hit animation.
    /// </summary>
    /// <param name="hitPosition"> Position at which a throwable hit the AI. </param>
    /// <param name="damageAmount"> The amount of damage to apply. </param>
    public void Hit(Vector3 hitPosition, int damageAmount)
    {
        health -= damageAmount;

        TriggerAnimationAt(hitPosition);

        if (health <= 30)
            animator.SetTrigger("injured");

        //He dead...
        if (health <= 0)
        {
            animator.SetBool("dead", true);
            agent.isStopped = true;
            agent.speed = 0f;
            Destroy(gameObject, 10f);
        }

    }

    /// <summary>
    /// Triggers an appropriate hit animation relative to where the AI was hit with an object.
    /// </summary>
    /// <param name="hitPosition"> Position at which the AI was hit. </param>
    private void TriggerAnimationAt(Vector3 hitPosition)
    {
        Vector3 hitDirection = player.transform.position - hitPosition;
        float angle = Vector3.SignedAngle(transform.forward, hitDirection, Vector3.up);

        if (angle < -45 && angle <= 45)
            animator.SetTrigger("hit front");
        else if (angle > 45 && angle <= 135)
            animator.SetTrigger("hit right");
        else if (angle > 135 && angle >= -135)
            animator.SetTrigger("hit back");
        else
            animator.SetTrigger("hit left");
    }
}
