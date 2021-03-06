using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Farm.Buildings;
using UnityEngine;
using System;

namespace Farm.Grid
{
    public class GridBuildingManager : MonoSingleton<GridBuildingManager>
    {
        public enum TileType { Empty, Dirt, Grass, Asphalt, OnBuilding, Green, Red }

        [SerializeField] public GridLayout gridLayout;
        [SerializeField] private Tilemap mainTileMap;
        [SerializeField] private Tilemap tempTileMap;

        [SerializeField] private TileBase[] tiles;
        [SerializeField] Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

        private Building temp;
        private Vector3 prevPos;
        private BoundsInt prevArea;

        #region UnityMethods
        private void Start()
        {
            // Adding automatically all tiles in `tiles` array to tileBases dict.
            int i = 0;
            foreach (TileType item in Enum.GetValues(typeof(TileType)))
            {
                //Debug.Log($"TileType: {item} Tilebase: {tiles[i]}");
                tileBases.Add(item, tiles[i]);
                if (tiles.Length >= i)
                    i++;
            }
        }

        private void Update()
        {
            FirstLocatingOfBuilding();
        }
        #endregion

        #region TileHandler
        private int GetAreaSize(BoundsInt area) => area.size.x * area.size.y * area.size.z;

        /// <summary>
        /// Usually using, wants to FirstLocating or Relocating the building.
        /// </summary>
        /// <param name="area">Building of area</param>
        /// <param name="tilemap">Which tilemap you want to work on</param>
        /// <returns></returns>
        private TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
        {
            TileBase[] tiles = new TileBase[GetAreaSize(area)];
            int counter = 0;

            foreach (Vector3Int item in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(item.x, item.y, 0);
                tiles[counter] = tilemap.GetTile(pos);
                counter++;
            }
            return tiles;
        }

        private void SetTiles(BoundsInt area, TileType type, Tilemap tilemap)
        {
            TileBase[] tiles = new TileBase[GetAreaSize(area)];
            FillTiles(tiles, type);
            tilemap.SetTilesBlock(area, tiles);
        }

        private void FillTiles(TileBase[] tileBases, TileType type)
        {
            for (int i = 0; i < tileBases.Length; i++)
            {
                tileBases[i] = this.tileBases[type];
            }
        }
        #endregion

        #region BuildingHandler
        public void InitializeWithBuilding(GameObject building)
        {
            temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
            RelocateBuilding();
        }

        public void ClearArea()
        {
            TileBase[] willbeClear = new TileBase[GetAreaSize(prevArea)];
            FillTiles(willbeClear, TileType.Empty);
            tempTileMap.SetTilesBlock(prevArea, willbeClear);
        }

        private void FirstLocatingOfBuilding()
        {
            // Checking: is player want to build anything?
            if (temp == null)
                return;

            // Changes location of the building.
            if (Input.GetMouseButtonDown(1))
            {
                if (EventSystem.current.IsPointerOverGameObject(0))
                    return;

                if (!temp.isPlaced)
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                    if (prevPos != cellPos)
                    {
                        temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                        prevPos = cellPos;
                        RelocateBuilding();
                    }
                }
            }
        }

        private void RelocateBuilding()
        {
            ClearArea();
            temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
            BoundsInt buildingArea = temp.area;
            TileBase[] baseArray = GetTilesBlock(buildingArea, mainTileMap);

            TileBase[] tileArray = new TileBase[baseArray.Length];

            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == tileBases[TileType.Grass])
                {
                    tileArray[i] = tileBases[TileType.Green];
                }
                else
                {
                    FillTiles(tileArray, TileType.Red);
                    break;
                }
            }
            tempTileMap.SetTilesBlock(buildingArea, tileArray);
            prevArea = buildingArea;
        }

        public bool CanTakeArea(BoundsInt area)
        {
            TileBase[] baseArray = GetTilesBlock(area, mainTileMap);
            foreach (var item in baseArray)
            {
                if (item != tileBases[TileType.Grass])
                {
                    Debug.Log("Cannot place here!");
                    return false;
                }
            }
            return true;
        }

        public void TakeArea(BoundsInt area)
        {
            SetTiles(area, TileType.Empty, tempTileMap);
            SetTiles(area, TileType.OnBuilding, mainTileMap);
        }
        #endregion
    }
}