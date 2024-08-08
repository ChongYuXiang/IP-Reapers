/* Author: Chong Yu Xiang  
 * Filename: Door
 * Descriptions: Opening and closing doors
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private bool opened = false;
    private bool turning = false;
    private float openDuration = 0.8f;
    private float currentDuration;

    private Vector3 startRotation;
    private Vector3 targetRotation;

    private void Update()
    {
        if (turning)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / openDuration;
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);

            if (currentDuration >= openDuration)
            {
                currentDuration = 0f;
                turning = false;
                transform.eulerAngles = targetRotation;
                opened = !opened;
            }
        }
    }

    //Handles the opening of door
    public void OpenDoor()
    {
        if (!turning)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.y -= 90f;

            turning = true;
        }
    }

    public void CloseDoor()
    {
        if (!turning)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.y += 90f;

            turning = true;
        }
    }

    public void Change()
    {
        if (!opened)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
}