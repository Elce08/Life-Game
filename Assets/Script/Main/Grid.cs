using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public GameObject grid;
    HorizontalLayoutGroup horizontal;
    VerticalLayoutGroup vertical;

    private void Awake()
    {
        horizontal = GetComponentInChildren<HorizontalLayoutGroup>();
        vertical = GetComponentInChildren<VerticalLayoutGroup>();
    }

    public void SetHorizontal(int width)
    {
        
        for(int i = 0; i < width; i++)
        {
            TextMeshProUGUI text = Instantiate(grid).GetComponent<TextMeshProUGUI>();
            text.gameObject.transform.SetParent(horizontal.transform);
            text.text = $"{GameManager.alphabet[i]}";
        }
    }

    public void SetVertical(int height)
    {
        for(int i = 0; i < height; i++)
        {
            TextMeshProUGUI text = Instantiate(grid).GetComponent<TextMeshProUGUI>();
            text.gameObject.transform.SetParent(vertical.transform);
            text.text = $"{i+1}";
        }
    }
}
