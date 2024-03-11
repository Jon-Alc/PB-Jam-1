using System;
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
    private BoundsInt _eligibleSpawnBounds;
    private Vector3 _tileOffset;

    private Dictionary<(int, int), List<(int, int)>> _waterBodies;

    public static event Action<List<(int, int)>> LightningHitsWaterEvent;

    void Awake()
    {
        Reset();
    }

    void OnEnable()
    {
        Lightning.LightningStrikeEvent += OnLightningStrike;
    }

    void OnDisable()
    {
        Lightning.LightningStrikeEvent -= OnLightningStrike;
    }

    void Reset()
    {
        _lightningTimer = 0f;
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
        // get bounds of water tilemap
        // iterate through those bounds for water cells
        // add water cells to Set
        
        BoundsInt waterBounds = waterTilemap.cellBounds;
        List<(int, int)> unexploredWaterTiles = new List<(int, int)>();
        Vector3Int currentCoordinates;
        
        for (int x = waterBounds.xMin; x < waterBounds.xMax; x++)
        {
            for (int y = waterBounds.yMin; y < waterBounds.yMax; y++)
            {
                currentCoordinates = new Vector3Int(x, y, 0);
                if (waterTilemap.GetTile(currentCoordinates) != null)
                {
                    unexploredWaterTiles.Add((x, y));
                }
            }
        }
        
        // Iterate through unexplored water tiles list
        // Make new list for adjacency
        // Figure out adjacency via DFS
        // If adjacent water tile found, remove from unexploredList and add to adjacencyList
        // Add list to Dictionary of waterBodies
        
        _waterBodies = new Dictionary<(int, int), List<(int, int)>>();

        while (unexploredWaterTiles.Count > 0)
        {
            List<(int, int)> adjacencyList = new List<(int, int)>();
            WaterAdjacencyFinder(unexploredWaterTiles, adjacencyList, unexploredWaterTiles[0]);
        }

    }

    private void WaterAdjacencyFinder
        (
            List<(int, int)> unexploredList,
            List<(int, int)> adjacencyList,
            (int, int) currentTileCoordinates
        )
    {
        // exit early if a tile either doesn't exist or has been explored already
        if (!unexploredList.Contains(currentTileCoordinates))
        {
            return;
        }

        unexploredList.Remove(currentTileCoordinates);
        adjacencyList.Add(currentTileCoordinates);
        _waterBodies[currentTileCoordinates] = adjacencyList;
        
        // repeat the search for all adjacent tiles
        WaterAdjacencyFinder(unexploredList, adjacencyList, (currentTileCoordinates.Item1 - 1, currentTileCoordinates.Item2));
        WaterAdjacencyFinder(unexploredList, adjacencyList, (currentTileCoordinates.Item1 + 1, currentTileCoordinates.Item2));
        WaterAdjacencyFinder(unexploredList, adjacencyList, (currentTileCoordinates.Item1, currentTileCoordinates.Item2 - 1));
        WaterAdjacencyFinder(unexploredList, adjacencyList, (currentTileCoordinates.Item1, currentTileCoordinates.Item2 + 1));
        
    }

    private Vector3Int PickLocationAtRandomToStrike()
    {
        int randomX = UnityEngine.Random.Range(_eligibleSpawnBounds.xMin, _eligibleSpawnBounds.xMax);
        int randomY = UnityEngine.Random.Range(_eligibleSpawnBounds.yMin, _eligibleSpawnBounds.yMax);

        Vector3Int randomCoordinates = new Vector3Int(randomX, randomY, 0);

        // reroll coordinates if no eligible tile was hit
        if (waterTilemap.GetTile(randomCoordinates) == null && groundTilemap.GetTile(randomCoordinates) == null)
        {
            return PickLocationAtRandomToStrike();
        }

        return randomCoordinates;
    }

    void OnLightningStrike(Vector3 position)
    {
        Vector3 tileCoordinates = position - _tileOffset;
        (int, int) keyTile = ((int) tileCoordinates.x, (int) tileCoordinates.y);
        if (_waterBodies.TryGetValue(keyTile, out var waterBody))
        {
            LightningHitsWaterEvent?.Invoke(waterBody);
        }
    }
    
}
