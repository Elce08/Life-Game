using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    Button back;
    Button pause;
    Button resume;
    Board board;

    private void Awake()
    {
        back = transform.GetChild(0).GetComponent<Button>();
        pause = transform.GetChild(1).GetComponent<Button>();
        resume = transform.GetChild(2).GetComponent<Button>();
        board = FindObjectOfType<Board>();
    }

    private void OnEnable()
    {
        back.onClick.AddListener(Back);
        pause.onClick.AddListener(Pause);
        resume.onClick.AddListener(Resume);
    }

    private void OnDisable()
    {
        back.onClick.RemoveListener(Back);
        pause.onClick.RemoveListener(Pause);
        resume.onClick.RemoveListener(Resume);
    }

    private void Back()
    {
        board.Turn--;
    }
    
    private void Pause()
    {
        Time.timeScale = 0.0f;
    }

    private void Resume()
    {
        Time.timeScale = 1.0f;
    }
}
