using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The Enemy AI controller class. Contains logic to control the enemy movement and interactions.
/// </summary>
public class AIController : MonoBehaviour
{
    //How far can the enemy see the player.
    [SerializeField] private float triggerDistance;

    //An array of waypoints describing the path the enemy patrols.
    [SerializeField] private GameObject[] waypoints;

    //Describes the current state of the enemy.
    private bool walking;
    private bool chasing;

    private GameObject player;
    private NavMeshAgent agent;

    //To monitor the time passed during certain interactions.
    private float time;

    private int currentWaypoint;

    //To ensure synchronization of coroutines.
    private bool coroutines;

    /// <summary>
    /// Initialize private attributes of the AI.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        walking = true;
        chasing = false;
        time = 0.0f;
        currentWaypoint = 0;
        coroutines = false;
    }

    /// <summary>
    /// FixedUpdate - Called every fixed rate frame.
    /// Contains logic to walk between waypoints and chase the player if the player is seen.
    /// </summary>
    void FixedUpdate()
    {
        //Walk between the waypoints defined above when in the walking / idle state.
        if (walking && !coroutines)
        {
            Walk();    
        }

        //Check if the enemy encounters the player and change its state to chasing.
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);

        //Raycast to check if it hits the player within the triggerDistance.
        if (Physics.Raycast(ray, out RaycastHit hit, triggerDistance))
        {
            if (hit.collider.gameObject == player && !chasing)
            {
                Debug.Log("Be Seeing You");
                Debug.DrawRay(ray.origin, ray.direction * triggerDistance, Color.red);
                ChangeState();
                time = 0.0f;
            }
        }

        //If the enemy sees the player, then chase him.
        if (chasing && !coroutines)
        {
            time += Time.deltaTime;
            transform.LookAt(player.transform);
            Move(player.transform.position);
        }

        //Stop chasing and return to idle path if the enemy can't see the player for at least 3 seconds.
        if (chasing)
        {
            if (time >= 3.0f)
            {
                Debug.Log("I lost him...");
                agent.isStopped = true;
                ChangeState();
            }
        }
    }

    /// <summary>
    /// Contains logic to move enemy between waypoints as defined in the waypoints array.
    /// </summary>
    void Walk()
    {
        Vector3 position = waypoints[currentWaypoint].transform.position;

        if (Vector3.Distance(transform.position, position) < 1f)
            StartCoroutine(ChangeWaypoint());

        //Ensure realism!
        if (Vector3.Distance(transform.position, position) > 2f)
            transform.LookAt(position);

        Move(position);
    }

    /// <summary>
    /// Coroutine to change the current waypoint for the enemy to go to. Waits for 3 seconds first.
    /// </summary>
    IEnumerator ChangeWaypoint()
    {
        //Ensure synchronization - if a couroutine is executing then wait for it to finish first.
        if (coroutines)
            yield break;

        coroutines = true;

        yield return new WaitForSeconds(5.0f);

        currentWaypoint++;

        if (currentWaypoint >= waypoints.Length)
            currentWaypoint = 0;

        coroutines = false;
    }

    /// <summary>
    /// Stun the enemy, i.e. make it immovable for 5 seconds.
    /// This is a container method for the coroutine pauseAndResume(), which contains the main logic.
    /// This can be called by other objects when they collide with the enemy, i.e. when an item hits an enemy.
    /// </summary>
    public void Stun()
    {
        StartCoroutine(PauseAndResume());
    }


    /// <summary>
    /// Coroutine which pauses enemy movement for 5 seconds and then resumes.
    /// </summary>
    IEnumerator PauseAndResume()
    {
        //Ensure synchronization.
        if (coroutines)
            yield break;

        coroutines = true;

        Debug.Log("Stunned!");
        agent.isStopped = true;

        yield return new WaitForSeconds(5.0f);

        Debug.Log("Free");
        agent.isStopped = false;

        coroutines = false;
    }

    /// <summary>
    /// Move the enemy NavMeshAgent to the specified position
    /// </summary>
    /// <param name="position">The point to move the enemy to.</param>
    void Move(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
    }

    /// <summary>
    /// Change the enemy state from walking to chasing or vice-versa.
    /// </summary>
    void ChangeState()
    {
        walking = !walking;
        chasing = !chasing;
    }
}
