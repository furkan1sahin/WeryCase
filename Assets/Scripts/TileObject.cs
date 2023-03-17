using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileObject : MonoBehaviour
{
    public int ColorID = 0;

    bool isAvailable = true;
    public GridController gridController;

    public Vector2Int myIndex;

    private void Start()
    {
        SetColor(ColorManager.instance.GetRandomColor());
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }

    public List<TileObject> CheckObject(List<TileObject> tileList)
    {
        if(!isAvailable) return tileList;

        tileList.Add(this);

        List<Vector2Int> neighboursList = gridController.FindNeighbours(myIndex);
        foreach (Vector2Int tile in neighboursList)
        {
            TileObject tileObject = gridController.GetTileObject(tile);
            if(tileObject.ColorID == tileList[0].ColorID && !tileList.Contains(tileObject))
            tileList = tileObject.CheckObject(tileList);
        }
        return tileList;
    }

    public void CrushObject()
    {
        isAvailable = false;
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
        Invoke(nameof(ResetObject), 1);  
    }

    public void ResetObject()
    {
        SetColor(ColorManager.instance.GetRandomColor());
        isAvailable = true;
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

    }

    public void NegativeShake()
    {
        transform.DOShakePosition(0.2f, 0.15f, 30);
    }

    public void SetColor(ColorData data)
    {
        ColorID = data.colorID;
        GetComponentInChildren<Renderer>().material.color = data.color;
    }

    public void HighlightObject()
    {
        transform.DOScale(1.2f, 0.1f);
        Invoke(nameof(ResetObject), 1);
    }
}
