using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance;

    public event EventHandler OnTurnChange;

    private int turnNumber = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one TurnSystem in the scene! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;
        OnTurnChange?.Invoke(this, EventArgs.Empty);
    }

    public int GetCurrentTurnNumber()
    {
        return turnNumber;
    }
}
