using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected Tilemap entityTilemap;
    [Tooltip("Up = 0, Down = 1, Left = 2, Right = 3")]
    [SerializeField] protected List<Sprite> directionalSprites;

    public List<Sprite> DirectionalSprites => directionalSprites;

    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected Vector3 _tileOffset;
    
    void Awake()
    {
        SetupOnAwake();
    }

    protected virtual void SetupOnAwake()
    {
        _tileOffset = entityTilemap.tileAnchor;
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
        Vector2 tileCoordinates = transform.position - _tileOffset;
        (int, int) coordinateTuple = ((int)tileCoordinates.x, (int)tileCoordinates.y);
        foreach (var tuple in affectedCoordinates)
        {
            Debug.Log(tuple);
        }
        if (affectedCoordinates.Contains(coordinateTuple))
        {
            Die();
        }
    }
    
    public abstract void Die();
}
