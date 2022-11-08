using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private Image selectedVisual;
    [SerializeField] private Image iconUI;

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

        Sprite iconSprite = baseAction.GetIcon();

        if (iconSprite != null)
        {
            iconUI.enabled = true;
            iconUI.sprite = iconSprite;
            textMeshPro.text = "";
        }
        else
        {
            textMeshPro.text = baseAction.GetActionName().ToUpper();
            iconUI.enabled = false;
        }

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
