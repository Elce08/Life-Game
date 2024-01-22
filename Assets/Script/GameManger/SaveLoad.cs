using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;
using TMPro;

public class SaveLoad : MonoBehaviour
{
    string[] paths;
    Button[] buttons;
    GameManager gameManager;
    Board board;
    TextMeshProUGUI[] texts;

    GameObject Check;

    int Num;

    public int Loaded;

    public SaveData[] saveDatas;

    private void Awake()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();
        saveDatas = new SaveData[5];
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
            string loadJson = File.ReadAllText(paths[i]);
            saveDatas[i] = JsonUtility.FromJson<SaveData>(loadJson);
            if (saveDatas[i] != null) texts[2 * i + 1].text = $"turn : {saveDatas[i].turn}";
            else texts[2 * i + 1].text = "None";
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
        SaveLoadAct();
    }

    private void Save3()
    {
        Num = 3;
        SaveLoadAct();
    }

    private void Save4()
    {
        Num = 4;
        SaveLoadAct();
    }

    private void Save5()
    {
        Num = 5;
        SaveLoadAct();
    }

    private void ReturnSaveLoad()
    {
        Check.SetActive(false);
    }

    private void SaveLoadAct()
    {
        buttons[5].onClick.RemoveAllListeners();
        switch (gameManager.currentScene)
        {
            case "Select":
                texts[10].text = "Load";
                if (saveDatas[Num-1] == null) Check.SetActive(false);
                else
                {
                    Check.SetActive(true);
                    buttons[5].onClick.AddListener(JsonLoad);
                }
                break;
            case "Main":
                Check.SetActive(true);
                texts[10].text = "Save";
                buttons[5].onClick.AddListener(XmlSave);
                break;
        }
    }

    private void XmlSave()
    {
        board = FindObjectOfType<Board>();
        saveDatas[Num-1] = new()
        {
            turn = board.Turn,
            width = gameManager.width,
            height = gameManager.height,
            start = new()
        };
        foreach (int index in board.start) saveDatas[Num - 1].start.Add(index);

        string jsonData = JsonUtility.ToJson(saveDatas[Num-1], true);
        File.WriteAllText(paths[Num - 1], jsonData);

        texts[2 * (Num - 1) + 1].text = $"Turn : {saveDatas[Num - 1].turn}";
        Num = 0;
        Check.SetActive(false);
    }

    private void JsonLoad()
    {
        Loaded = Num - 1;
        gameManager.turn = 0;
        gameManager.width = saveDatas[Loaded].width;
        gameManager.height = saveDatas[Loaded].height;
        gameManager.newGame = GameManager.Game.Load;
        gameManager.start = saveDatas[Loaded].start;
        SceneManager.LoadScene(1);
        gameManager.MenuState = GameManager.GameState.Play;
        board = FindObjectOfType<Board>();
        Check.SetActive(false);
        gameObject.SetActive(false);
    }
}
