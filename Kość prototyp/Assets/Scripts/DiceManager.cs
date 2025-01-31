using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using DataModels;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private List<SingleDice> diceList = new List<SingleDice>();
    [SerializeField] private ComboList comboList;
    [SerializeField] private TextMeshProUGUI textTMP;
    [SerializeField] private TextMeshProUGUI enemyNameTMP;
    [SerializeField] private RectTransform combosRoot;
    [SerializeField] private TextMeshProUGUI comboItemPrefab;

    private bool isCharging;
    private bool wasTossed = false;

    public Slider powerSlider;
    public Slider healthSlider;
    [SerializeField] private Color lowPowerColor = Color.green;
    [SerializeField] private Color highPowerColor = Color.red;
    [SerializeField] private CombatResolver combatResolver;
    private bool combatResolved;
    private List<Vector3> dicePositions = new List<Vector3>();
    private void Start()
    {
        foreach (SingleDice dice in diceList)
        {
            dice.diceManager = this;
            dicePositions.Add(dice.transform.position);
        }

        if (powerSlider != null)
        {
            powerSlider.minValue = diceList[0].minPower; // Ustawienie wartości minimalnej mocy
            powerSlider.maxValue = diceList[0].maxPower; // Ustawienie wartości maksymalnej mocy
            powerSlider.value = diceList[0].minPower; // Ustawienie początkowej wartości
        }

        FillComboList(comboList);
        combatResolver.Init();
        textTMP.text = string.Empty;
    }



    void Update()
    {
        if (combatResolved == true)
        {
            if (Input.GetKey(KeyCode.R))
            {
                ResetAllDice();
                powerSlider.value = powerSlider.minValue;
                textTMP.text = string.Empty;
                combatResolved = false;
                isCharging = false;
                wasTossed = false;
                return;
            }
            return;
        }
        if (Input.GetKey(KeyCode.Space) && !wasTossed)
        {
            isCharging = true;
            ChargeAllDice();
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging && !wasTossed)
        {
            isCharging = false;
            ThrowAllDice();
            wasTossed = true;
        }

        if (AreAllDiceResting())
        {
            List<DiceData> results = GatherAllResults();
            var builder = new StringBuilder();
            foreach (DiceData diceData in results)
            {
                builder.Append(diceData.ToString());
                builder.Append(' ');
            }
            textTMP.SetText(builder.ToString());
            var damage = VerifyCombos(results, combatResolver.CurrentEnemyData);
            combatResolver.ResolveCombat(damage);
            combatResolved = true;
        }
    }
    private void LateUpdate()
    {
        if(combatResolver.CurrentEnemy == null)
        {
            return;
        }

        HealthModel healthModel = combatResolver.CurrentEnemy.HealthModel;
        healthSlider.value = healthModel.GetPercentage();
        enemyNameTMP.SetText($"{combatResolver.CurrentEnemy.EnemyData.Name} ({healthModel.Health}/{healthModel.MaxHealth})");
    }

    private int VerifyCombos(List<DiceData> results, EnemyData enemyData)
    {
        return comboList.MatchesCombo(results.Select(x => x.Side).ToList(), enemyData.MinimumRank, enemyData.CriticalRank);

    }

    private List<DiceData> GatherAllResults()
    {
        var results = new List<DiceData>();

        foreach (var dice in diceList)
        {
            var result = dice.GetResult();
            if (result == null) continue;
            results.Add(result);
        }
        return results;
    }

    private bool AreAllDiceResting()
    {
        return diceList.All(x => x.HasResult);
    }
    private void ResetAllDice()
    {
        for (int i = 0; i < diceList.Count; i++)
        {
            SingleDice dice = diceList[i];
            dice.Reset();
            dice.transform.position = dicePositions[i];
            dice.transform.localRotation = Quaternion.identity;
        }
    }
    private void ChargeAllDice()
    {
        foreach (SingleDice dice in diceList)
        {
            dice.ChargePower();
            UpdatePowerSlider(dice);
        }
    }

    private void ThrowAllDice()
    {
        foreach (SingleDice dice in diceList)
        {
            dice.TossDice();
        }
    }

    private void UpdatePowerSlider(SingleDice dice)
    {
        if (powerSlider != null)
        {
            powerSlider.value = dice.currentPower; // Ustawienie wartości paska mocy na moc kości
        }
    }
    private void FillComboList(ComboList comboList)
    {
        var distinct = new List<string>();
        foreach(var combo in comboList.combos)
        {
            if (distinct.Contains(combo.name)) continue;
            distinct.Add(combo.name);
            var item = Instantiate(comboItemPrefab, combosRoot);
            item.text = combo.name;

        }
    }

}