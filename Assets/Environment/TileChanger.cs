using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    [SerializeField] Tilemap _tileMap;
    [SerializeField] TileBase _badlandBase;
    [SerializeField] TileBase _midlandBase;
    [SerializeField] TileBase _healthySoilBase;
    [SerializeField] Camera _camera;

    Vector3Int _firstClick;
    Vector3Int _secondClick;
    //since (0,0,0) is in the playspace, I set a max int point to be the "empty space".
    //this shouldn't come up, but it is in for error handling/edge cases
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
                FindSelectionCorners();
                ResetStoredPositions();
            }
        }
    }

    void ResetStoredPositions()
    {
        _firstClick = _emptyCell;
        _secondClick = _emptyCell;
    }

    Vector3Int CaptureMousePosition()
    {
        var screenPosition = Input.mousePosition;

        if (screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > Screen.width || screenPosition.y > Screen.height)
        {
            return _emptyCell;
        }

        var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
        var gridPosition = _tileMap.WorldToCell(worldPosition);

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
        Vector3Int startPoint = new Vector3Int(leftMostX, topMostY, 0);

        int rightMostX = (_firstClick.x < _secondClick.x) ? _firstClick.x : _secondClick.x;
        int bottomMostY = (_firstClick.y < _secondClick.y) ? _firstClick.y : _secondClick.y;        
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
                
                print("Tile to Change: " + positionOfTheChange);
                DetermineNewTile(positionOfTheChange);
            }
        }
    }
    void DetermineNewTile(Vector3Int positionOfTheChange)
    {
        TileBase tileAtPosition = _tileMap.GetTile(positionOfTheChange);

        if (tileAtPosition.name == _badlandBase.name)
        {
            ChangeTile(positionOfTheChange, _midlandBase);
        }
        else if (tileAtPosition.name == _midlandBase.name)
        {
            ChangeTile(positionOfTheChange, _healthySoilBase);
        }
    }

    void ChangeTile(Vector3Int positionOfTheChange, TileBase tileToChangeTo)
    {
        _tileMap.SetTile(positionOfTheChange, tileToChangeTo);
    }
}

/*
 Using code from this forum post: https://forum.unity.com/threads/change-tiles-in-a-grid-system.1398958/
*/
