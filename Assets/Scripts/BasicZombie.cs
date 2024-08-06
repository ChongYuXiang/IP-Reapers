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

    string currentState;
    string nextState;

    private float dist;

    private void Start()
    {
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
        Debug.Log(dist);
        Debug.Log(min_range);
        Debug.Log(max_range);

        nextState = "chasing";
        yield return new WaitForEndOfFrame();
        SwitchState();
    }

    IEnumerator chasing()
    {
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
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}

