using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsLabel;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChange += Unit_OnAnyActionPointsChange;
        healthSystem.OnHealthChange += HealthSystem_OnHealthChange;
        UpdateActionPointsLabel();
        UpdateHealthBar();
    }

    private void UpdateActionPointsLabel()
    {
        actionPointsLabel.text = "" + unit.GetActionPoints();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void Unit_OnAnyActionPointsChange(object sender, EventArgs e)
    {
        UpdateActionPointsLabel();
    }

    private void HealthSystem_OnHealthChange(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
