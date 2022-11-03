using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private Image selectedVisual;

    private TextMeshProUGUI textMeshPro;
    private Button button;

    private BaseAction baseAction;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;

        textMeshPro.text = baseAction.GetActionName().ToUpper();

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedVisual.enabled = selectedBaseAction == baseAction;
    }
}
