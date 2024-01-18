using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;

public class SaveLoad : MonoBehaviour
{
    string[] paths;
    Button[] buttons;
    GameManager gameManager;
    Board board;

    GameObject Check;

    int Num;

    private void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
        buttons = GetComponentsInChildren<Button>();
        paths = new string[5];
        buttons[0].onClick.AddListener(Save1);
        buttons[1].onClick.AddListener(Save2);
        buttons[2].onClick.AddListener(Save3);
        buttons[3].onClick.AddListener(Save4);
        buttons[4].onClick.AddListener(Save5);
        buttons[6].onClick.AddListener(ReturnSaveLoad);
        Check = transform.GetChild(5).gameObject;
        Check.SetActive(false);
    }

    private void Start()
    {
        for(int i = 0; i < paths.Length; i++)
        {
            paths[i] = Path.Combine(Application.dataPath + "/Datas/", $"Data{i}.json");
        }
    }

    private void Save1()
    {
        Num = 1;
        SaveLoadAct();
    }

    private void Save2()
    {
        Num = 2;
    }

    private void Save3()
    {
        Num = 3;
    }

    private void Save4()
    {
        Num = 4;
    }

    private void Save5()
    {
        Num = 5;
    }

    private void ReturnSaveLoad()
    {
        Check.SetActive(false);
    }

    private void SaveLoadAct()
    {
        Check.SetActive(true);
        buttons[5].onClick.RemoveAllListeners();
        switch (gameManager.currentScene)
        {
            case "Select":
                buttons[5].onClick.AddListener(JsonLoad);
                break;
            case "Main":
                buttons[5].onClick.AddListener(JsonSave);
                break;
        }
    }

    private void JsonSave()
    {
        board = FindObjectOfType<Board>();
        SaveData saveData = new()
        {
            turn = board.Turn,
            width = gameManager.width,
            height = gameManager.height,
        };
        saveData.cellDatas = new();
        for(int i = 0; i < board.cells.Length; i++)
        {
            saveData.cellDatas.Add(null);
            saveData.cellDatas[i] = new Cell.State[board.Turn];
            for (int j = 0; j < saveData.cellDatas[i].Length; j++) saveData.cellDatas[i][j] = board.cells[i].lifeCycle[j];
        }

        string jsonData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(paths[Num-1],jsonData);

        Time.timeScale = 1;
        Num = 0;
        Check.SetActive(false);
        gameObject.SetActive(false);
    }

    private void JsonLoad()
    {

    }
}
