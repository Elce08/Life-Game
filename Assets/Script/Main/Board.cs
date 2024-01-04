using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject cell;
    GridLayoutGroup layOut;
    Cell[] cells;
    public float speed = 10.0f;
    public int Turn;
    int MaxTurn;
    TextMeshProUGUI turn;
    TextMeshProUGUI changeMaxTurn;
    TMP_InputField changeTurnInput;

    private void Awake()
    {
        layOut = GetComponentInChildren<GridLayoutGroup>();
        turn = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        changeMaxTurn = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        changeTurnInput = transform.GetChild(2).GetComponentInChildren<TMP_InputField>();
    }

    public void GameStart(int width, int height)
    {
        cells = new Cell[width * height];
        layOut.cellSize = new(1000.0f / width, 1000.0f / height);
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                cells[j * width + i] = Instantiate(cell, layOut.transform).GetComponent<Cell>();
                cells[j * width + i].gameObject.name = $"{GameManager.alphabet[j]}{i + 1}";
            }
        }
        Turn = 0;
        MaxTurn = 0;
        foreach(Cell cell in cells) cell.lifeCycle.Clear();
        StartCoroutine(Game());
        changeTurnInput.onValueChanged.AddListener(ChangeTurn);
    }

    private void ChangeTurn(string turn)
    {
        int.TryParse(turn, out int result);
        if(result < MaxTurn && result > 0) Turn = result-1;
    }

    IEnumerator Game()
    {
        while (true)
        {
            Turn++;
            if(Turn > MaxTurn)MaxTurn = Turn;
            turn.text = $"{Turn}";
            changeMaxTurn.text = $"{MaxTurn}";
            changeTurnInput.text = $"{Turn}";
            yield return new WaitForSeconds(speed);
        }
    }
}
