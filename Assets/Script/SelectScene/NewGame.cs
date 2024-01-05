using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    TMP_InputField widthInput;
    TMP_InputField heightInput;
    Slider widthSlide;
    Slider heightSlide;
    Button startButton;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        Transform width = transform.GetChild(0);
        Transform height = transform.GetChild(1);
        widthInput = width.GetComponentInChildren<TMP_InputField>();
        widthSlide = width.GetComponentInChildren<Slider>();
        heightInput = height.GetComponentInChildren<TMP_InputField>();
        heightSlide = height.GetComponentInChildren<Slider>();
        startButton = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        Width = 30;
        Height = 30;
        widthInput.onValueChanged.AddListener(SetWidth);
        heightInput.onValueChanged.AddListener(SetHeight);
        widthSlide.onValueChanged.AddListener(SetWidth);
        heightSlide.onValueChanged.AddListener(SetHeight);
        startButton.onClick.AddListener(GameStart);
    }

    public int width;
    int Width
    {
        get => width;
        set
        {
            if (value < 1) value = 1;
            if (value > 30) value = 30;
            width = value;
            widthInput.text = value.ToString();
            widthSlide.value = (float)value / 30.0f;
            gameManager.width = width;
        }
    }

    public int height;
    int Height
    {
        get => height;
        set
        {
            if (value < 1) value = 1;
            if (value > 30) value = 30;
            height = value;
            heightInput.text = value.ToString();
            heightSlide.value = (float)value / 30.0f;
            gameManager.height = height;
        }
    }

    private void SetWidth(string width)
    {
        int.TryParse(width, out int result);
        Width = result;
    }

    private void SetWidth(float width)
    {
        Width = (int)(width * 30.0f);
    }

    private void SetHeight(string height)
    {
        int.TryParse(height, out int result);
        Height = result;
    }

    private void SetHeight(float height)
    {
        Height = (int)(height * 30.0f);
    }

    private void GameStart()
    {
        gameManager.newGame = GameManager.Game.New;
        SceneManager.LoadScene(1);
    }
}
