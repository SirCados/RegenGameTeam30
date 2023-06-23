using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    public enum TypeOfSeed
    {
        NONE,
        COVER,
        FEED,
        HEDGEROW,
        MONSTER
    }

    [SerializeField] Tilemap _worldMap;
    [SerializeField] Tilemap _selectorMap;
    [SerializeField] Tilemap _plantMap;
    [SerializeField] Tilemap _corruptionMap;
    [SerializeField] TileBase _badlandBase;
    [SerializeField] TileBase _badlandInfested1;
    [SerializeField] TileBase _badlandInfested2;
    [SerializeField] TileBase _midlandBase;
    [SerializeField] TileBase _healthySoilBase;
    [SerializeField] TileBase _healthySoilRooty;
    [SerializeField] TileBase _cropTile;
    [SerializeField] TileBase _selectorTile;
    [SerializeField] Camera _camera;

    [SerializeField] TileBase _coverCrop;
    [SerializeField] TileBase _feedCrop;
    [SerializeField] TileBase _hedgerow;
    [SerializeField] TileBase _monsterPlant;

    bool _isSelecting = false;
    TypeOfSeed seedToPlant;
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
        GameObject player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerClick();

        GameObject player = GameObject.Find("player");

        //TempSeedSelector();
    }

    void TempSeedSelector()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            print("C");
            seedToPlant = TypeOfSeed.COVER;
            GetMousePosition();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            print("H");
            seedToPlant = TypeOfSeed.HEDGEROW;
            GetMousePosition();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            print("F");
            seedToPlant = TypeOfSeed.FEED;
            GetMousePosition();
        }

        if (_isSelecting)
        {
            FindSelectionCorners(_firstClick, GetCurrentMousePosition()); ;
        }
    }

    void GetMousePosition()
    {
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

    void PlayerClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
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
        seedToPlant = TypeOfSeed.NONE;
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
            //print(gridPosition);
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
                //PlantSeed(positionOfTheChange, seedToPlant);
            }
        }
    }    

    void PlantSeed(Vector3Int positionOfTheChange, TypeOfSeed seed)
    {
        Tilemap targetMap = _selectorMap;
        bool isTileGettingPlant = false;

        if (!_isSelecting)
        {
            targetMap = _worldMap;
        }

        TileBase tileAtPosition = targetMap.GetTile(positionOfTheChange);
        TileBase seedToPlant = _selectorTile;

        if (!_isSelecting && tileAtPosition == _firstSelectedTile)
        {
            switch (seed)
            {
                case TypeOfSeed.COVER:
                    if (tileAtPosition == _badlandBase || tileAtPosition == _badlandInfested1 || tileAtPosition == _badlandInfested2 || tileAtPosition == _midlandBase || tileAtPosition == _healthySoilBase)
                    {
                        seedToPlant = _coverCrop;
                    }
                    break;
                case TypeOfSeed.HEDGEROW:
                    if (tileAtPosition == _midlandBase || tileAtPosition == _healthySoilBase)
                    {
                        seedToPlant = _hedgerow;
                    }
                    break;
                case TypeOfSeed.FEED:
                    if (tileAtPosition == _healthySoilBase)
                    {
                        seedToPlant = _feedCrop;
                    }
                    break;
                case TypeOfSeed.MONSTER:
                    if (tileAtPosition == _badlandBase || tileAtPosition == _badlandInfested1 || tileAtPosition == _badlandInfested2)
                    {
                        seedToPlant = _feedCrop;
                    }
                    break;
            }
        }

        if (_isSelecting || (!_isSelecting && tileAtPosition == _firstSelectedTile))
        {
            if (isTileGettingPlant)
            {
                ChangeTile(_plantMap, positionOfTheChange, _cropTile);
            }            
            else
            {
                ResetSelector();
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
