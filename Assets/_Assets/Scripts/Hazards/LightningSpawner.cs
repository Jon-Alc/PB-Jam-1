using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightningSpawner : MonoBehaviour
{
    [SerializeField] private GameObject lightningWarning;
    [SerializeField] [Range(1f, 10f)] private float lightningSpawnDelay;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap waterTilemap;

    private float _lightningTimer;
    private float _warningTimer;
    private bool _warningSpawned;
    private BoundsInt _eligibleSpawnBounds;
    private Vector3 _tileOffset;

    private Dictionary<(int, int), bool> _waterMap;

    void Awake()
    {
        Reset();
    }

    void Reset()
    {
        _lightningTimer = 0f;
        _warningTimer = 0f;
        _warningSpawned = false;
        _tileOffset = groundTilemap.tileAnchor;

        GetEligibleBounds();
        BuildWaterMap();
    }

    void Update()
    {
        _lightningTimer += Time.deltaTime;

        if (_lightningTimer >= lightningSpawnDelay)
        {
            _lightningTimer = 0f;
            Vector3 lightningCoordinates = PickLocationAtRandomToStrike();
            lightningCoordinates += _tileOffset;
            Instantiate(lightningWarning, lightningCoordinates, Quaternion.identity);
        }
    }

    void GetEligibleBounds()
    {
        BoundsInt groundBounds = groundTilemap.cellBounds;
        BoundsInt waterBounds = waterTilemap.cellBounds;

        _eligibleSpawnBounds.xMin = Mathf.Min(groundBounds.xMin, waterBounds.xMin);
        _eligibleSpawnBounds.xMax = Mathf.Max(groundBounds.xMax, waterBounds.xMax);
        _eligibleSpawnBounds.yMin = Mathf.Min(groundBounds.yMin, waterBounds.yMax);
        _eligibleSpawnBounds.yMax = Mathf.Max(groundBounds.yMax, waterBounds.yMax);
    }

    void BuildWaterMap()
    {
        BoundsInt waterBounds = waterTilemap.cellBounds;

        _waterMap = new Dictionary<(int, int), bool>();

        Vector3Int currentCoordinates;
        for (int x = waterBounds.xMin; x < waterBounds.xMax; x++)
        {
            for (int y = waterBounds.yMin; y < waterBounds.yMax; y++)
            {
                currentCoordinates = new Vector3Int(x, y, 0);
                if (waterTilemap.GetTile(currentCoordinates) != null)
                {
                    _waterMap[(x, y)] = false;
                }
            }
        }
    }

    private Vector3Int PickLocationAtRandomToStrike()
    {
        int randomX = Random.Range(_eligibleSpawnBounds.xMin, _eligibleSpawnBounds.xMax);
        int randomY = Random.Range(_eligibleSpawnBounds.yMin, _eligibleSpawnBounds.yMax);

        Vector3Int randomCoordinates = new Vector3Int(randomX, randomY, 0);

        // reroll coordinates if no eligible tile was hit
        if (waterTilemap.GetTile(randomCoordinates) == null && groundTilemap.GetTile(randomCoordinates) == null)
        {
            return PickLocationAtRandomToStrike();
        }

        return randomCoordinates;
    }
    
}
