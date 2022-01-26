using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Question
{
    public bool isMultChoice;
    public string term;
    public string def;
    public int score;
    private DisplayedText text;

    public Question(string _term, string _def, int _score)
    {
        this.term = _term;
        this.def = _def;
        this.score = _score;
        this.isMultChoice = false;
    }

    public Question(string _term, string _def, int _score, bool _isMultChoice)
    {
        this.term = _term;
        this.def = _def;
        this.score = _score;
        this.isMultChoice = _isMultChoice;
    }

    public void CreateDisplayedText(int itemNum)
    {
        this.text = new DisplayedText(itemNum, this.def, this.term);
    }

    public int GetItemNum()
    {
        return this.text.GetItemNum();
    }

    public string GetDisplayedText()
    {
        return this.text.GetDisplayedText();
    }

    public void SetItemNum(int num)
    {
        this.text.SetItemNum(num);
    }

    public Question Destroy()
    {
        this.text.Destroy();
        return null;
    }

    public bool CheckCorrectness(string hasTyped)
    {
        Debug.Log("spawnedText: " + this.term);
        Debug.Log("hastyped: " + hasTyped);
        bool correct = this.term.ToLower().Equals(hasTyped.Substring(3).ToLower());

        if (correct)
            this.score += 2;
        else
            this.score--;

        return correct;
    }
}

public class QuestionBank
{
    private Question[] chosenQuestions;
    public List<Question> questions;
    private string filename;
    private int numTerms;

    public QuestionBank(string filename)
    {
        this.filename = filename;
        questions = new List<Question>();

        using (var reader = new StreamReader(@".\Assets\QuestionBanks\" + filename))
        {
            // consume filename
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                Debug.Log(values[0] + "," + values[1]);
                values[1] = values[1].Replace('\\', '\n');
                questions.Add(new Question((string)(values[0]), (string)values[1], Int32.Parse(values[2])));
            }
        }

        System.Random rand = new System.Random();
        int randNumTerms = rand.Next(8, 20);
        numTerms = (randNumTerms > questions.Count) ? questions.Count : randNumTerms;

        SortQuestionsByScore();

        chosenQuestions = new Question[questions.Count];
        questions.CopyTo(chosenQuestions);

        // removes highest scoring questions
        while (questions.Count > numTerms)
            questions.RemoveAt(questions.Count - 1);
    }

    public void SortQuestionsByScore()
    {
        bool delta = false;

        for (int i = 0; i < questions.Count; i++)
        {
            delta = false;
            for (int j = i + 1; j < questions.Count; j++)
            {
                if (questions[j - 1].score > questions[j].score)
                {
                    Question temp = questions[j - 1];
                    questions[j - 1] = questions[j];
                    questions[j] = temp;

                    delta = true;
                }
            }

            if (!delta)
                break;
        }
    }

    public Question GetRandomQuestionAndAnswer()
    {
        System.Random rand = new System.Random();
        int idx = rand.Next(0, questions.Count);

        Question select = this.questions[idx];
        this.questions.RemoveAt(idx);

        return select;
    }

    public void saveProgress()
    {
        StringBuilder csv = new StringBuilder();

        foreach (Question q in chosenQuestions)
            csv.AppendLine($"{q.term},{q.def},{q.score}");

        File.WriteAllText(@".\Assets\QuestionBanks\" + this.filename, csv.ToString());    }
}