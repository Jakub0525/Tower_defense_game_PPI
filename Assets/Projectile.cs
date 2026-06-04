using UnityEngine;

/// <summary>
/// Controls individual projectile physics, homing tracking vectors towards an enemy target,
/// raycast obstacle obstruction logic, and collision impact damage application.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>The target Transform entity (Enemy) that the projectile is actively tracking.</summary>
    public Transform target;

    /// <summary>The structural velocity travel speed of the projectile.</summary>
    public float speed = 10f;

    /// <summary>The numerical damage payload transferred to the target upon a successful hit sequence.</summary>
    public int damage = 25;

    /// <summary>
    /// Standard Unity callback. Validates target lifetime, performs trajectory occlusion raycasts 
    /// against defensive structures, updates position vectors, and applies damage upon hitting proximity thresholds.
    /// </summary>
    void Update()
    {
        // Self-destruct if the target enemy has already been eliminated by another tower
        if (target == null) { Destroy(gameObject); return; }

        Vector3 direction = (target.position - transform.position).normalized;
        float moveDistance = speed * Time.deltaTime;

        // Predictive raycast loop to prevent projectiles from tracing through unintended structural obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, moveDistance))
        {
            if (hit.collider.CompareTag("Building") || hit.collider.CompareTag("Wall"))
            {
                if (hit.transform != target)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }

        transform.position += direction * moveDistance;

        // Proximity impact threshold detection loop
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            target.GetComponent<Enemy>()?.ReceiveDamage(damage);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Unity physics trigger callback. Destroys the projectile instantly if it physically 
    /// intersects with a collider tagged as a blocking obstacle.
    /// </summary>
    /// <param name="hitCollider">The Collider object that intersected with this projectile's trigger zone.</param>
    void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}