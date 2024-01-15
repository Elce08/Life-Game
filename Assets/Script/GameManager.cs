using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static List<string> alphabet = new() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};

    public int width;
    public int height;
    Grid grid;
    Board board;

    PlayerInputSystem input;

    public GameObject saveLoad;
    GameObject menu;
    GameObject selectButton;
    NewGame newMenu;

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
        menu = canvas.GetChild(1).gameObject;
        saveLoad.SetActive(false);
        menu.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        input.Menu.Enable();
        input.Menu.Menu.performed += Menu_performed;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        input.Menu.Menu.performed -= Menu_performed;
        input.Menu.Disable();
    }

    private void OnSceneLoad(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadScene)
    {
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
                break;
            case "Select":
                grid = null;
                board = null;
                newMenu = FindObjectOfType<NewGame>();
                selectButton = FindObjectOfType<SelectButton>().gameObject;
                break;
        }
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        switch (MenuState)
        {
            case GameState.Play:
                MenuState = GameState.Menu;
                menu.SetActive(true);
                break;
            case GameState.Menu:
                MenuState = GameState.Play;
                menu.SetActive(false);
                break;
            case GameState.SaveLoadMenu:
                MenuState = GameState.Play;
                saveLoad.SetActive(false);
                if (selectButton != null) selectButton.SetActive(true);
                break;
            case GameState.NewMenu:
                MenuState = GameState.Play;
                if (newMenu != null) newMenu.Return();
                break;
        }
    }
}
