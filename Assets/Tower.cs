using UnityEngine;

/// <summary>
/// Controls offensive tower mechanics including automated nearby target acquisition, 
/// tactical weapon cooldown loops, projectile instantiation, and upgrade scaling.
/// </summary>
public class Tower : MonoBehaviour
{
    /// <summary>The transform position reference tracking the actively targeted enemy unit.</summary>
    private Transform currentTarget;

    /// <summary>The maximum radial range threshold inside which the tower can acquire and lock onto targets.</summary>
    public float range = 11f;

    /// <summary>The prefabricated GameObject archetype for projectile munitions generated upon attacking.</summary>
    public GameObject projectilePrefab;

    /// <summary>The designated node point in 3D space from which projectiles are launched.</summary>
    public Transform firePoint;

    /// <summary>The weapon fire interval delay expressed in seconds between consecutive shots.</summary>
    public float fireRate = 0.8f;

    /// <summary>Internal timer tracking engine delta pacing to evaluate when weapon systems are ready to cycle.</summary>
    private float fireTimer = 0f;

    /// <summary>The base numerical damage payload assigned to generated projectiles.</summary>
    public int damage = 25;

    /// <summary>The localized AudioSource component executed to play custom muzzle attack acoustic sound effects.</summary>
    public AudioSource shootSound;

    /// <summary>
    /// Standard Unity callback. Periodically executes scene structural scans to lock targets, 
    /// processes localized weapon reload trackers, and fires projectiles when timers clear.
    /// </summary>
    void Update()
    {
        TargetNearestEnemy();

        if (currentTarget == null) return;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    /// <summary>
    /// Performs a linear distance inspection across all objects tagged as "Enemy" within the active scene hierarchy, 
    /// localizes the closest target vector, and verifies it against the weapon range limits.
    /// </summary>
    void TargetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // Iterate through all spawned enemies to resolve the closest spatial entity
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            currentTarget = nearestEnemy.transform;
        }
        else
        {
            currentTarget = null;
        }
    }

    /// <summary>
    /// Instantiates a projectile at the fire point location, extracts its structural Projectile configuration component, 
    /// maps target tracking variables, injects scaling damage factors, and triggers shoot sounds.
    /// </summary>
    void Shoot()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.target = currentTarget;
            projectileScript.damage = damage;
        }
        if (shootSound != null) shootSound.Play();
    }

    /// <summary>
    /// Advances offensive parameters. Scales projectile impact payloads by an exponential multiplier (1.25x) 
    /// and scales attack frequency by cutting the reload interval time down proportionally.
    /// </summary>
    public void UpgradeTower()
    {
        damage = Mathf.RoundToInt(damage * 1.25f);
        fireRate = fireRate / 1.25f;

        Debug.Log("Tower Upgraded! New Damage: " + damage + " | New Fire Rate: " + fireRate);
    }
}