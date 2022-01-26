using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("home"))
        {
            SceneManager.LoadScene(1);
        }
        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
