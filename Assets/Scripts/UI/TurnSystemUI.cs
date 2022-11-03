using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI currentTurnLabel;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        UpdateCurrentTurnLabel();
    }

    private void UpdateCurrentTurnLabel()
    {
        currentTurnLabel.text = "TURN " + TurnSystem.Instance.GetCurrentTurnNumber();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateCurrentTurnLabel();
    }
}
