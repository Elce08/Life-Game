using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnything : MonoBehaviour
{
    PlayerInputSystem input;

    private void Awake()
    {
        input = new(); 
    }

    private void OnEnable()
    {
        input.Start.Enable();
        input.Start.Start.performed += KeyPressed;
    }

    private void OnDisable()
    {
        input.Start.Start.performed -= KeyPressed;
        input.Start.Disable();
    }

    private void KeyPressed(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        SceneManager.LoadScene(0);
    }
}
