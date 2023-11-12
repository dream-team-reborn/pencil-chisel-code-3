using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _meshRenderers;
    
    private const string CharacterColor = "_CharacterColor";
    private int _playerID;
    private Color _playerColor;

    public int PlayerID => _playerID;

    public void Initialize(int playerID, Color color)
    {
        _playerID = playerID;
        _playerColor = color;
        
        foreach (var meshRenderer in _meshRenderers)
        {
            meshRenderer.material.SetColor(CharacterColor, _playerColor);
        }
    }

    public void Die()
    {
        GameManager.Instance.OnCharacterDied(_playerID);
    }
}