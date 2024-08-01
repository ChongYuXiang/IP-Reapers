/* Author: Chong Yu Xiang  
 * Filename: Enemy
 * Descriptions: Enemy AI, attack and health
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent zombie;
    public Transform target;
    public float min_range;
    public float max_range;

    private void Update()
    {
        if (zombie != null || target != null)
        {
            float dist = Vector3.Distance(target.transform.position, transform.position);

            if (dist <= max_range)
            {
                zombie.stoppingDistance = min_range;
                zombie.SetDestination(target.position);
            }
            if (dist <= min_range)
            {
                transform.LookAt(target.position);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
            
        }
    }
}
