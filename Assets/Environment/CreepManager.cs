using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreepManager : MonoBehaviour
{
    [SerializeField] Tilemap _worldMap;
    [SerializeField] Tilemap _corruptionMap;
    [SerializeField] TileBase _badlandBase;
    [SerializeField] TileBase _corruptionOverlay;
    [SerializeField] float _creepTimer = 0;

    bool _isSpreadConverting = false;

    // Start is called before the first frame update
    void Start()
    {
        if(_creepTimer < 1f)
        {
            _creepTimer = 5f;
        }
        InvokeRepeating("GetTilesToBeCorrupted", 1f, _creepTimer);        
    }

    void GetTilesToBeCorrupted()
    {
        _worldMap.CompressBounds();
        
        int localMinimumX = (int)_worldMap.localBounds.min.x;
        int localMinimumY = (int)_worldMap.localBounds.min.y;
        int localMaxiumuX = (int)_worldMap.localBounds.max.x;
        int localMaximumY = (int)_worldMap.localBounds.max.y;

        Vector3Int minimumBounds = new Vector3Int(localMinimumX, localMinimumY, 0);
        Vector3Int maximumBounds = new Vector3Int(localMaxiumuX, localMaximumY, 0);

        FindSelectionCorners(minimumBounds, maximumBounds);
        _isSpreadConverting = !_isSpreadConverting;
    }

    void FindSelectionCorners(Vector3Int firstPoint, Vector3Int secondPoint)
    {
        int leftMostX = (firstPoint.x > secondPoint.x) ? firstPoint.x : secondPoint.x;
        int topMostY = (firstPoint.y > secondPoint.y) ? firstPoint.y : secondPoint.y;
        Vector3Int startPoint = new Vector3Int(leftMostX, topMostY, 0);

        int rightMostX = (firstPoint.x < secondPoint.x) ? firstPoint.x : secondPoint.x;
        int bottomMostY = (firstPoint.y < secondPoint.y) ? firstPoint.y : secondPoint.y;
        Vector3Int endPoint = new Vector3Int(rightMostX, bottomMostY, 0);

        int height = Mathf.Abs(topMostY - bottomMostY) + 1;
        int width = Mathf.Abs(leftMostX - rightMostX) + 1;

        GetTilesWithinSelection(startPoint, endPoint, height, width);
    }

    void GetTilesWithinSelection(Vector3Int startPoint, Vector3Int endPoint, int height, int width)
    {
        Vector3Int positionOfTheChange;

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                positionOfTheChange = new Vector3Int(startPoint.x - column, startPoint.y - row, 0);

                TileBase tileAtPosition = _worldMap.GetTile(positionOfTheChange);
                if (tileAtPosition != null && tileAtPosition == _badlandBase)
                {
                    GetNeighborsOfTile(positionOfTheChange);
                }
            }
        }
    }
    void GetNeighborsOfTile(Vector3Int positionOfTheChange)
    {
        Vector3Int[] tileAndNeighbors = new Vector3Int[4];

        tileAndNeighbors[0] = new Vector3Int(positionOfTheChange.x - 1, positionOfTheChange.y, 0);
        tileAndNeighbors[1] = new Vector3Int(positionOfTheChange.x, positionOfTheChange.y -1, 0);
        tileAndNeighbors[2] = new Vector3Int(positionOfTheChange.x + 1, positionOfTheChange.y, 0);
        tileAndNeighbors[3] = new Vector3Int(positionOfTheChange.x, positionOfTheChange.y + 1, 0);

        foreach(Vector3Int tile in tileAndNeighbors)
        {
            CorruptTile(tile);
        }
    }

    void CorruptTile(Vector3Int positionOfTheChange)
    {
        TileBase tileAtPosition = _worldMap.GetTile(positionOfTheChange);
        if (tileAtPosition != null && tileAtPosition != _badlandBase)
        {
            TileBase corruptedTile = _corruptionMap.GetTile(positionOfTheChange);
            if (_isSpreadConverting && corruptedTile != null && corruptedTile == _corruptionOverlay && tileAtPosition != _badlandBase)
            {
                _corruptionMap.SetTile(positionOfTheChange, null);
                _worldMap.SetTile(positionOfTheChange, _badlandBase);
            }
            else if(!_isSpreadConverting)
            {
                _corruptionMap.SetTile(positionOfTheChange, _corruptionOverlay);
            }
        }
    }
}
