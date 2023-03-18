using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] float cameraDistance = 1.8f;

    private void Awake()
    {
        instance = this;
    }

    public void CalculateCameraPos(int gridWidth, int gridHeight, GridData gridData)
    {
        Vector3 gridSize = new Vector3(gridWidth * gridData.cellSize + 2 * gridData.evenTileOffset - gridData.cellSize, 0, gridHeight * gridData.cellSize * gridData.yOffsetMultiplier);
        float maxSize = Mathf.Max(gridSize.x, gridSize.z * Camera.main.aspect);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.main.fieldOfView);
        float distance = cameraDistance * maxSize / cameraView;
        distance += 0.5f * maxSize;
        Camera.main.transform.position = gridSize / 2.0f - distance * Vector3.down;
    }
}
