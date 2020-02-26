using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assigned to a throwable item which stuns the enemy on collision with it.
/// </summary>
public class HitEnemy : MonoBehaviour
{
    /// <summary>
    /// Check for collision with an object
    /// </summary>
    /// <param name="collision">The collision object that this body collided with.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AI")
        {
            AIController controller = collision.gameObject.GetComponent<AIController>();
            controller.Stun();
        }
    }
}
