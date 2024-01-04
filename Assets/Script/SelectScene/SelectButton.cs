using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    Button newButton;
    NewGame newMenu;

    private void Awake()
    {
        newButton = transform.GetChild(0).GetComponent<Button>();
        newMenu = FindObjectOfType<NewGame>();
        newMenu.gameObject.SetActive(false);
        newButton.onClick.AddListener(NewMenu);
    }

    private void NewMenu()
    {
        newMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
