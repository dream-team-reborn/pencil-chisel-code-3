using System.Collections;
using CharacterMovements;
using UnityEngine;
using System.Collections.Generic;

public class DamageDetector : MonoBehaviour
{
    [SerializeField] private byte _maxDamageAmount;
    [SerializeField] private float _damageImmunitySeconds;
    
    private Character _character;
    private byte _damageCounter;
    private bool _canBeDamaged;
    private AudioSource[] _damageSounds;

    private void Start()
    {
        _character = GetComponent<Character>();
        _damageSounds = GetComponentsInChildren<AudioSource>();

        _canBeDamaged = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var surface = other.GetComponent<ISurface>();
        if (surface == null)
            return;

        if (surface.GetSurfaceType() != SurfaceType.Oil)
            return;
        
        HandleOilCollision();
    }

    private void HandleOilCollision()
    {
        if (!_canBeDamaged)
            return;
        
        _damageCounter++;
        _damageSounds[Random.Range(0, _damageSounds.Length)].Play();
        
        if (_damageCounter >= _maxDamageAmount)
        {
            _character.Die();
            return;
        }

        StartCoroutine(StartDamageImmunity());
    }

    private IEnumerator StartDamageImmunity()
    {
        _canBeDamaged = false;
        yield return new WaitForSeconds(_damageImmunitySeconds);
        _canBeDamaged = true;
    }
}
