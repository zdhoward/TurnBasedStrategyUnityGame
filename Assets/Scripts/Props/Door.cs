using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Action OnInteractComplete;

    [SerializeField] private bool isOpen;

    GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        if (isOpen)
            OpenDoor(false);
        else
            CloseDoor(false);
    }

    public void Interact(Action OnInteractComplete)
    {
        this.OnInteractComplete = OnInteractComplete;
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    private void OpenDoor(bool animated = true)
    {
        isOpen = true;
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);


        foreach (Transform child in transform)
        {
            if (animated)
                LeanTween.scaleX(child.gameObject, .1f, 1f).setEaseInOutQuad().setOnComplete(OnInteractComplete);
            else
                LeanTween.scaleX(child.gameObject, .1f, 0f).setEaseInOutQuad().setOnComplete(OnInteractComplete);

        }
    }

    private void CloseDoor(bool animated = true)
    {
        isOpen = false;
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
        foreach (Transform child in transform)
        {
            if (animated)
                LeanTween.scaleX(child.gameObject, 1f, 1f).setEaseInOutQuad().setOnComplete(OnInteractComplete);
            else
                LeanTween.scaleX(child.gameObject, 1f, 0f).setEaseInOutQuad().setOnComplete(OnInteractComplete);
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
