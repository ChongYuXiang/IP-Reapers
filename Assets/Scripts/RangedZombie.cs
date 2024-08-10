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
        // Start attacking
        if (currentState != "dying")
        {
            StartCoroutine(attackLoop());
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
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    IEnumerator attackLoop()
    {
        // While target is within minimum range and alive
        while (dist <= zombie.stoppingDistance && currentState != "dying")
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
                // Wait for attack animation to end
                yield return new WaitForSeconds(0.2f);

                // Idle in between attacks
                animator.SetTrigger("idle");
                yield return new WaitForSeconds(1f);
                Destroy(instantiatedProjectile.gameObject);
            }
        }
    }
}
