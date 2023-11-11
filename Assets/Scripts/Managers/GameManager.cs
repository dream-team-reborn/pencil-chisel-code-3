using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Declare a private static instance variable
    private static GameManager instance = null;

    // Create a public accessor that will get the instance
    public static GameManager Instance
    {
        get
        {
            // Test if the instance is null
            // If so, try to get it using FindObjectOfType
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private int playerNumber = 0;
    [SerializeField]
    private GameObject characterPrefab;
    [SerializeField]
    private Vector3[] spawnPositions;
    
    private List<GameObject> playerCharacters = new List<GameObject>();
    private MoveUp oil;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UIManager.OnClicked += OnButtonClicked;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        if(scene.name == "Level")
        {
            StartMatch();
        }
    }

    void OnButtonClicked(string buttonName)
    {
        switch (buttonName)
        {
            case "Play":
                SceneManager.LoadScene("Level");
                break;

            case "1 Player":
                playerNumber = 1;
                break;

            case "2 Players":
                playerNumber = 2;
                break;

            case "3 Players":
                playerNumber = 3;
                break;

            case "4 Players":
                playerNumber = 4;
                break;
        }
    }

    void StartMatch()
    {
        if (playerNumber > 4)
        {
            playerNumber = 4;
        }
        else if (playerNumber < 1)
        {
            playerNumber = 1;
        }

        for (int i = 0; i < playerNumber; i++)
        {
            GameObject newPlayer = Instantiate(characterPrefab, spawnPositions[i], Quaternion.identity);
            newPlayer.GetComponent<CharacterMovement>().PlayerID = i;
            playerCharacters.Add(newPlayer);

        }

        oil = FindObjectOfType<MoveUp>();
        oil.StartMovement();
    }

    void EndMatch()
    {
        playerNumber = 0;
        oil.StopMovement();
    }
}
