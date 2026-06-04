using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the enemy AI behavior using Unity's NavMesh navigation system.
/// Handles pathfinding towards the player's base, path occlusion fallback logic 
/// (attacking blocking structures like walls/towers), combat damage execution, and reward generation upon death.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>Cached Transform reference to the player's core base structure destination.</summary>
    private Transform target;

    /// <summary>The internal NavMeshAgent component responsible for path calculations and map navigation.</summary>
    private NavMeshAgent agent;

    /// <summary>Reference to the UI HealthBar script to visually mirror health fluctuations.</summary>
    public HealthBar healthBar;

    /// <summary>Internal tracker caching the enemy's initial maximum health value threshold.</summary>
    private int maxHP;

    /// <summary>The current active health points remaining for this unit.</summary>
    public int hp = 100;

    /// <summary>The mandatory cooldown time delay in seconds required between consecutive structural hits.</summary>
    private float timeBetweenAttacks = 1f;

    /// <summary>Internal cooldown countdown timer tracking attack pacing metrics.</summary>
    private float attackTimer = 0f;

    /// <summary>
    /// Standard Unity callback. Fetches component references, caches maximum health metrics, 
    /// and localizes the main player core object within the scene hierarchy.
    /// </summary>
    void Start()
    {
        maxHP = hp;
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("PlayerBase").transform;
    }

    /// <summary>
    /// Standard Unity callback. Validates target availability, handles suicide-strike damage logic upon 
    /// reaching the base, evaluates navigation paths for physical blocks, and updates movement vectors.
    /// </summary>
    void Update()
    {
        if (target == null) return;

        // Suicide-strike mechanism when the enemy reaches proximity thresholds of the player core base
        if (Vector3.Distance(transform.position, target.position) < 1.5f)
        {
            target.GetComponent<PlayerBase>().ReceiveDamage(20);
            Destroy(gameObject);
            return;
        }

        // Logic check to verify if the navigation path is physically blocked by walls/towers or velocity dropped to near-zero
        bool isBlocked = agent.pathStatus == NavMeshPathStatus.PathPartial ||
                         (agent.hasPath && agent.velocity.sqrMagnitude < 0.1f && Vector3.Distance(transform.position, agent.destination) > 1f);

        if (isBlocked)
        {
            AttackNearestObstacle();
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }

    /// <summary>
    /// Iterates through all map assets tagged as "Building" to identify and pursue the physically closest 
    /// target structure, executing timed damage loops when within strike range.
    /// </summary>
    void AttackNearestObstacle()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestBuilding = null;

        // Spatial linear scan loop evaluating nearby structural grid entities
        foreach (GameObject building in buildings)
        {
            float d = Vector3.Distance(transform.position, building.transform.position);
            if (d < shortestDistance)
            {
                shortestDistance = d;
                nearestBuilding = building;
            }
        }

        if (nearestBuilding != null)
        {
            agent.SetDestination(nearestBuilding.transform.position);

            attackTimer -= Time.deltaTime;

            // Attack damage iteration evaluation based on proximity thresholds and cooldown timers
            if (shortestDistance <= 1.5f && attackTimer <= 0f)
            {
                nearestBuilding.GetComponent<Building>().ReceiveDamage(10);
                attackTimer = timeBetweenAttacks;
            }
        }
    }

    /// <summary>
    /// Deducts incoming numerical payloads from the tracking pool, refreshes graphical user interfaces, 
    /// rewards economy gold to the system upon zero health event loops, and disposes of the object.
    /// </summary>
    /// <param name="damage">The amount of raw damage points inflicted onto this enemy entity.</param>
    public void ReceiveDamage(int damage)
    {
        hp -= damage;
        if (healthBar != null) healthBar.UpdateBar(hp, maxHP);
        if (hp <= 0)
        {
            GameManager.instance.AddGold(5);
            Destroy(gameObject);
        }
    }
}