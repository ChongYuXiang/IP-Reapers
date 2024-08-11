/* Author: Chong Yu Xiang  
 * Filename: FreakZombie
 * Descriptions: FSM for 'Freak' enemy class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FreakZombie : MonoBehaviour
{
    public NavMeshAgent zombie;
    public Transform target;
    public int health = 300;
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
        target = GameObject.Find("PlayerCapsule").transform;
        animator = GetComponent<Animator>();
        dist = Vector3.Distance(zombie.transform.position, target.transform.position);

        // Start state
        currentState = "idle";
        nextState = "idle";
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

    // Neutral idle state
    IEnumerator idle()
    {
        // While target is out of range
        while (health == 300)
        {
            // Do nothing
            yield return new WaitForEndOfFrame();
        }
        // Target enters range
        yield return new WaitForEndOfFrame();
        AudioManager.instance.PlaySFX("Freak");
        nextState = "chasing";
        SwitchState();
    }

    IEnumerator chasing()
    {
        // Switch to chasing animation
        if (health > 0)
        {
            animator.SetTrigger("chase");
        }

        // While target is further than minimum range
        while (dist > zombie.stoppingDistance && currentState != "dying")
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
        yield return new WaitForSeconds(0.1f);
        // Start attacking
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
                gameManager.GetComponent<GameManager>().AdjustHealth(-40);
                AudioManager.instance.PlaySFX("ZombieAttack");
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void bleed()
    {
        // Spawn a blood effect on zombie for 0.3 seconds
        GameObject clone = (GameObject)Instantiate(bloodEffect, bloodSpot.position, Quaternion.identity);
        Destroy(clone, 0.3f);
    }
}
