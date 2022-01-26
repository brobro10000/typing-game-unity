using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CreateDatabase : MonoBehaviour
{
    public GameObject textField;
    private Transform saveButton;
    public GameObject mainMenu;
    public GameObject dbMenu;
    public static bool shouldSave = false;
    
    // Start is called before the first frame update
    void Start()
    {
        saveButton = GameObject.FindGameObjectWithTag("SaveButton").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SpawnNewTextField();
        else if (shouldSave)
        {
            Debug.Log("About to save a new database!");
            SaveNewDatabase();

            mainMenu.SetActive(true);
            dbMenu.SetActive(false);
        }
    }

    public static void SaveFile()
    {
        Debug.Log("Save changed to true!");
        shouldSave = true;
    }

    public void SpawnNewTextField()
    {
        GameObject obj = Instantiate(textField);
        obj.transform.SetParent(GameObject.FindGameObjectWithTag("TermBox").transform);
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void SaveNewDatabase()
    {
        Debug.Log("Saving a new database!");
        shouldSave = false;
        StringBuilder csv = new StringBuilder();
        Transform termBox = GameObject.FindGameObjectWithTag("TermBox").transform;
        int numTerms = termBox.childCount;
        string filename = GameObject.FindGameObjectWithTag("FileNameInput").transform.GetChild(2).transform.GetComponent<Text>().text;

        Debug.Log(filename);
        csv.AppendLine($"{filename}");

        for (int i = 0; i < numTerms; i++)
        {
            Transform g = termBox.transform.GetChild(i);
            string term = g.GetChild(0).transform.GetChild(2).transform.GetComponent<Text>().text;
            string def = g.GetChild(1).transform.GetChild(2).transform.GetComponent<Text>().text;

            if (term.Equals("") || def.Equals(""))
                continue;

            csv.AppendLine($"{term},{def},50");
        }

        File.WriteAllText(@".\Assets\QuestionBanks\" + filename + ".csv", csv.ToString());

        using (StreamWriter sw = File.AppendText(@".\Assets\Other Assets\filenames.txt"))
        {
            sw.WriteLine(filename + ".csv");
        }
    }
}
