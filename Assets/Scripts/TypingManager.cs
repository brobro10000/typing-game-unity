using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TypingManager : MonoBehaviour
{
    public int userWPM = 35;
    public string hasTyped = "";
    public int currentlySelectedWord = 0;
    public static int score = 100;

    // TODO: makes sense to move this to different class
    public Text display;
    public Text scoreDisplay;

    public AudioClip audioData;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = "Score: " + score;
        string input = Input.inputString;
        if (input.Equals(""))
            return;

        if (input[0] >= '0' && input[0] <= '9')
        {
            currentlySelectedWord = (int)(input[0] - '0');
            hasTyped = input[0] + ". ";
            display.text = hasTyped;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (SpawnManager.spawnedText[currentlySelectedWord] == null)
            {
                display.text = "Ooh! Just missed it!";
                hasTyped = "";
                return;
            }

            bool correct = SpawnManager.spawnedText[currentlySelectedWord].CheckCorrectness(hasTyped);
            if (correct)
            {
                AudioSource.PlayClipAtPoint(audioData, 0.9f * Camera.main.transform.position + 0.1f * transform.position, 20f);
                SpawnManager.spawnedText[currentlySelectedWord] = SpawnManager.spawnedText[currentlySelectedWord].Destroy();
                Destroy(SpawnManager.spawnedObjs[currentlySelectedWord]);

                hasTyped = "";
                display.text = "Good job! You got it right!";

                Timer.incrementTime(MainMenu.scale);

                score += 15;
                scoreDisplay.text = "Score: " + score;
                return;
            }
            // possibly keep the text the user entered
            hasTyped = "";
            display.text = "Oh no! Incorrect!";

            Timer.decrementTime(MainMenu.scale);

            score -= 10;
            scoreDisplay.text = "Score: " + score;
            return;
        }
        else if(Input.GetKeyDown(KeyCode.Backspace) && hasTyped.Length > 3)
        {
            hasTyped = hasTyped.Remove(hasTyped.Length - 1);
            display.text = hasTyped;
            return;
        }

        hasTyped += input[0];

        display.text = hasTyped;
    }
}
