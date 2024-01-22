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
    GameManager gameManager;
    public bool onStatus;
    public Image image;
    public int index;

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
            button.onClick.AddListener(GameStart);
            if (gameState == Game.Setting) button.onClick.AddListener(GameSetting);
            /*switch (gameState)
            {
                case Game.Setting:
                    button.onClick.AddListener(GameSetting);
                    break;
                case Game.Start:
                    break;
            }*/
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
                        image.color = Color.black;
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

    public List<State> lifeCycle;
    int life = 0;
    public int count;  // for count alive neighbors

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        lifeCycle = new();
        button = GetComponent<Button>();
        status = FindObjectOfType<Status>();
        board = FindObjectOfType<Board>();
        neighbors = new();
        image.rectTransform.sizeDelta = board.cellSize;
        image.color = Color.clear;
    }

    public void PreGame()
    {
        lifeCycle.Clear();
        if (gameManager.newGame == GameManager.Game.Load)
        {
            if (gameManager.start.Contains(index)) CellState = State.Alive;
            else CellState = State.Die;
        }
        else if (CellState == State.Alive) board.start.Add(index);
        lifeCycle.Add(CellState);
    }

    /// <summary>
    /// Cell in Each Turn
    /// </summary>
    /// <param name="turn">Present Turn</param>
    /// <param name="maxtTurn">Max Turn</param>
    public void Life(int turn, int maxtTurn)
    {
        count = 0;
        foreach (Cell cell in neighbors) if (cell.lifeCycle[lifeCycle.Count-1] == 0) count++;
        if (turn == maxtTurn)
        {
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
        }
        else CellState = lifeCycle[turn];
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

    public void GameStart()
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
