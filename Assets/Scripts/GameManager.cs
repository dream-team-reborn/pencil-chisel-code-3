using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int playerNumber = 0;
    [SerializeField]
    private GameObject characterPrefab;
    [SerializeField]
    private Vector3[] spawnPositions;
    [SerializeField]
    private GameObject oil;

    private List<GameObject> playerCharacters = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if(playerNumber > 4)
        {
            playerNumber = 4;
        }
        else if (playerNumber < 1)
        {
            playerNumber = 1;
        }
        
        for(int i = 0; i < playerNumber; i++)
        {
            playerCharacters.Add(Instantiate(characterPrefab, spawnPositions[i], Quaternion.identity));
        }

        oil.GetComponent<MoveUp>().StartMovement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
