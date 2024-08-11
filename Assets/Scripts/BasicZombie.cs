/* Author: Chong Yu Xiang  
 * Filename: BasicZombie
 * Descriptions: FSM for 'Undead' enemy class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.HighDefinition;

public class BasicZombie : MonoBehaviour
{
    public NavMeshAgent zombie;
    public Transform target;
    public float detectRange;
    public float patrolRange;
    public Transform centrePoint;
    public int health = 200;
    public GameObject bloodEffect;
    public Transform bloodSpot;

    private GameObject gameManager;
    private string currentState;
    private string nextState;
    private float dist;
    private Animator animator;
    private bool switchable = true;

    // Start is called before the first frame update
    private void Start()
    {
        // Set up variables
        if (target == null)
        {
            target = GameObject.Find("PlayerCapsule").transform;
        }
        animator = GetComponent<Animator>();
        dist = Vector3.Distance(zombie.transform.position, target.transform.position);

        // Start state
        currentState = "patrol";
        nextState = "patrol";
        SwitchState();
    }

    // For switching states
    private void SwitchState()
    {
        if (switchable)
        {
            StartCoroutine(currentState);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Update current state
        if (currentState != nextState)
        {
            currentState = nextState;
        }
        // Update distance from zombie to target
        dist = Vector3.Distance(zombie.transform.position, target.transform.position);

        if (health <= 0)
        {
            currentState = "dying";
        }
    }

    // Call to damage zombie
    public void damage(int damageAmt)
    {
        health -= damageAmt;
        bleed();

        // If no more health remaining
        if (health <= 0 && currentState != "dying")
        {
            // Switch to dying state
            nextState = "dying";
            currentState = nextState;
            SwitchState();
        }
    }

    // Neutral patrolling state
    IEnumerator patrol()
    {
        // Patrol animation
        animator.SetTrigger("patrol");
        zombie.stoppingDistance = 0;

        // While target is out of range
        while (dist >= detectRange && health == 200)
        {
            // While not moving
            if (zombie.remainingDistance <= 0.05 && currentState != "dying")
            {
                Vector3 point;
                // Find random location in patrol range
                if (RandomPoint(centrePoint.position, patrolRange, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    // Patrol
                    zombie.SetDestination(point);
                }
            }
            yield return new WaitForEndOfFrame();
        }
        // Target enters range
        yield return new WaitForEndOfFrame();
        nextState = "chasing";
        SwitchState();
    }

    IEnumerator chasing()
    {
        // Switch to chasing animation
        if (health > 0)
        {
            animator.SetTrigger("chase");
            zombie.stoppingDistance = 2;
            zombie.speed = 2.5f;
        }

        // While target is further than minimum range
        while (dist > zombie.stoppingDistance && currentState == "chasing")
        {
            // Chase target
            zombie.SetDestination(target.position);
            yield return new WaitForEndOfFrame();
        }
        // Target is at minimum range
        yield return new WaitForEndOfFrame();
        nextState = "attacking";
        SwitchState();
    }

    IEnumerator attacking()
    {
        // Start attacking
        yield return new WaitForSeconds(0.1f);
        if (health > 0)
        {
            StartCoroutine(attackLoop());
            animator.SetTrigger("attack");
        }

        // While target is within minimum range
        while (dist <= zombie.stoppingDistance)
        {
            // Always look at  target
            transform.LookAt(target.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            yield return new WaitForEndOfFrame();
        }
        // Target exits minimum range
        StopCoroutine(attackLoop());
        yield return new WaitForEndOfFrame();
        nextState = "chasing";
        SwitchState();
    }

    IEnumerator dying()
    {
        // Stop chasing
        zombie.SetDestination(zombie.transform.position);
        switchable = false;
        // Zombie dies
        animator.SetTrigger("death");
        AudioManager.instance.PlaySFX("ZombieDeath");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    IEnumerator attackLoop()
    {
        // While target is within minimum range and alive
        while (dist <= zombie.stoppingDistance)
        {
            // Send 10 damage per second
            yield return new WaitForSeconds(0.5f);
            if (currentState == "attacking")
            {
                gameManager = GameObject.Find("GameManager");
                gameManager.GetComponent<GameManager>().AdjustHealth(-10);
                AudioManager.instance.PlaySFX("ZombieAttack");
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    // Function to select random point within patrol range
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        // Get random point in a sphere
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void bleed()
    {
        // Spawn a blood effect on zombie for 0.3 seconds
        GameObject clone = (GameObject)Instantiate(bloodEffect, bloodSpot.position, Quaternion.identity);
        Destroy(clone, 0.3f);
    }
}

