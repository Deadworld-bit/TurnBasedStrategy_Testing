using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private UnitActionBase actionBase;

    public void SetBaseAction(UnitActionBase actionBase)
    {
        this.actionBase = actionBase;
        textMeshPro.text = actionBase.GetActionName().ToUpper();

        // button.onClick.AddListener(MoveActionBtn_OnClick);     Simple way to active button
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(actionBase);
        });
    }

    // private void MoveActionBtn_OnClick(){
    // }

    public void UpdateSelectedVisual()
    {
        UnitActionBase selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == actionBase);
    }
}
