using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float turnSpeed = 10f;

    [SerializeField] private int maxMoveDistance = 4;

    private UnitAnimator unitAnimator;
    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        unitAnimator = GetComponentInChildren<UnitAnimator>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive)
            return;

        MoveUpdate();
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        ActionStart(onActionComplete);

        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }

    private void MoveUpdate()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // Movement
        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }

        // Rotation
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, turnSpeed * Time.deltaTime);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // Validations
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                if (unit.GetGridPosition() == testGridPosition)
                    continue;

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public float GetTurnSpeed()
    {
        return turnSpeed;
    }
}
