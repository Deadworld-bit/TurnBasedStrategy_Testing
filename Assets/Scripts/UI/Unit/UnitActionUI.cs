using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI actionPointsText;
   [SerializeField] private Unit unit;
   [SerializeField] private Image hpBarImage;
   [SerializeField] private HealthPointSystem healthPointSystem;
   [SerializeField] private Gradient healthBarGradient;

   //Down side for this is it will automactically update every unit if one unit consume action points
   private void Start()
   {
      Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
      healthPointSystem.OnDamaged += HealthPointSystem_OnDamaged;
      
      UpdateActionPointsText();
      UpdateHealthBar();
   }

   private void UpdateActionPointsText()
   {
      actionPointsText.text = unit.GetActionPoints().ToString();
   }

   private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
   {
      UpdateActionPointsText();
   }

   //Update Health Bar image
   private void UpdateHealthBar()
   {
      hpBarImage.fillAmount = healthPointSystem.getHealthPercentage();
      hpBarImage.color = healthBarGradient.Evaluate(hpBarImage.fillAmount);
   }

   private void HealthPointSystem_OnDamaged(object sender, EventArgs e)
   {
      UpdateHealthBar();
   }
}
