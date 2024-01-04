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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadScene)
    {
        switch (scene.name)
        {
            case "Main":
                grid = FindObjectOfType<Grid>();
                grid.SetHorizontal(width);
                grid.SetVertical(height);
                board = FindObjectOfType<Board>();
                board.GameStart(width, height);
                break;
        }
    }
}
