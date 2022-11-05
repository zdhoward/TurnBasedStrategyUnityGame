using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler<bool> OnBusyStateChange;
    public event EventHandler OnActionStart;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem in the scene!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
            return;

        if (!TurnSystem.Instance.IsPlayerTurn())
            return;

        // If pointer is over UI button
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (TryHandleUnitSelection())
            return;

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            // Validation
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
                return;

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                return;

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStart?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                        return false;

                    if (unit.IsEnemy())
                        return false;

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        this.selectedAction = baseAction;

        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyStateChange?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyStateChange?.Invoke(this, isBusy);
    }
}
