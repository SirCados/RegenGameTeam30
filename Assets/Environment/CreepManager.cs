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

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GetTilesToBeCorrupted", 5, 10);
        GetTilesToBeCorrupted();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetTilesToBeCorrupted()
    {
        _worldMap.CompressBounds();
        
        print("minbounds: " + _worldMap.localBounds.min);
        print("maxbounds: " + _worldMap.localBounds.max);

        int localMinimumX = (int)_worldMap.localBounds.min.x;
        int localMinimumY = (int)_worldMap.localBounds.min.y;
        int localMaxiumuX = (int)_worldMap.localBounds.max.x;
        int localMaximumY = (int)_worldMap.localBounds.max.y;

        Vector3Int minimumBounds = new Vector3Int(localMinimumX, localMinimumY, 0);
        Vector3Int maximumBounds = new Vector3Int(localMaxiumuX, localMaximumY, 0);

        FindSelectionCorners(minimumBounds, maximumBounds);
    }

    void GetNeighborsOfTile()
    {

    }

    void CorruptTile()
    {
        //check if tile already has corruption or if it exists in array more than once
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

                if (tileAtPosition != null && tileAtPosition != _badlandBase)
                {
                    print("corrupt this: " + tileAtPosition.name);
                    TileBase corruptedTile = _corruptionMap.GetTile(positionOfTheChange);
                    if(corruptedTile != null && corruptedTile == _corruptionOverlay)
                    {
                        _corruptionMap.DeleteCells(positionOfTheChange, new Vector3Int(1, 1, 0));
                        _worldMap.SetTile(positionOfTheChange, _badlandBase);
                    }
                    else
                    {
                        _corruptionMap.SetTile(positionOfTheChange, _corruptionOverlay);
                    }
                }
            }
        }
    }

}
