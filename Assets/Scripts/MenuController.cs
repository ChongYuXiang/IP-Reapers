/* Author: Chong Yu Xiang  
 * Filename: MenuController
 * Descriptions: Functions for UI buttons
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject background;
    public GameObject originalPos;
    public GameObject targetPos;
    public GameObject settings;
    public GameObject tutorial;
    public GameObject credits;

    private string movingBG;

    private void Update()
    {
        if (movingBG == "forward")
        {
            background.transform.position = Vector3.MoveTowards(background.transform.position, targetPos.transform.position, 3500 * Time.deltaTime);
        }
        else if (movingBG == "backward")
        {
            background.transform.position = Vector3.MoveTowards(background.transform.position, originalPos.transform.position, 3500 * Time.deltaTime);
        }
    }

    public void SwitchToSettings()
    {
        settings.SetActive(true);
        tutorial.SetActive(false);
        credits.SetActive(false);
        movingBG = "forward";
    }

    public void SwitchToTutorial()
    {
        settings.SetActive(false);
        tutorial.SetActive(true);
        credits.SetActive(false);
        movingBG = "forward";
    }

    public void SwitchToCredits()
    {
        settings.SetActive(false);
        tutorial.SetActive(false);
        credits.SetActive(true);
        movingBG = "forward";
    }

    public void BackButton()
    {
        movingBG = "backward";
    }

    // Quit Button
    public void QuitGame()
    {
        // Close unity build
        Application.Quit();
    }
}
