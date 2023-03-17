using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] GameObject UIItemPrefab;
    [SerializeField] GameObject UIItemsPanel;

    List<TMP_Text> texts= new List<TMP_Text>();
    List<int> scores= new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateUIItems();
    }

    void CreateUIItems()
    {
        List<ColorData> colorList = ColorManager.instance.GetColorList();

        foreach (ColorData item in colorList)
        {
            GameObject newText = Instantiate(UIItemPrefab, UIItemsPanel.transform);
            UIItem newItem = newText.GetComponent<UIItem>();
            newItem.image.color = item.color;
            texts.Add(newItem.text);
            scores.Add(0);
            newItem.text.text = ":0";
        }
    }

    public void AddScore(int ColorID, int pointsToAdd)
    {
        StartCoroutine(UpdateScoreText(ColorID, pointsToAdd));
    }

    IEnumerator UpdateScoreText(int index, int score)
    {
        float step = 0.3f / (float)score;
        for (int i = 0; i < score; i++)
        {
            scores[index] ++;
            texts[index].text = ":" + scores[index].ToString();
            yield return new WaitForSeconds(step);
        }
    }
}
