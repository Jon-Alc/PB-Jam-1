using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    
    private void OnEnable()
    {
        LightningSpawner.LightningHitsWaterEvent += OnLightningHitsWater;
    }

    private void OnDisable()
    {
        LightningSpawner.LightningHitsWaterEvent -= OnLightningHitsWater;
    }
    
    void OnLightningHitsWater(List<(int, int)> affectedCoordinates)
    {
        Vector2 tileCoordinates = transform.position - _tileOffset;
        (int, int) coordinateTuple = ((int)tileCoordinates.x, (int)tileCoordinates.y);
        if (affectedCoordinates.Contains(coordinateTuple))
        {
            Die();
        }
    }
    
    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
