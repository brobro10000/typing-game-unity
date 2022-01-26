using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SpawnManager : MonoBehaviour
{
    public GameObject obj;
    public static Question[] spawnedText = new Question[10];
    public static GameObject[] spawnedObjs = new GameObject[10];
    private static List<QuestionBank> phaseBanks;
    public static Queue<int> numQ;
    public static List<string> filenames = null;
    public static List<int> phaseTimes = null; 
    private static float timer = 0.0f;
    private static float spawnClock = 0.0f;
    public static int lastUsedNum;

    void Start()
    {
        RefreshGameState();
    }

    void Update()
    {
        timer += Time.deltaTime;
        spawnClock += Time.deltaTime;

        if (timer > MainMenu.timeScale)
        {
            if (spawnClock > 15.0f)
            {
                EndGame();
                SceneManager.LoadScene(0);
            }
        }
        else if (spawnClock > 5.0f)
        {
            for (int i = 0; i < phaseBanks.Count; i++)
            {
                if (timer < phaseTimes[i])
                {
                    Debug.Log("spawned cell!");
                    SpawnNewCell(phaseBanks[i].GetRandomQuestionAndAnswer());
                    break;
                }
            }

            spawnClock = 0.0f;
        }
    }

    public static void AddBackToQueue(int num)
    {
        numQ.Enqueue(num);
    }

    public GameObject SpawnNewCell(Question txt)
    {
        int item = numQ.Dequeue();
        lastUsedNum = item;
        Debug.Log("Item num!" + item);

        txt.CreateDisplayedText(item);
        spawnedText[item] = txt;
        spawnedObjs[item] = Instantiate(obj);

        if (txt.GetDisplayedText().Length > 65)
            spawnedObjs[item].transform.GetChild(0).localScale = new Vector3(4.1f, 1.1f, 2.1f);

        spawnedObjs[item].transform.GetChild(1).GetComponent<Text>().text = txt.GetDisplayedText();

        spawnedObjs[item].transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);

        spawnedObjs[item].transform.GetComponent<CellProperties>().text = txt;

        return spawnedObjs[item];
    }

    public static void saveQuestionBanks()
    {
        foreach (QuestionBank bank in phaseBanks)
            bank.saveProgress();
    }

    public void EndGame()
    {
        saveQuestionBanks();
    }

    public static void RefreshGameState()
    {
        numQ = new Queue<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 });
        phaseBanks = new List<QuestionBank>();

        // PLACEHOLDER WHILE LIST SELECTION IS BEING DEVELOPED
        GetFilenames();

        foreach (string filename in filenames)
        {
            phaseBanks.Add(new QuestionBank(filename));
        }

        timer = 0.0f;
        spawnClock = 0.0f;
        spawnedText = new Question[10];
        spawnedObjs = new GameObject[10];

        int numQs = 0;

        phaseTimes = new List<int>();
        foreach (QuestionBank bank in phaseBanks)
        {
            Debug.Log("bank");
            numQs += bank.questions.Count;
            phaseTimes.Add(numQs * 5);
        }


        MainMenu.timeScale = (5 * numQs);
    }

    public static void GetFilenames()
    {
        filenames = new List<string>();

        using (StreamReader reader = new StreamReader(@".\Assets\Other Assets\filenames.txt"))
        {
            while (!reader.EndOfStream)
            {
                string filename = reader.ReadLine();
                Debug.Log(filename);
                filenames.Add(filename);
            }
        }

        System.Random rand = new System.Random();
        while (filenames.Count > 3)
        {
            filenames.RemoveAt(rand.Next(filenames.Count));
        }
    }
}
