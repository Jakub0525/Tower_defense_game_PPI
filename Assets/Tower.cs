using UnityEngine;

public class Tower : MonoBehaviour
{
    private Transform currentTarget;

    public float range = 5f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float fireTimer = 0f;
    public int damage = 25;

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

    void TargetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

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

    void Shoot()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.target = currentTarget;
            projectileScript.damage = damage; 
        }
    }

    public void UpgradeTower()
    {
        damage = Mathf.RoundToInt(damage * 1.25f);
        fireRate = fireRate / 1.25f;

        Debug.Log("Tower Upgraded! New Damage: " + damage + " | New Fire Rate: " + fireRate);
    }
}