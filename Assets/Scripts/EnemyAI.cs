using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    private Animator animator;
    private AudioSource audio;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float hp;

    [SerializeField]
    public AudioClip[] clips;
    private bool soundplayed = false;

    //Attack
    public float timeBetweenAttacks;
    bool attacked;

    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange, attackRange;
    public bool playerInSight, playerInRange;
    private bool spawned = true;
    private bool wasAttacked = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSight && !playerInRange && !wasAttacked) Patrol();
        if ((playerInSight && !playerInRange) || wasAttacked) Chase();
        if (playerInSight && playerInRange) Attack();
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
        else agent.SetDestination(walkPoint);

        Vector3 distanceToWalk = transform.position - walkPoint;

        if (distanceToWalk.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        if (spawned)
        {
            walkPoint = new Vector3(transform.position.x, transform.position.y, 40);
        }

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
            spawned = false;
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!attacked)
        {
            attacked = true;
            player.GetComponent<PlayerScript>().LoseHP();
            animator.SetBool("IsAttacking", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        attacked = false;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        wasAttacked = true;
        int pickedClip = Random.Range(0, clips.Length-1);
        //Debug.Log(pickedClip);
        audio.PlayOneShot(clips[pickedClip]);

        if (hp <= 0)
        {
            animator.SetBool("isDead", true);
            Invoke(nameof(Die), .5f);
        }
    }

    private void Die()
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
