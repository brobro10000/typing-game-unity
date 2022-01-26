using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static float timeScale = 15*12;//1 is placeholder for # of words running in the game
    public float easyScale = 1;
    public float mediumScale = .75f;
    public float hardScale = .5f;
    public static float scale;

    public void PlayGame()
    {
       SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
       Debug.Log("QUIT!");
       Application.Quit();
    }
    public void GoBack()
    {
       SceneManager.LoadScene(0);
    }

    public void EasyDifficulty()
    {
        TimeVariable(15 * easyScale);
        Timer.incrementTime(easyScale);
        scale = easyScale;
        SpawnManager.RefreshGameState();
    }

    public void MediumDifficulty()
    {
        TimeVariable(15 * mediumScale);
        Timer.incrementTime(mediumScale);
        scale = mediumScale;
        SpawnManager.RefreshGameState();
    }

    public void HardDifficulty()
    {
        TimeVariable(15 * .5f);
        Timer.incrementTime(hardScale);
        scale = hardScale;
        SpawnManager.RefreshGameState();
    }

    public static float TimeVariable(float listCount)
    {
        float wordCount = listCount;
        timeScale = wordCount * 6;//1 is placeholder for # of words running in the game

        return timeScale;
    }

    public static float passVariable()
    {
        return timeScale;
    }
}
