using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum State
    {
        Alive,
        Die,
    }

    private State cellState = State.Die;

    public State CellState
    {
        get => cellState;
        set
        {
            if (cellState != value)
            {
                cellState = value;
                switch (cellState)
                {
                    case State.Alive:
                        break;
                    case State.Die:
                        break;
                }
            }
        }
    }

    public List<int> lifeCycle = new();

    public void Life()
    {

    }
}
