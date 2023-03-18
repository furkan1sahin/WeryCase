using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GridData")]
public class GridData : ScriptableObject
{
    public GameObject tilePrefab;
    public float cellSize = 2;
    public float evenTileOffset = 0.5f;
    public float yOffsetMultiplier = 0.866f;
}
