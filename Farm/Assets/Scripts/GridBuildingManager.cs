using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Farm.Buildings;
using UnityEngine;

namespace Farm.Grid
{
    public class GridBuildingManager : MonoSingleton<GridBuildingManager>
    {
        public enum TileType { Empty, Dirt, Grass, Asphalt }

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
            tileBases.Add(TileType.Empty, null);
            tileBases.Add(TileType.Dirt, tiles[0]);
            tileBases.Add(TileType.Grass, tiles[1]);
            tileBases.Add(TileType.Asphalt, tiles[2]);
        }

        private void Update()
        {
            if (temp == null)
                return;

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
        #endregion

        #region TileHandler
        private int GetAreaSize(BoundsInt area) => area.size.x * area.size.y * area.size.z;

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
        public void InitiazlieWithBuilding(GameObject building)
        {
            temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
            Debug.Log("init!");
            RelocateBuilding();
        }

        public void ClearArea()
        {
            TileBase[] willbeClear = new TileBase[GetAreaSize(prevArea)];
            FillTiles(willbeClear, TileType.Empty);
            tempTileMap.SetTilesBlock(prevArea, willbeClear);
        }

        /*
         * Dirt => Farm -> Cannot Placeable
         * Grass => Can Placeable
         * Asphalt => Cannot Placeable
         */
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
                    tileArray[i] = tileBases[TileType.Dirt];
                }
                else
                {
                    FillTiles(tileArray, TileType.Asphalt);
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
            SetTiles(area, TileType.Asphalt, mainTileMap);
        }
        #endregion
    }
}