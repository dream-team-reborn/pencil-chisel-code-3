using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

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

        _audioSource = GetComponentInChildren<AudioSource>();
    }

    [SerializeField]
    private int playerNumber = 0;
    [SerializeField]
    private GameObject characterPrefab;
    [SerializeField]
    private Vector3[] spawnPositions;

    private bool isGameInProgress = false;
    private List<Character> playerCharacters = new List<Character>();
    private MoveUp oil;
    private SpawnObjectOnSpace spawner;
    private AudioSource _audioSource;

    private void Start()
    {
        spawner = FindObjectOfType<SpawnObjectOnSpace>();
    }

    void OnEnable()
    {
        // SceneManager.sceneLoaded += OnSceneLoaded;
        UIManager.OnClicked += OnButtonClicked;
    }

    /*
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        if(scene.name == "Level")
        {
            StartMatch();
        }
    }
    */

    void OnButtonClicked(string buttonName)
    {
        switch (buttonName)
        {
            case "Play":
                // SceneManager.LoadScene("Level");
                StartMatch();
                break;

            case "Continue":
                ResetMatch();
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
            Character newPlayer = Instantiate(characterPrefab, spawnPositions[i], Quaternion.identity).GetComponent<Character>();
            newPlayer.Initialize(i);
            playerCharacters.Add(newPlayer);
        }

        oil = FindObjectOfType<MoveUp>();
        oil.StartMovement();

        spawner.StartSpawning();
        
        _audioSource.Play();

        UIManager.Instance.StartMatch();

        isGameInProgress = true;
    }

    void EndMatch()
    {
        oil.StopMovement();
        spawner.StopSpawning();

        int winner = -1;
        if (playerCharacters.Count > 0)
        {
            winner = playerCharacters[0].PlayerID;
        }
        Debug.Log(winner);
        UIManager.Instance.EndMatch(winner);
        // SceneManager.LoadScene("Menu");
        
        _audioSource.Stop();

        isGameInProgress = false;
    }

    void ResetMatch()
    {
        playerNumber = 0;
        oil.ResetPosition();
        spawner.ClearSpawning();

        for (int i = playerCharacters.Count - 1; i >= 0; i--)
        {
            GameObject playerGO = playerCharacters[i].gameObject;
            playerCharacters.RemoveAt(i);
            Destroy(playerGO);
        }

        UIManager.Instance.Reset();
    }

    public void OnCharacterDied(int playerID)
    {
        if (isGameInProgress)
        {
            for (int i = playerCharacters.Count - 1; i >= 0; i--)
            {
                if (playerCharacters[i].PlayerID != playerID)
                    continue;

                GameObject playerGO = playerCharacters[i].gameObject;
                playerCharacters.RemoveAt(i);
                Destroy(playerGO);
            }

            if (playerCharacters.Count <= 1)
            {
                EndMatch();
            }
        }
    }
}
