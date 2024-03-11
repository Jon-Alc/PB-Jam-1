using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected Tilemap entityTilemap;

    protected Vector3 _tileOffset;
    
    void Awake()
    {
        SetupOnAwake();
    }

    protected virtual void SetupOnAwake()
    {
        _tileOffset = entityTilemap.tileAnchor;
        Debug.Log($"{gameObject.name}");
        Debug.Log(_tileOffset);
    }
    
    void OnEnable()
    {
        LightningSpawner.LightningHitsWaterEvent += OnLightningHitsWater;
    }

    void OnDisable()
    {
        LightningSpawner.LightningHitsWaterEvent -= OnLightningHitsWater;
    }
    
    void OnLightningHitsWater(List<(int, int)> affectedCoordinates)
    {
        // Debug.Log("OnLightningHitsWater called");
        Vector2 tileCoordinates = transform.position - _tileOffset;
        (int, int) coordinateTuple = ((int)tileCoordinates.x, (int)tileCoordinates.y);
        // Debug.Log($"{gameObject.name}, transform: {transform.position}");
        // Debug.Log($"{gameObject.name}, adjusted: {coordinateTuple}");
        foreach (var tuple in affectedCoordinates)
        {
            Debug.Log(tuple);
        }
        if (affectedCoordinates.Contains(coordinateTuple))
        {
            Debug.Log("Bear should be dead");
            Die();
        }
    }
    
    public abstract void Die();
}
