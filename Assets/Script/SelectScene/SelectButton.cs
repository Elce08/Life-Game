using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    Button newButton;
    Button LoadButton;
    NewGame newMenu;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        newButton = transform.GetChild(0).GetComponent<Button>();
        LoadButton = transform.GetChild(1).GetComponent<Button>();
        newMenu = FindObjectOfType<NewGame>();
        newButton.onClick.AddListener(NewMenu);
        LoadButton.onClick.AddListener(LoadMenu);
    }

    private void Start()
    {
        newMenu.gameObject.SetActive(false);
    }

    private void NewMenu()
    {
        gameManager.MenuState = GameManager.GameState.NewMenu;
        newMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void LoadMenu()
    {
        gameManager.MenuState = GameManager.GameState.SaveLoadMenu;
        gameManager.saveLoad.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
