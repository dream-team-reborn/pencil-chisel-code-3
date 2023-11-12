using System;
using System.Collections;
using System.Collections.Generic;
using CharacterMovements;
using UnityEngine;

public class IngredientAudio : MonoBehaviour
{
    private bool _splashSoundPlayed = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleSurfaceEnter(other.gameObject);
    }

    private void HandleSurfaceEnter(GameObject gameObj)
    {
        var surface = gameObj.GetComponent<ISurface>();
        if (surface == null) return;

        switch (surface.GetSurfaceType())
        {
            case SurfaceType.Oil:
                PlaySound();
                break;

            default:
                return;
        }
    }

    private void PlaySound()
    {
        if (_audioSource == null || _splashSoundPlayed) return;
        
        _audioSource.Play();
        _splashSoundPlayed = true;
    }
}
