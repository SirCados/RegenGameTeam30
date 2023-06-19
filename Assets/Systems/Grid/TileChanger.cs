using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    [SerializeField] Tilemap _tileMap;
    [SerializeField] Tile _orangeTile;
    [SerializeField] Camera _camera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CaptureMousePosition();
        }
    }

    void ChangeTile(Vector3Int position)
    {
        _tileMap.SetTile(position, _orangeTile);
    }

    Vector3 CaptureMousePosition()
    {
        //get the mouse position
        var screenPosition = Input.mousePosition;
        //test to ensure that it's within the visible area
        if (screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > Screen.width || screenPosition.y > Screen.height)
            return new Vector3();

        //get the tilemap grid position
        var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
        var gridPosition = _tileMap.WorldToCell(worldPosition);

        //is there a tile there?
        var tile = _tileMap.GetTile(gridPosition);
        if (tile != null)
        {
            ChangeTile(gridPosition);
        }
        else
        {
            Debug.Log($"No tile at {gridPosition} ");
        }

        return new Vector3();
    }
}

/*
 Using code from this forum post: https://forum.unity.com/threads/change-tiles-in-a-grid-system.1398958/
*/
