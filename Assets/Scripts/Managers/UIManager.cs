using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Declare a private static instance variable
    private static UIManager instance = null;

    // Create a public accessor that will get the instance
    public static UIManager Instance
    {
        get
        {
            // Test if the instance is null
            // If so, try to get it using FindObjectOfType
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    // Use Awake to set the instance
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public delegate void ClickAction(string buttonName);
    public static event ClickAction OnClicked;

    [SerializeField]
    GameObject startMenu;
    [SerializeField]
    GameObject gameMenu;
    [SerializeField]
    GameObject endMenu;
    [SerializeField]
    TMPro.TMP_Text playerLabel;
    [SerializeField]
    TMPro.TMP_Text winnerLabel;
    [SerializeField]
    TMPro.TMP_Text timerLabel;

    public void OnButtonPressed(string buttonName)
    {
        OnClicked(buttonName);

        switch (buttonName)
        {
            case "1 Player":
                playerLabel.text = "Players: 1";
                break;

            case "2 Players":
                playerLabel.text = "Players: 2";
                break;

            case "3 Players":
                playerLabel.text = "Players: 3";
                break;

            case "4 Players":
                playerLabel.text = "Players: 4";
                break;
        }
    }

    public void OnGameUpdate(float timePassed)
    {
        UpdateTimer(((int)timePassed));
    }

    private void UpdateTimer(int timeToShow)
    {
        if (timeToShow >= 10)
        {
            timerLabel.text = timeToShow.ToString();
        }
        else
        {
            timerLabel.text = "0" + timeToShow.ToString();
        }
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
