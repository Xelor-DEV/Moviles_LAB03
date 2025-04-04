using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Score Data", menuName = "ScriptableObjects/Player Data/Score Data", order = 1)]
public class ScoreData : ScriptableObject
{
    [SerializeField] private Action<int> onScoreChanged;
    [SerializeField] private Action<int> onHighScoreChanged;
    [SerializeField] private int currentScore;
    [SerializeField] private int highScore;

    public int CurrentScore
    {
        get 
        { 
            return currentScore; 
        }
        set
        {
            currentScore = value;
            onScoreChanged?.Invoke(currentScore);

            if (currentScore > highScore)
            {
                HighScore = currentScore;
            }
        }
    }

    public int HighScore
    {
        get 
        { 
            return highScore; 
        }
        set
        {
            highScore = value;
            onHighScoreChanged?.Invoke(highScore);
        }
    }

    public void ResetCurrentScore()
    {
        CurrentScore = 0;
    }

    public void ResetHighScore()
    {
        HighScore = 0;
    }
}