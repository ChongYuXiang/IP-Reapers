using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField]
    GameObject axe;

    private bool attacking = false;

    IEnumerator Attack()
    {
        attacking = true;
        for (int i = 0; i < 20; ++i)
        {
            axe.transform.Rotate(250 * Time.deltaTime, -100 * Time.deltaTime, 0);
            yield return new WaitForSeconds(0.02f);
        }
        attacking = false;
        for (int i = 0; i < 20; ++i)
        {
            axe.transform.Rotate(-250 * Time.deltaTime, 100 * Time.deltaTime, 0);
            yield return new WaitForSeconds(0.02f);
        }
    }

}
