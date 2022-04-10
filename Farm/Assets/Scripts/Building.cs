using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool isPlaced { get; private set; }
    public BoundsInt area;

    public bool CanBePlaced()
    {
        Vector3Int position = GridBuildingManager.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = position;

        if (GridBuildingManager.Instance.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int position = GridBuildingManager.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = position;
        isPlaced = true;
        GridBuildingManager.Instance.TakeArea(areaTemp);
    }
}
