/* Author: Chong Yu Xiang  
 * Filename: SceneChanger
 * Descriptions: For changing scenes and switching BGM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Load scene based on given index
    public void ChangeScene(int sceneIndex)
    {
        if (sceneIndex == 0)
        {
            AudioManager.instance.BGMSource.Stop();
            AudioManager.instance.PlayBGM("Menu");
            Cursor.visible = true;
        }
        if (sceneIndex == 1)
        {
            AudioManager.instance.BGMSource.Stop();
            AudioManager.instance.PlayBGM("Ambience");
            Cursor.visible = false;
        }
        if (sceneIndex == 2)
        {
            AudioManager.instance.BGMSource.Stop();
            AudioManager.instance.PlayBGM("Police");
        }
        if (sceneIndex == 4)
        {
            AudioManager.instance.BGMSource.Stop();
            AudioManager.instance.PlayBGM("Victory");
            Cursor.visible = true;
        }
        if (sceneIndex == 5)
        {
            AudioManager.instance.BGMSource.Stop();
            AudioManager.instance.PlayBGM("Loss");
            Cursor.visible = true;
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
