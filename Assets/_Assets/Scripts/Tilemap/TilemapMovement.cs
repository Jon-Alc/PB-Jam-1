using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

// https://www.youtube.com/watch?v=YnwOoxtgZQI
public class TilemapMovement : MonoBehaviour
{

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private Tilemap ledgeTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    [SerializeField] private TileData tileData;
    [SerializeField] [Range(1.0f, 10.0f)] private float tileTravelSpeed;

    private float _elapsedTravelTime;
    private Vector3 startingPoint;
    private Vector3 destination;
    public bool CanMove { get; private set; }

    private void Awake()
    {
        Reset();
    }

    private void Reset()
    {
        _elapsedTravelTime = 0f;
        startingPoint = transform.position;
        destination = startingPoint;
        CanMove = true;
    }

    public void Move(Direction directionEnum)
    {
        if (!CanMove) return;
        
        Vector3 directionVector = GetVectorDirectionByEnum(directionEnum);
        Vector3Int destinationTileCoordinates = groundTilemap.WorldToCell(transform.position + directionVector);
        
        if (!ValidMove(directionEnum, destinationTileCoordinates)) return;

        // this does NOT check if the tile in front of the ledge is passable
        // it assumes that the level designer accounts for this
        if (ledgeTilemap.HasTile(destinationTileCoordinates))
        {
            directionVector *= 2;
        }
        
        // transform.position += directionVector;
        destination = transform.position + directionVector;
    }

    void FixedUpdate()
    {
        if (destination != transform.position && _elapsedTravelTime < 1)
        {
            _elapsedTravelTime = Mathf.MoveTowards(_elapsedTravelTime, 1, tileTravelSpeed * Time.fixedDeltaTime);
            transform.position = Vector2.Lerp(startingPoint, destination, _elapsedTravelTime);
            CanMove = false;
        }
        else
        { // it's possible for _elapsedTravelTime to be .99999f although the transform.position == destination
            startingPoint = transform.position;
            _elapsedTravelTime = 0;
            CanMove = true;
        }
        

    }

    private bool ValidMove(Direction playerDirection, Vector3Int destinationTileCoordinates)
    {

        
        if (groundTilemap.HasTile(destinationTileCoordinates) || waterTilemap.HasTile(destinationTileCoordinates))
        {
            return true;
        }
        if (ledgeTilemap.HasTile(destinationTileCoordinates))
        {
            TileBase destinationTile = ledgeTilemap.GetTile(destinationTileCoordinates);
            if ((playerDirection == Direction.Up && tileData.upTiles.Contains(destinationTile)) ||
                (playerDirection == Direction.Down && tileData.downTiles.Contains(destinationTile)) ||
                (playerDirection == Direction.Left && tileData.leftTiles.Contains(destinationTile)) ||
                (playerDirection == Direction.Right && tileData.rightTiles.Contains(destinationTile)))
            {
                return true;
            }
        }
        // tile is either a collision or doesn't exist
        return false;
    }

    private Vector3 GetVectorDirectionByEnum(Direction directionEnum)
    {
        // make sure that direction values are either 0 or 1 to avoid moving between tiles
        if (directionEnum == Direction.Up)
            return Vector3.up;
        if (directionEnum == Direction.Down)
            return Vector3.down;
        if (directionEnum == Direction.Left)
            return Vector3.left;
        return Vector3.right;
    }
}
