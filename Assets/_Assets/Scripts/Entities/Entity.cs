using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected Tilemap entityTilemap;

    protected Vector3 _tileOffset;
    
    void Awake()
    {
        _tileOffset = entityTilemap.tileAnchor;
    }
    
    public abstract void Die();
}
