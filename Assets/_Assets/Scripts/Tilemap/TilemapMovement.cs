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
    [FormerlySerializedAs("tileDirectionData")] [SerializeField] private TileData tileData;

    public void Move(Vector2 direction)
    {
        Direction directionEnum = DetermineMoveDirection(direction);
        Vector3 directionVector = GetVectorDirectionByEnum(directionEnum);
        Vector3Int destinationTileCoordinates = groundTilemap.WorldToCell(transform.position + directionVector);
        
        if (!ValidMove(directionEnum, destinationTileCoordinates)) return;

        // this does NOT check if the tile in front of the ledge is passable
        // it assumes that the level designer accounts for this
        if (ledgeTilemap.HasTile(destinationTileCoordinates))
        {
            directionVector *= 2;
        }
        
        transform.position += directionVector;
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

    private Direction DetermineMoveDirection(Vector2 direction)
    {
        Direction moveDirection = Direction.Up;
        if (direction.x > 0) { moveDirection = Direction.Right; }
        else if (direction.x < 0) { moveDirection = Direction.Left; }
        else if (direction.y < 0) { moveDirection = Direction.Down; }

        return moveDirection;
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
