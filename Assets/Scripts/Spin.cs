/* Author: Chong Yu Xiang  
 * Filename: Spin
 * Descriptions: Spin object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rotateZ;

    // Update is called once per frame
    void Update()
    {
        // Rotate object over time
        transform.Rotate(0, 0, rotateZ * Time.deltaTime, Space.Self);
    }
}
