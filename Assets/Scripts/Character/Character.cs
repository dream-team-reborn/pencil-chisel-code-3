using UnityEngine;

public class Character : MonoBehaviour
{
    private int _playerID;

    public int PlayerID => _playerID;

    public void Initialize(int playerID)
    {
        _playerID = playerID;
    }

    public void Die()
    {
        GameManager.Instance.OnCharacterDied(_playerID);
    }
}
