using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    Image timerBar;
    float maxTime = MainMenu.passVariable();
    public static float timeLeft;
    // Start is called before the first frame update
    public static float incrementTime(float scale)
    {
        timeLeft += 2*scale;
        return timeLeft;
    }
    public static float decrementTime(float scale)
    {
        timeLeft -= 2 / scale;
        return timeLeft;
    }
    private void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    private void Update()
    {
        //need to add a delay to time, if visible object is visible to camera, start timer or something along those lines
        if (timeLeft > 0)
        {
            timeLeft -= Time.smoothDeltaTime;

            timerBar.fillAmount = timeLeft / maxTime;

            //need to reference if true for correct answer +x to maxTime
        } 
    }
}
