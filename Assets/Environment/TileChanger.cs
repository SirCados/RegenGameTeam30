using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    [SerializeField] Tilemap _worldMap;
    [SerializeField] Tilemap _selectorMap;
    [SerializeField] Tilemap _plantMap;
    [SerializeField] Tilemap _corruptionMap;
    [SerializeField] TileBase _badlandBase;
    [SerializeField] TileBase _midlandBase;
    [SerializeField] TileBase _healthySoilBase;
    [SerializeField] TileBase _healthySoilRooty;
    [SerializeField] TileBase _cropTile;
    [SerializeField] TileBase _selectorTile;
    [SerializeField] Camera _camera;

    bool _isSelecting = false;
    Vector3Int _firstClick;
    Vector3Int _secondClick;
    Vector3Int _currentMousePosition;

    TileBase _firstSelectedTile;

    //since (0,0,0) is in the playspace, I set a max int point to be the "empty space".
    //this shouldn't come up, but it is in for error handling/edge cases
    Vector3Int _emptyCell = new Vector3Int(999999999, 999999999, 999999999);

    private void Awake()
    {
        _isSelecting = false;
        _firstSelectedTile = _selectorTile;
    }

    private void Start()
    {
        ResetSelector();        
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
            //What is being planted? I need to know here before anything else
            if (_firstClick == _emptyCell)
            {
                _firstClick = CaptureMousePosition();
                SetFirstSelectedTile();
                _isSelecting = true;
            }
            else if (_firstClick != _emptyCell && _secondClick == _emptyCell)
            {
                _secondClick = CaptureMousePosition();
                _isSelecting = false;
            }
            
            if (_firstClick != _emptyCell && _secondClick != _emptyCell)
            {
                FindSelectionCorners(_firstClick, _secondClick);
                ResetSelector();
            }
        }

        if (_isSelecting)
        {
            FindSelectionCorners(_firstClick, GetCurrentMousePosition()); ;
        }
    }

    void ResetSelector()
    {
        _firstClick = _emptyCell;
        _secondClick = _emptyCell;
        _selectorMap.ClearAllTiles();
        _isSelecting = false;
    }

    Vector3Int GetCurrentMousePosition()
    {
        if (_isSelecting)
        {
            Vector3Int mouseIsAt = CaptureMousePosition();
            if(mouseIsAt != _currentMousePosition)
            {
                _selectorMap.ClearAllTiles();
                _currentMousePosition = CaptureMousePosition();                
            }
        }
        return _currentMousePosition;
    }

    Vector3Int CaptureMousePosition()
    {
        var screenPosition = Input.mousePosition;

        if (screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > Screen.width || screenPosition.y > Screen.height)
        {
            return _emptyCell;
        }

        var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
        var gridPosition = _worldMap.WorldToCell(worldPosition);

        var tile = _worldMap.GetTile(gridPosition);
        if (tile != null)
        {
            print(gridPosition);
            return gridPosition;
        }
        else
        {
            Debug.Log($"No tile at {gridPosition} ");
            return _emptyCell;
        }
    }

    void SetFirstSelectedTile()
    {
        _firstSelectedTile = _worldMap.GetTile(_firstClick);
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
                DetermineNewTile(positionOfTheChange);
            }
        }
    }

    void DetermineNewTile(Vector3Int positionOfTheChange)
    {
        Tilemap targetMap = _selectorMap;
        bool isTileGettingPlant = false;

        if (!_isSelecting)
        {
            targetMap = _worldMap;
        }

        TileBase tileAtPosition = targetMap.GetTile(positionOfTheChange);
        TileBase tileToPlace = _selectorTile;

        if(!_isSelecting && tileAtPosition == _firstSelectedTile)
        {
            if (tileAtPosition == _badlandBase)
            {
                tileToPlace = _midlandBase;
            }
            else if (tileAtPosition == _midlandBase)
            {
                tileToPlace = _healthySoilBase;
            }
            else if(tileAtPosition == _healthySoilBase)
            {
                tileToPlace = _healthySoilRooty;
                isTileGettingPlant = true;
            }
        }  
        
        if(_isSelecting || (!_isSelecting && tileAtPosition == _firstSelectedTile))
        {
            if(_firstSelectedTile != _healthySoilRooty)
            {                
                ChangeTile(targetMap, positionOfTheChange, tileToPlace);
                if (isTileGettingPlant)
                {
                    ChangeTile(_plantMap, positionOfTheChange, _cropTile);
                }
            } 
            else
            {
                ResetSelector();
            }
        }
    }

    void ChangeTile(Tilemap targetMap, Vector3Int positionOfTheChange, TileBase tileToChangeTo)
    {
        targetMap.SetTile(positionOfTheChange, tileToChangeTo);
    }

    void AddCropToTile(Vector3Int positionOfTheChange, TileBase plantToAdd)
    {

    }
}

/*
 Using code from this forum post: https://forum.unity.com/threads/change-tiles-in-a-grid-system.1398958/
*/
