using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    GameManager gameManager;
    public Button[] buttons;
    GameObject Check;

    private void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(ReturnButton);
        buttons[1].onClick.AddListener(SelectButton);
        buttons[2].onClick.AddListener(QuitButton);
        buttons[4].onClick.AddListener(ReturnMenu);
        Check = transform.GetChild(3).gameObject;
        Check.SetActive(false);
    }

    private void ReturnButton()
    {
        Time.timeScale = 1;
        gameManager.MenuState = GameManager.GameState.Play;
        gameObject.SetActive(false);
    }

    private void SelectButton()
    {
        Check.SetActive(true);
        buttons[3].onClick.AddListener(ToSelect);
    }

    private void QuitButton()
    {
        Check.SetActive(true);
        buttons[3].onClick.AddListener(Quit);
    }

    private void ToSelect()
    {
        buttons[3].onClick.RemoveAllListeners();
        gameManager.MenuState = GameManager.GameState.Play;
        SceneManager.LoadScene(0);
        Check.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Quit()
    {
        buttons[3].onClick.RemoveAllListeners();
        Application.Quit();
    }

    private void ReturnMenu()
    {
        buttons[3].onClick.RemoveAllListeners();
        Check.SetActive(false);
    }
}
