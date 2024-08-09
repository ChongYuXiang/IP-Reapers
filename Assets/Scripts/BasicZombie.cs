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
    }

    // Call to damage zombie
    public void damage(int damageAmt)
    {
        health -= damageAmt;
        Debug.Log(health);

        // If no more health remaining
        if (health <= 0)
        {
            // Switch to dying state
            nextState = "dying";
            SwitchState();
            Debug.Log("IM FUCKING DEAD");
        }
    }

    // Neutral patrolling state
    IEnumerator patrol()
    {
        // Patrol animation
        animator.SetTrigger("patrol");
        zombie.stoppingDistance = 0;

        // While target is out of range
        while (dist >= detectRange)
        {
            // While not moving
            if (zombie.remainingDistance <= 0.05)
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
        animator.SetTrigger("chase");
        zombie.stoppingDistance = 2;
        zombie.speed = 2.5f;

        // While target is further than minimum range
        while (dist > zombie.stoppingDistance)
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
        StartCoroutine(attackLoop());
        animator.SetTrigger("attack");

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
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    IEnumerator attackLoop()
    {
        // While target is within minimum range and alive
        while (dist <= zombie.stoppingDistance && currentState != "dying")
        {
            // Send 10 damage per second
            yield return new WaitForSeconds(0.5f);
            gameManager = GameObject.Find("GameManager");
            gameManager.GetComponent<GameManager>().AdjustHealth(-10);
            yield return new WaitForSeconds(0.5f);
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
}

