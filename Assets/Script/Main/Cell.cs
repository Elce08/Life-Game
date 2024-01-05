using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    Button button;
    Status status;
    Board board;
    Cell[] cells;
    List<Cell> neighbors;
    public bool onStatus;
    public Image image;

    public enum Game
    {
        Setting,
        Start,
    }

    private Game gameState;

    public Game GameState
    {
        get => gameState;
        set
        {
            gameState = value;
            button.onClick.RemoveAllListeners();
            switch (gameState)
            {
                case Game.Setting:
                    button.onClick.AddListener(GameSetting);
                    break;
                case Game.Start:
                    button.onClick.AddListener(GameStart);
                    break;
            }
        }
    }

    public enum State
    {
        Alive,
        Die,
    }

    private State cellState = State.Die;

    public State CellState
    {
        get => cellState;
        set
        {
            if (cellState != value)
            {
                cellState = value;
                switch (cellState)
                {
                    case State.Alive:
                        image.color = Color.white;
                        break;
                    case State.Die:
                        life = 0;
                        image.color = Color.clear;
                        break;
                }
            }
        }
    }

    public enum Field
    {
        Middle,
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
    }

    public Field cellField;

    public List<State> lifeCycle = new();
    int life = 0;
    public int count;  // for count alive neighbors

    private void Awake()
    {
        button = GetComponent<Button>();
        status = FindObjectOfType<Status>();
        board = FindObjectOfType<Board>();
        neighbors = new();
        image.rectTransform.sizeDelta = board.cellSize;
        image.color = Color.clear;
    }

    public void Life()
    {
        count = 0;
        foreach (Cell cell in neighbors) if (cell.lifeCycle[lifeCycle.Count-1] == State.Alive) count++;
        switch (CellState)
        {
            case State.Alive:
                if (count < 2 || count > 3) CellState = State.Die;
                else life++;
                break;
            case State.Die:
                if (count == 3)
                {
                    CellState = State.Alive;
                    life++;
                }
                break;
        }
        lifeCycle.Add(CellState);
        if (onStatus) GameStart();
    }

    public void SetNeighbors(int width, int index)
    {
        cells = board.cells;
        switch (cellField)
        {
            case Field.UpLeft:
                neighbors.Add(cells[index + 1]);
                neighbors.Add(cells[index + width]);
                neighbors.Add(cells[index + width + 1]);
                break;
            case Field.UpRight:
                neighbors.Add(cells[index - 1]);
                neighbors.Add(cells[index + width]);
                neighbors.Add(cells[index + width - 1]);
                break;
            case Field.DownLeft:
                neighbors.Add(cells[index + 1]);
                neighbors.Add(cells[index - width]);
                neighbors.Add(cells[index - width + 1]);
                break;
            case Field.DownRight:
                neighbors.Add(cells[index - 1]);
                neighbors.Add(cells[index - width]);
                neighbors.Add(cells[index - width - 1]);
                break;
            case Field.Up:
                neighbors.Add(cells[index + 1]);
                neighbors.Add(cells[index - 1]);
                neighbors.Add(cells[index + width]);
                neighbors.Add(cells[index + width - 1]);
                neighbors.Add(cells[index + width + 1]);
                break;
            case Field.Down:
                neighbors.Add(cells[index + 1]);
                neighbors.Add(cells[index - 1]);
                neighbors.Add(cells[index - width]);
                neighbors.Add(cells[index - width - 1]);
                neighbors.Add(cells[index - width + 1]);
                break;
            case Field.Left:
                neighbors.Add(cells[index + 1]);
                neighbors.Add(cells[index + width]);
                neighbors.Add(cells[index - width]);
                neighbors.Add(cells[index + width + 1]);
                neighbors.Add(cells[index - width + 1]);
                break;
            case Field.Right:
                neighbors.Add(cells[index - 1]);
                neighbors.Add(cells[index + width]);
                neighbors.Add(cells[index - width]);
                neighbors.Add(cells[index + width - 1]);
                neighbors.Add(cells[index - width - 1]);
                break;
            default:
                neighbors.Add(cells[index + 1]);
                neighbors.Add(cells[index - 1]);
                neighbors.Add(cells[index + width]);
                neighbors.Add(cells[index - width]);
                neighbors.Add(cells[index + width + 1]);
                neighbors.Add(cells[index - width + 1]);
                neighbors.Add(cells[index + width - 1]);
                neighbors.Add(cells[index - width - 1]);
                break;
        }
    }

    public void GameSetting()
    {
        switch (CellState)
        {
            case State.Alive:
                CellState = State.Die;
                break;
            case State.Die:
                CellState = State.Alive;
                break;
        }
    }

    private void GameStart()
    {
        foreach (Cell cell in cells) cell.onStatus = false;
        onStatus = true;
        switch (CellState)
        {
            case State.Alive:
                status.ChangeText(gameObject.name, $"{life}");
                break;
            case State.Die:
                status.ChangeText(gameObject.name, "Die");
                break;
        }
    }
}
