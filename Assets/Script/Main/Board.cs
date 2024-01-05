using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    GameManager gameManager;
    public GameObject cell;
    GridLayoutGroup layOut;
    public Cell[] cells;
    public float speed = 10.0f;
    public int Turn;
    int MaxTurn;
    TextMeshProUGUI turn;
    TextMeshProUGUI changeMaxTurn;
    TMP_InputField changeTurnInput;
    Button StartSaveButton;
    TextMeshProUGUI StartSave;
    public Vector2 cellSize;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        layOut = GetComponentInChildren<GridLayoutGroup>();
        turn = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        changeMaxTurn = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        changeTurnInput = transform.GetChild(2).GetComponentInChildren<TMP_InputField>();
        StartSaveButton = transform.GetChild(5).GetComponent<Button>();
        StartSave = StartSaveButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void GameStart(int width, int height)
    {
        SetCells(width, height);
        Turn = 0;
        MaxTurn = 0;
        turn.text = $"{Turn}";
        changeMaxTurn.text = $"{MaxTurn}";
        changeTurnInput.text = $"{Turn}";
        foreach (Cell cell in cells) cell.lifeCycle.Clear();
        switch (gameManager.newGame)
        {
            case GameManager.Game.New:
                foreach (Cell cell in cells) cell.GameState = Cell.Game.Setting;
                StartSave.text = "Start";
                StartSaveButton.onClick.AddListener(NewGameStart);
                break;
        }
    }

    private void NewGameStart()
    {
        foreach (Cell cell in cells) cell.GameState = Cell.Game.Start;
        StartSaveButton.onClick.RemoveAllListeners();
        StartSave.text = "Save";
        StartCoroutine(Game());
        changeTurnInput.onValueChanged.AddListener(ChangeTurn);
    }

    private void ChangeTurn(string turn)
    {
        foreach (Cell cell in cells) cell.lifeCycle.Add(cell.CellState);
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
            foreach (Cell cell in cells) cell.Life();
            yield return new WaitForSeconds(speed);
        }
    }

    private void SetCells(int width, int height)
    {
        cells = new Cell[width * height];
        cellSize = new(1000.0f / width, 1000.0f / height);
        layOut.cellSize = cellSize;
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                int index = i * width + j;
                cells[index] = Instantiate(cell, layOut.transform).GetComponent<Cell>();
                cells[index].gameObject.name = $"{GameManager.alphabet[j]}{i + 1}";
                if(i == 0)
                {
                    if (j == 0) cells[index].cellField = Cell.Field.UpLeft;
                    else if (j == width - 1) cells[index].cellField = Cell.Field.UpRight;
                    else cells[index].cellField = Cell.Field.Up;
                }
                else if(i == height - 1)
                {
                    if (j == 0) cells[index].cellField = Cell.Field.DownLeft;
                    else if (j == width - 1) cells[index].cellField = Cell.Field.DownRight;
                    else cells[index].cellField = Cell.Field.Down;
                }
                else
                {
                    if (j == 0) cells[index].cellField = Cell.Field.Left;
                    else if (j == width - 1) cells[index].cellField = Cell.Field.Right;
                    else cells[index].cellField = Cell.Field.Middle;
                }
            }
        }
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].SetNeighbors(width, i);
        }
    }
}
