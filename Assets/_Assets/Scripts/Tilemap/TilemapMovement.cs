using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

// https://www.youtube.com/watch?v=YnwOoxtgZQI
public class TilemapMovement : MonoBehaviour
{

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private Tilemap ledgeTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    public void Move(Vector2 direction)
    {
        Direction directionEnum = DetermineMoveDirection(direction);
        Vector3 directionVector = GetVectorDirectionByEnum(directionEnum);
        
        if (!ValidMove(directionVector, directionEnum)) return;
        
        transform.position += directionVector;
    }

    private bool ValidMove(Vector3 direction, Direction playerDirection)
    {
        Vector3Int tileDestination = groundTilemap.WorldToCell(transform.position + direction);
        
        if (groundTilemap.HasTile(tileDestination) || waterTilemap.HasTile(tileDestination))
        {
            return true;
        }
        if (ledgeTilemap.HasTile(tileDestination))
        {
            Direction ledgeDirection = ledgeTilemap.GetTile(tileDestination).GetComponent<Ledge>().LedgeJumpDirection;
            return playerDirection == ledgeDirection;
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
