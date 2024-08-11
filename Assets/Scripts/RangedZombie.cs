/* Author: Chong Yu Xiang  
 * Filename: BasicZombie
 * Descriptions: FSM for 'Bombardier' enemy class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedZombie : MonoBehaviour
{
    public NavMeshAgent zombie;
    public Transform target;
    public Rigidbody projectile;
    public float detectRange;
    public int health = 100;
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

    // Neutral idle state
    IEnumerator idle()
    {
        // Switch to idle animation
        animator.SetTrigger("idle");
        zombie.SetDestination(zombie.transform.position);   

        // While target is out of range
        while (dist >= detectRange && currentState != "dying")
        {
            // Do nothing
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
        animator.SetTrigger("patrol");

        // While target is further than minimum range
        while (dist > zombie.stoppingDistance && dist < detectRange && currentState != "dying")
        {
            // Chase target
            zombie.SetDestination(target.position);
            yield return new WaitForEndOfFrame();
        }
        // If target is at minimum range
        if (dist < zombie.stoppingDistance)
        {
            yield return new WaitForEndOfFrame();
            nextState = "attacking";
            SwitchState();
        }
        // If target is out of range
        if (dist > detectRange)
        {
            yield return new WaitForEndOfFrame();
            nextState = "idle";
            SwitchState();
        }
    }

    IEnumerator attacking()
    {
        yield return new WaitForSeconds(0.1f);
        // Start attacking
        StartCoroutine(attackLoop());

        // While target is within minimum range
        while (dist <= zombie.stoppingDistance && currentState == "attacking")
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
        while (dist <= zombie.stoppingDistance && health > 0)
        {
            // Start attack
            animator.SetTrigger("attack");
            yield return new WaitForSeconds(0.5f);
            if (currentState == "attacking")
            {
                // Create clone of projectile
                Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position + new Vector3(0, 1.4f, 0), transform.rotation) as Rigidbody;
                // Send projectile forwards
                instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, 25));
                yield return new WaitForSeconds(0.3f);

                // Send 15 damage
                gameManager = GameObject.Find("GameManager");
                gameManager.GetComponent<GameManager>().AdjustHealth(-15);
                AudioManager.instance.PlaySFX("Bombardier");
                // Wait for attack animation to end
                yield return new WaitForSeconds(0.2f);

                
                // Idle in between attacks
                if (currentState == "attacking")
                {
                    animator.SetTrigger("idle");
                }

                // Destroy projectile after 1 second
                yield return new WaitForSeconds(1f);
                Destroy(instantiatedProjectile.gameObject);
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
