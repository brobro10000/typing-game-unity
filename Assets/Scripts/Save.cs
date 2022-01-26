using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    public Button saveButton; 
    
    // Start is called before the first frame update
    void Start()
    {
        saveButton.onClick.AddListener(() => CreateDatabase.SaveFile());
        Debug.Log("OnClickListener added!");
    }
}
