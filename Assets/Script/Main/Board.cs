using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    GameManager gameManager;        // Game Total Manage

    public GameObject cell;
    GridLayoutGroup layOut;         // Cell Grid Manage
    public Cell[] cells;            // Every Cells

    public float speed = 10.0f;     // Turn Speed
    public int Turn;
    int MaxTurn;                    // Max Turn which already computed
    TextMeshProUGUI turn;           // Turn Display
    TextMeshProUGUI changeMaxTurn;  // Max Turn Display
    TMP_InputField changeTurnInput; // Turn Change InputField
    TMP_InputField speedInput;      // Speed InputField
    Button StartSaveButton;         // Game Start & Save Button
    TextMeshProUGUI StartSave;      // Game Start & Save Button Text
    /// <summary>
    /// x: width y: height
    /// </summary>
    public Vector2 cellSize;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        layOut = GetComponentInChildren<GridLayoutGroup>();
        turn = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        changeMaxTurn = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        changeTurnInput = transform.GetChild(2).GetComponentInChildren<TMP_InputField>();
        speedInput = transform.GetChild(3).GetComponent<TMP_InputField>();
        StartSaveButton = transform.GetChild(5).GetComponent<Button>();
        StartSave = StartSaveButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Setting New Game
    /// </summary>
    /// <param name="width">width</param>
    /// <param name="height">height</param>
    public void GameStart(int width, int height)
    {
        SetCells(width, height);
        Turn = 0;
        MaxTurn = 0;
        turn.text = $"{Turn}";
        changeMaxTurn.text = $"{MaxTurn}";
        changeTurnInput.text = $"{Turn}";
        speedInput.text = $"{speed}";
        speedInput.onEndEdit.AddListener(SetSpeed);
        switch (gameManager.newGame)
        {
            case GameManager.Game.New:
                foreach (Cell cell in cells) cell.GameState = Cell.Game.Setting;
                StartSave.text = "Start";
                StartSaveButton.onClick.AddListener(NewGameStart);
                break;
            case GameManager.Game.Load:
                LoadGameStart();
                Time.timeScale = 0;
                break;
        }
    }

    private void NewGameStart()
    {
        foreach (Cell cell in cells)
        {
            cell.PreGame();
            cell.GameState = Cell.Game.Start;
        }
        StartSaveButton.onClick.RemoveAllListeners();
        StartSaveButton.onClick.AddListener(Save);
        StartSave.text = "Save";
        StartCoroutine(Game());
        changeTurnInput.onValueChanged.AddListener(ChangeTurn);
    }

    private void LoadGameStart()
    {
        foreach(Cell cell in cells)
        {
            cell.PreGame();
            cell.GameState = Cell.Game.Start;
        }
        Turn = gameManager.turn;
        MaxTurn = gameManager.turn;
        StartSaveButton.onClick.AddListener(Save);
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
            foreach (Cell cell in cells) cell.Life(Turn,MaxTurn);
            CellCount();
            yield return new WaitForSeconds(speed);
        }
    }

    /// <summary>
    /// Make Board
    /// </summary>
    /// <param name="width">width</param>
    /// <param name="height">height</param>
    private void SetCells(int width, int height)
    {
        cells = new Cell[width * height];
        cellSize = new(1000.0f / width, 1000.0f / height);
        layOut.cellSize = cellSize;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int index = i * width + j;
                cells[index] = Instantiate(cell, layOut.transform).GetComponent<Cell>();
                cells[index].index = index;
                cells[index].gameObject.name = $"{GameManager.alphabet[j]}{i + 1}";
                if (i == 0)
                {
                    if (j == 0) cells[index].cellField = Cell.Field.UpLeft;
                    else if (j == width - 1) cells[index].cellField = Cell.Field.UpRight;
                    else cells[index].cellField = Cell.Field.Up;
                }
                else if (i == height - 1)
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
        cells[0].GameStart();
    }

    private void SetSpeed(string speed)
    {
        float.TryParse(speed, out float result);
        this.speed = result;
    }

    private void CellCount()
    {
        int count = 0;
        foreach(Cell cell in cells) if(cell.CellState == Cell.State.Alive) count++;
        if(count == 0) StopAllCoroutines();
    }

    public void Save()
    {
        gameManager.MenuState = GameManager.GameState.SaveLoadMenu;
        gameManager.saveLoad.SetActive(true);
        Time.timeScale = 0;
    }
}
