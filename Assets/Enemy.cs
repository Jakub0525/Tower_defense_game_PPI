using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;
    public HealthBar healthBar;
    private int maxHP;
    public int hp = 100;

    private float timeBetweenAttacks = 1f;
    private float attackTimer = 0f;

    void Start()
    {
        maxHP = hp;
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("PlayerBase").transform;
    }

    void Update()
    {
        if (target == null) return;

        if (Vector3.Distance(transform.position, target.position) < 1.5f)
        {
            target.GetComponent<PlayerBase>().ReceiveDamage(20);
            Destroy(gameObject);
            return;
        }

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

    void AttackNearestObstacle()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestBuilding = null;

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

            if (shortestDistance <= 1.5f && attackTimer <= 0f)
            {
                nearestBuilding.GetComponent<Building>().ReceiveDamage(10);
                attackTimer = timeBetweenAttacks; 
            }
        }
    }

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