/* Author: Chong Yu Xiang  
 * Filename: MenuController
 * Descriptions: Functions for main menu, game over and victory screens
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
    public Slider _VolumeSlider;

    private string movingBG;

    // Update is called once per frame
    private void Update()
    {
        if (movingBG == "forward") // Move background left
        {
            background.transform.position = Vector3.MoveTowards(background.transform.position, targetPos.transform.position, 3500 * Time.deltaTime);
        }
        else if (movingBG == "backward") // Move background right
        {
            background.transform.position = Vector3.MoveTowards(background.transform.position, originalPos.transform.position, 3500 * Time.deltaTime);
        }
    }

    // Switch to settings UI
    public void SwitchToSettings()
    {
        settings.SetActive(true);
        tutorial.SetActive(false);
        credits.SetActive(false);
        movingBG = "forward";
    }
    // Switch to turorial UI
    public void SwitchToTutorial()
    {
        settings.SetActive(false);
        tutorial.SetActive(true);
        credits.SetActive(false);
        movingBG = "forward";
    }
    // Switch to credits UI
    public void SwitchToCredits()
    {
        settings.SetActive(false);
        tutorial.SetActive(false);
        credits.SetActive(true);
        movingBG = "forward";
    }
    // Move back to main page
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


    // Tell audio manager to toggle BGM
    public void ToggleBGM()
    {
        AudioManager.instance.ToggleBGM();
    }
    // Tell audio manager to toggle SFX
    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }
    // Tell audio manager to adjust volume
    public void Volume()
    {
        AudioManager.instance.Volume(_VolumeSlider.value);
    }
}
