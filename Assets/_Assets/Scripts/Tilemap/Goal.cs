using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    private bool _playerInFrontOfCave;

    void Awake()
    {
        _playerInFrontOfCave = false;
    }

    void OnEnable()
    {
        PlayerMovement.PlayerCharacterMove += OnPlayerCharacterMove;
    }

    void OnDisable()
    {
        PlayerMovement.PlayerCharacterMove -= OnPlayerCharacterMove;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInFrontOfCave = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInFrontOfCave = false;
        }
    }

    private void OnPlayerCharacterMove(Direction facingDirection)
    {
        if (_playerInFrontOfCave && facingDirection == Direction.Up)
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextIndex);
        }
    }
}
