/* Author: Chong Yu Xiang  
 * Filename: FaceCamera
 * Descriptions: Always face player camera
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        // Face target
        transform.LookAt(target.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - 180, 0);
    }
}
