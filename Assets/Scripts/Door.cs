using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class Door : MonoBehaviour
{

    IEnumerator Change()
    {
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(0, 359, 0);
            while (transform.eulerAngles.y > 270)
            {
                transform.Rotate(0, -110 * Time.deltaTime, 0);
                yield return new WaitForEndOfFrame();
            }
            transform.eulerAngles = new Vector3(0, 270, 0);
        }
        else if (transform.eulerAngles.y == 270)
        {
            while (transform.eulerAngles.y < 357)
            {
                Debug.Log(transform.eulerAngles.y);
                transform.Rotate(0, 110 * Time.deltaTime, 0);
                yield return new WaitForEndOfFrame();
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
