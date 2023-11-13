using System;
using com.trashpandaboy.core;
using TMPro;
using UnityEngine;

public class UIManager : Manager<UIManager>
{
    public static event Action<string> OnClicked;
    public static event Action<int> OnPlayerSelected;

    [SerializeField]
    GameObject startMenu;
    [SerializeField]
    GameObject gameMenu;
    [SerializeField]
    GameObject endMenu;
    [SerializeField]
    TMP_Text playerLabel;
    [SerializeField]
    TMP_Text winnerLabel;
    [SerializeField]
    TMP_Text timerLabel;

    public void OnPlayerSelectionClicked(int playerAmount)
    {
        playerLabel.text = $"Players: {playerAmount}";
        OnPlayerSelected(playerAmount);
    }

    public void OnButtonPressed(string buttonName)
    {
        OnClicked(buttonName);
    }

    public void OnGameUpdate(float timePassed)
    {
        UpdateTimer(((int)timePassed));
    }

    private void UpdateTimer(int timeToShow)
    {
        timerLabel.text = $"{timeToShow}".PadLeft(2, '0');
    }

    public void StartMatch()
    {
        timerLabel.text = "00";
        startMenu.SetActive(false);
        gameMenu.SetActive(true);
        endMenu.SetActive(false);
    }

    public void EndMatch(int winner)
    {
        startMenu.SetActive(false);
        
        if(winner >= 0)
        {
            winnerLabel.text = "Congratulations\nPlayer " + (winner + 1) + " won!";
        }
        else
        {
            winnerLabel.text = "Unlucky\nYou lost!";
        }

        endMenu.SetActive(true);
    }

    public void Reset()
    {
        playerLabel.text = "Players: 1";
        startMenu.SetActive(true);
        gameMenu.SetActive(false);
        endMenu.SetActive(false);
    }
}
