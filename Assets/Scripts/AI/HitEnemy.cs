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
        if (collision.gameObject.CompareTag("AI"))
        {
            AIController controller = collision.gameObject.GetComponent<AIController>();
            ThrowableManager throwable = GetComponent<ThrowableManager>();

            if (throwable.HitEffect == Throwable.HitEffects.stun)
                controller.Stun();
            else if (throwable.HitEffect == Throwable.HitEffects.slow)
                controller.Slow();
            else if (throwable.HitEffect == Throwable.HitEffects.damage)
                controller.Hit(throwable.Damage);
            else if (throwable.HitEffect == Throwable.HitEffects.attract)
                controller.Attract(gameObject);
        }
    }
}
