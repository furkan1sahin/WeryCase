using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    [SerializeField] int addRandomColorCount;

    [SerializeField] List<ColorData> colors = new List<ColorData>();

    private void Awake()
    {
        instance = this;
        GenerateColors(addRandomColorCount);
    }

    void GenerateColors(int ColorCount)
    {
        for (int i = 0; i < ColorCount; i++)
        {
            ColorData newData = new ColorData();
            newData.colorID = i;
            newData.color = Color.HSVToRGB(1.0f / ColorCount * i, 0.7f, 0.8f);
            colors.Add(newData);
        }
    }

    public ColorData GetRandomColor()
    {
        return colors[Random.Range(0, colors.Count)];
    }

    public List<ColorData> GetColorList()
    {
        return colors;
    }
}
