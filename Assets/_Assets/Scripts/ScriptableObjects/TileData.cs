using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] upTiles;
    public TileBase[] downTiles;
    public TileBase[] leftTiles;
    public TileBase[] rightTiles;

    public TileBase[] waterTiles;
    public TileBase[] mudTiles;
}
