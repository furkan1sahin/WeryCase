using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] int tileCountToDestroy = 3;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;

    [SerializeField] float cellSize = 1;
    const float evenTileOffset = 0.5f;
    const float yOffsetMultiplier = 0.866f;

    TileObject[,] tileMatrix;

    Plane plane = new Plane(Vector3.up, 0);

    TileObject highlighted=null;

    bool isCreated = false;

    float padding = 0.5f;

    void Start()
    {
        if (gridWidth == 0) CalculateMaxWidth();
        if (gridHeight == 0) CalculateMaxHeigth();

        tileMatrix = new TileObject[gridWidth, gridHeight];
        StartCoroutine(CreateGrid());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = GetScreenToWorldPoint(Input.mousePosition);

            if (isCreated)
                FindClickedTile(transform.TransformPoint(worldPosition));
        }
    }

    void FindClickedTile(Vector3 clickPos)
    {
        clickPos.x += (clickPos.y % 2) * cellSize * evenTileOffset;

        Vector2Int roughPos = new Vector2Int(Mathf.RoundToInt(clickPos.x / cellSize),
            Mathf.RoundToInt(clickPos.z / (cellSize * yOffsetMultiplier)));

        if (roughPos.x < 0 || roughPos.y < 0 || roughPos.x >= tileMatrix.GetLength(0) || roughPos.y >= tileMatrix.GetLength(1)) return;

        
        highlighted = tileMatrix[roughPos.x, roughPos.y];

        float closestDist = Vector3.Distance(clickPos, highlighted.transform.position);
        foreach (var tile in FindNeighbours(roughPos)) 
        {
            float dist = Vector3.Distance(clickPos, tileMatrix[tile.x, tile.y].transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                highlighted = tileMatrix[tile.x, tile.y];
            }
        }

        List<TileObject> tileObjects = new List<TileObject>();
        tileObjects = highlighted.CheckObject(tileObjects);

        if (tileObjects.Count >= tileCountToDestroy)
        {
            foreach (var tileObject in tileObjects)
            {
                tileObject.CrushObject();
            }
            ScoreManager.Instance.AddScore(tileObjects[0].ColorID, tileObjects.Count);
        }
        else if (tileObjects.Count > 0)
        {
            tileObjects[0].NegativeShake();
        }
        //highlighted.HighlightObject();
        
        //foreach (var tile in FindNeighbours(highlighted.myIndex))
        //{
        //    tileMatrix[tile.x, tile.y].HighlightObject();
        //}
    }

    public List<Vector2Int> FindNeighbours(Vector2Int index)
    {
        List<Vector2Int> neighbourList= new List<Vector2Int>();
        if(index.x - 1 >=0) neighbourList.Add(new Vector2Int(index.x-1, index.y));
        if(index.x + 1 < tileMatrix.GetLength(0)) neighbourList.Add(new Vector2Int(index.x+1, index.y));

        if(index.y - 1 >= 0)
        {
            int x = index.x + index.y % 2;
            if(x-1 >= 0) neighbourList.Add(new Vector2Int(x-1, index.y-1));
            if (x < tileMatrix.GetLength(0)) neighbourList.Add(new Vector2Int(x, index.y - 1));
        }

        if(index.y + 1 < tileMatrix.GetLength(1))
        {
            int x = index.x + index.y % 2;
            if (x-1 >= 0) neighbourList.Add(new Vector2Int(x-1, index.y + 1));
            if (x < tileMatrix.GetLength(0)) neighbourList.Add(new Vector2Int(x, index.y + 1));
        }

        return neighbourList;
    }

    IEnumerator CreateGrid()
    {
        float tileWidth = cellSize;
        float tileHeight = cellSize * yOffsetMultiplier;

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                float xPos = x * tileWidth + (y % 2 * tileWidth * evenTileOffset);
                float yPos = y * tileHeight;

                GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, 0, yPos), Quaternion.identity);
                tile.transform.parent = transform;
                TileObject tileObj = tile.GetComponent<TileObject>();
                tileMatrix[x, y] = tileObj;
                //tileObj.SetColor(colorManager.GetRandomColor());
                tileObj.gridController = this;
                tileObj.myIndex = new Vector2Int(x, y);
                yield return new WaitForSeconds(0.01f);
            }
        }

        isCreated= true;
    }

    void CalculateMaxWidth()
    {
        Vector3 leftBorder = GetScreenToWorldPoint(Vector2.zero);
        Vector3 rightBorder = GetScreenToWorldPoint(new Vector2(Screen.width, 0));
        gridWidth = (int)((rightBorder.x - leftBorder.x - evenTileOffset - padding)/cellSize);
    }

    void CalculateMaxHeigth()
    {
        Vector3 bottomBorder = GetScreenToWorldPoint(Vector2.zero);
        Vector3 topBorder = GetScreenToWorldPoint(new Vector2(0, Screen.height));
        gridHeight = (int)((topBorder.z - bottomBorder.z - padding) / cellSize);
    }

    Vector3 GetScreenToWorldPoint(Vector2 screenPoint)
    {
        Vector3 worldPosition = new Vector3();
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        return worldPosition;
    }

    public TileObject GetTileObject(Vector2Int index)
    {
        return tileMatrix[index.x, index.y];
    }
}
