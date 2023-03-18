using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridController : GridController
{
    public override List<Vector2Int> FindNeighbours(Vector2Int index)
    {
        List<Vector2Int> neighbourList = new List<Vector2Int>();
        if (index.x - 1 >= 0) neighbourList.Add(new Vector2Int(index.x - 1, index.y));
        if (index.x + 1 < tileMatrix.GetLength(0)) neighbourList.Add(new Vector2Int(index.x + 1, index.y));

        if (index.y - 1 >= 0) neighbourList.Add(new Vector2Int(index.x, index.y - 1));
        if (index.y + 1 < tileMatrix.GetLength(1)) neighbourList.Add(new Vector2Int(index.x, index.y + 1));

        return neighbourList;
    }
}
