using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;

    private MeshRenderer meshRenderer;

    private GridPosition gridPosition;

    private Action onInteractComplete;
    private bool isActive;
    private float timer;

    private bool isGreen;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColorGreen();
    }

    private void Update()
    {
        if (!isActive)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractComplete();
        }
    }

    private void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    private void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action OnInteractComplete)
    {
        this.onInteractComplete = OnInteractComplete;

        isActive = true;
        timer = .5f;

        if (isGreen)
            SetColorRed();
        else
            SetColorGreen();
    }
}
