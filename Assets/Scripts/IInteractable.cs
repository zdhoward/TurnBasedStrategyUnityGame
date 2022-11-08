using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact(Action OnInteractComplete);
}
