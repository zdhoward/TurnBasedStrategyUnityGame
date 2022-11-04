using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int maxActionPoints = 2;
    [SerializeField] private bool isEnemy;

    public static event EventHandler OnAnyActionPointsChange;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDied;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private ShootAction shootAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;

    private int actionPoints;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        shootAction = GetComponent<ShootAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();

        actionPoints = maxActionPoints;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        healthSystem.OnDeath += HealthSystem_OnDeath;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public ShootAction GetShootAction()
    {
        return shootAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }

        return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return baseAction.GetActionPointsCost() <= actionPoints;
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChange?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
        (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = maxActionPoints;
            OnAnyActionPointsChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        Destroy(gameObject);

        OnAnyUnitDied?.Invoke(this, EventArgs.Empty);
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
}
