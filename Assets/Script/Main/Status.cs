using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Status : MonoBehaviour
{
    TextMeshProUGUI grid;
    TextMeshProUGUI old;

    private void Awake()
    {
        grid = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        old = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Refresh()
    {
        grid.text = "";
        old.text = "";
    }

    public void ChangeText(string grid, string old)
    {
        this.grid.text = grid;
        this.old.text = old;
    }
}
