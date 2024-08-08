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
    public float min_range;
    public float max_range;
    public int health = 5;

    private GameObject gameManager;
    private string currentState;
    private string nextState;
    private float dist;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        dist = Vector3.Distance(zombie.transform.position, target.transform.position);
        currentState = "patrol";
        nextState = "patrol";
        SwitchState();
    }
    private void SwitchState()
    {
        Debug.Log(currentState);
        StartCoroutine(currentState);
    }
    private void Update()
    {
        if (currentState != nextState)
        {
            currentState = nextState;
        }
        dist = Vector3.Distance(zombie.transform.position, target.transform.position);
    }

    public void damage()
    {
        Debug.Log(health);
        health -= 1;
        if (health <= 0)
        {
            nextState = "dying";
            SwitchState();
        }
    }

    IEnumerator patrol()
    {
        while (dist >= max_range)
        {
            yield return new WaitForEndOfFrame();
        }
        nextState = "chasing";
        yield return new WaitForEndOfFrame();
        SwitchState();
    }

    IEnumerator chasing()
    {
        animator.SetTrigger("chase");
        while (dist > min_range)
        {
            zombie.stoppingDistance = min_range;
            zombie.SetDestination(target.position);
            yield return new WaitForEndOfFrame();
        }
        nextState = "attacking";
        yield return new WaitForEndOfFrame();
        SwitchState();
    }

    IEnumerator attacking()
    {
        StartCoroutine(attackLoop());
        animator.SetTrigger("attack");
        while (dist <= min_range)
        {
            transform.LookAt(target.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            yield return new WaitForEndOfFrame();
        }
        nextState = "chasing";
        yield return new WaitForEndOfFrame();
        SwitchState();
    }

    IEnumerator dying()
    {
        zombie.SetDestination(zombie.transform.position);
        animator.SetTrigger("death");
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    IEnumerator attackLoop()
    {
        while (true)
        {
            gameManager = GameObject.Find("GameManager");
            gameManager.GetComponent<GameManager>().AdjustHealth(-15);
            yield return new WaitForSeconds(1);
        }
    }
}

