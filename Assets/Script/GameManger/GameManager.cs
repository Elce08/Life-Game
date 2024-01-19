using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData
{
    public int turn = 0;
    public int width = 0;
    public int height = 0;
    // public List<Cell.State[]> cellDatas;
    // public List<Cell.State> CellData = new();
    public List<List<Cell.State>> CellData = new();
}

public class GameManager : MonoBehaviour
{
    public static List<string> alphabet = new() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};

    public int turn = 0;
    public int width;
    public int height;
    Grid grid;
    Board board;

    PlayerInputSystem input;

    public GameObject saveLoad;
    Menu menu;
    GameObject selectButton;
    NewGame newMenu;

    public string currentScene;

    public enum GameState
    {
        Play,
        Menu,
        SaveLoadMenu,
        NewMenu,
    }

    public GameState MenuState;

    public enum Game
    {
        New,
        Load,
    }

    public Game newGame;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        MenuState = GameState.Play;
        input = new();
        Transform canvas = gameObject.transform.GetChild(0);
        saveLoad = canvas.GetChild(0).gameObject;
        menu = canvas.GetChild(1).GetComponent<Menu>();
        saveLoad.SetActive(false);
        menu.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        input.Menu.Enable();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        input.Menu.Disable();
    }

    private void OnSceneLoad(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadScene)
    {
        input.Menu.Menu.performed -= Menu_performed;
        currentScene = scene.name;
        MenuState = GameState.Play;
        switch (scene.name)
        {
            case "Main":
                newMenu = null;
                selectButton = null;
                grid = FindObjectOfType<Grid>();
                grid.SetHorizontal(width);
                grid.SetVertical(height);
                board = FindObjectOfType<Board>();
                board.GameStart(width, height);
        input.Menu.Menu.performed += Menu_performed;
                break;
            case "Select":
                grid = null;
                board = null;
                newMenu = FindObjectOfType<NewGame>();
                selectButton = FindObjectOfType<SelectButton>().gameObject;
                input.Menu.Menu.performed += Menu_performed;
                break;
        }
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        switch (MenuState)
        {
            case GameState.Play:
                Time.timeScale = 0;
                MenuState = GameState.Menu;
                OnMenu();
                break;
            case GameState.Menu:
                Time.timeScale = 1;
                MenuState = GameState.Play;
                menu.gameObject.SetActive(false);
                break;
            case GameState.SaveLoadMenu:
                Time.timeScale = 1;
                saveLoad.SetActive(false);
                if (selectButton != null) selectButton.SetActive(true);
                MenuState = GameState.Play;
                break;
            case GameState.NewMenu:
                if (newMenu != null) newMenu.Return();
                MenuState = GameState.Play;
                break;
        }
    }

    private void OnMenu()
    {
        menu.gameObject.SetActive(true);
        if(currentScene != "Main") menu.buttons[1].gameObject.SetActive(false);
        else menu.buttons[1].gameObject.SetActive(true);
    }
}
