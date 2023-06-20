using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    [SerializeField] Tilemap _tileMap;
    [SerializeField] Tile _orangeTile;
    [SerializeField] Camera _camera;

    Vector3Int _firstClick;
    Vector3Int _secondClick;
    Vector3Int _emptyCell = new Vector3Int(999999999, 999999999, 999999999);

    private void Start()
    {
        ResetStoredPositions();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerClick();
    }

    void PlayerClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_firstClick == _emptyCell)
            {
                _firstClick = CaptureMousePosition();
                print(_firstClick);
            }
            else if (_firstClick != _emptyCell && _secondClick == _emptyCell)
            {
                _secondClick = CaptureMousePosition();
                print(_secondClick);
            }

            if (_firstClick != _emptyCell && _secondClick != _emptyCell)
            {
               // print("Changing!");
                FindSelectionCorners();
                ResetStoredPositions();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ChangeTile(CaptureMousePosition());
        }
    }

    void ChangeTile(Vector3Int position)
    {
        _tileMap.SetTile(position, _orangeTile);
    }

    Vector3Int CaptureMousePosition()
    {
        //get the mouse position
        var screenPosition = Input.mousePosition;
        //test to ensure that it's within the visible area
        if (screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > Screen.width || screenPosition.y > Screen.height)
        {
            return _emptyCell;
        }

        //get the tilemap grid position
        var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
        var gridPosition = _tileMap.WorldToCell(worldPosition);

        //is there a tile there?
        var tile = _tileMap.GetTile(gridPosition);
        if (tile != null)
        {
            return gridPosition;
        }
        else
        {
            Debug.Log($"No tile at {gridPosition} ");
            return _emptyCell;
        }
    }

    void FindSelectionCorners()
    {
        

        int leftMostX = (_firstClick.x > _secondClick.x) ? _firstClick.x : _secondClick.x;
        int topMostY = (_firstClick.y > _secondClick.y) ? _firstClick.y : _secondClick.y;

        //print("Start point: (" + leftMostX + ", " + topMostY + ")");

        int rightMostX = (_firstClick.x < _secondClick.x) ? _firstClick.x : _secondClick.x;
        int bottomMostY = (_firstClick.y < _secondClick.y) ? _firstClick.y : _secondClick.y;

        //print("End point: (" + rightMostX + ", " + bottomMostY + ")");

        Vector3Int startPoint = new Vector3Int(leftMostX, topMostY, 0);
        Vector3Int endPoint = new Vector3Int(rightMostX, bottomMostY, 0);

        int height = Mathf.Abs(topMostY - bottomMostY) + 1;
        int width = Mathf.Abs(leftMostX - rightMostX) + 1;

        //print("The rectangle is " + height + " tall and " + width + " wide");

        GetTilesWithinSelection(startPoint, endPoint, height, width);
    }

    void GetTilesWithinSelection(Vector3Int startPoint, Vector3Int endPoint, int height, int width)
    {
        Tile[] tileArray;
        Vector3Int tileToChange;

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                tileToChange = new Vector3Int(startPoint.x - column, startPoint.y - row, 0);
                print("Tile to Change: " + tileToChange);
                ChangeTile(tileToChange);
            }
        }        
    }

    void ResetStoredPositions()
    {
        _firstClick = _emptyCell;
        _secondClick = _emptyCell;
        //print("Reset!");
    }
}

/*
 Using code from this forum post: https://forum.unity.com/threads/change-tiles-in-a-grid-system.1398958/
*/
