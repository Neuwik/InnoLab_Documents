using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAIMovement : MonoBehaviour
{
    private Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    //public int health;

    // Patroling
    private Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    //private bool alreadyAttacked;
    public GameObject projectile;

    // States
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    private const float DISTANCE_TO_WALK_POINT_THRESHOLD = 1f;
    private const float PROJECTILE_FORCE = 32f;
    private const float PROJECTILE_UP_FORCE = 8f;
    private const float MOVEMENT_SPEED = 5f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        UpdatePlayerState();
    }

    private void UpdatePlayerState()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();
        if (walkPointSet)
            MoveTowardsWalkPoint();

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < DISTANCE_TO_WALK_POINT_THRESHOLD)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) // Adjust the raycast length as needed
            walkPointSet = true;
    }

    private void MoveTowardsWalkPoint()
    {
        //transform.LookAt(walkPoint);
        //transform.Translate(Vector3.forward * MOVEMENT_SPEED * Time.deltaTime);

        Vector3 direction = new Vector3(walkPoint.x - transform.position.x, 0, walkPoint.z - transform.position.z);
        MoveInDirection(direction);
    }

    private void ChasePlayer()
    {
        //transform.LookAt(player);
        //transform.Translate(Vector3.forward * MOVEMENT_SPEED * Time.deltaTime);
        Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z);
        MoveInDirection(direction);
    }

    private void MoveInDirection(Vector3 direction)
    {
        direction = direction.normalized;
        SpriteRenderer sprite = FindSpriteRenderer();
        if (sprite != null)
        {
            if (direction.x < 0)
            {
                if (sprite.flipX)
                {
                    sprite.flipX = false;
                }
            }
            else
            {
                if (!sprite.flipX)
                {
                    sprite.flipX = true;
                }
            }
        }
        transform.Translate(direction * MOVEMENT_SPEED * Time.deltaTime);
    }

    private SpriteRenderer FindSpriteRenderer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.tag == "Sprite")
            {
                return transform.GetChild(i).gameObject.GetComponent("SpriteRenderer") as SpriteRenderer;
            }

        }
        return null;
    }

    private void AttackPlayer()
    {
        /*transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack code here
            Debug.Log("Attacking");
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * PROJECTILE_FORCE, ForceMode.Impulse);
            rb.AddForce(transform.up * PROJECTILE_UP_FORCE, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }*/
    }

    /*private void ResetAttack()
    {
        alreadyAttacked = false;
    }*/

    /*public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
    }*/

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
