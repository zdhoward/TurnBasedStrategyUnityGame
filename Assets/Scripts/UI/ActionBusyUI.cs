using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Awake()
    {
        UnitActionSystem.Instance.OnBusyStateChange += UnityActionSystem_OnBusyStateChange;
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void UnityActionSystem_OnBusyStateChange(object sender, bool isBusy)
    {
        if (isBusy)
            Show();
        else
            Hide();
    }
}
