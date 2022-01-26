using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DisplayedText
{
    private int itemNum = -1;
    private string displayedText = null;
    private string answerText = null;
    private int score = 0;
    private static List<string> terms = null;
    private static List<string> definitions = null;
    private static List<int> scores = null;

    public DisplayedText(int i, string display, string answer)
    {
        itemNum = i;
        displayedText = this.itemNum.ToString() + ". " + display;
        answerText = answer;
    }

    public void SetItemNum(int num)
    {
        itemNum = num;
    }

    public void SetDisplayedText(string text)
    {
        displayedText = text;
    }

    public void SetAnswerText(string text)
    {
        answerText = text;
    }

    public int GetItemNum()
    {
        return this.itemNum;
    }

    public string GetDisplayedText()
    {
        return this.displayedText;
    }

    public string GetAnswerText()
    {
        return this.answerText;
    } 

    public DisplayedText Destroy()
    {
        SpawnManager.AddBackToQueue(this.itemNum);

        return null;
    }

    private void GetRandomQuestionAndAnswer(List<string> availWords, List<string> availDefs)
    {
        if (terms == null || definitions == null)
            InitTermsAndAnswers("GameTerminology.csv");

        System.Random rand = new System.Random();
        int idx = rand.Next(0, terms.Count);

        this.displayedText = availDefs[idx];
        this.answerText = availWords[idx];
        this.score = scores[idx];

        terms.RemoveAt(idx);
        definitions.RemoveAt(idx);
        scores.RemoveAt(idx);
    }

    public void InitTermsAndAnswers(string filename)
    {
        using(var reader = new StreamReader(@".\Assets\QuestionBanks\" + filename))
        {
            terms = new List<string>();
            definitions = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                terms.Add(values[0]);
                definitions.Add(values[1]);
                scores.Add(Int32.Parse(values[2]));
            }
        }
    }
}