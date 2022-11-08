using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    public event EventHandler OnThrow;

    [SerializeField] private int maxThrowDistance = 7;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private Transform grenadeProjectilePrefab;

    [SerializeField] private float aimingStateTime = 1.6f;
    [SerializeField] private float throwingStateTime = .3f;
    [SerializeField] private float cooloffStateTime = 1f;

    private enum State
    {
        Aiming,
        Throwing,
        Cooloff
    }

    private State state;
    private float stateTimer;

    private bool canThrowGrenade;
    private bool canStartThrowingGrenade;

    private Vector3 targetPosition;
    private Vector3 targetDirection;

    private GridPosition gridPosition;

    private void Update()
    {
        if (!isActive)
            return;

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                if (transform.forward != targetDirection)
                    transform.forward = Vector3.Lerp(transform.forward, targetDirection, unit.GetAction<MoveAction>().GetTurnSpeed() * Time.deltaTime);

                if (canStartThrowingGrenade)
                {
                    StartThrowing();
                    canStartThrowingGrenade = false;
                }
                break;
            case State.Throwing:
                if (canThrowGrenade)
                {
                    Throw();
                    canThrowGrenade = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
            NextState();
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Throwing;
                stateTimer = throwingStateTime;
                break;
            case State.Throwing:
                state = State.Cooloff;
                stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                //ActionComplete();
                break;
        }
    }

    private void StartThrowing()
    {
        OnThrow?.Invoke(this, EventArgs.Empty);
    }

    private void Throw()
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, transform.position, Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // Validations
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance) // Only consider tiles within a diamond shape
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.gridPosition = gridPosition;

        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        targetDirection = (targetPosition - transform.position).normalized;

        state = State.Aiming;
        stateTimer = aimingStateTime;

        canThrowGrenade = true;
        canStartThrowingGrenade = true;
        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}
