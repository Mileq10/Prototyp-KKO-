using System.Collections.Generic;
using System.Linq;
using DataModels;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private List<SingleDice> diceList = new List<SingleDice>();
    [SerializeField] private ComboList comboList;

    private bool isCharging;
    private bool wasTossed = false;

    public Slider powerSlider;
    [SerializeField] private Color lowPowerColor = Color.green;
    [SerializeField] private Color highPowerColor = Color.red;

    private void Start()
    {
        foreach (SingleDice dice in diceList)
        {
            dice.diceManager = this;
        }

        if (powerSlider != null)
        {
            powerSlider.minValue = diceList[0].minPower; // Ustawienie wartości minimalnej mocy
            powerSlider.maxValue = diceList[0].maxPower; // Ustawienie wartości maksymalnej mocy
            powerSlider.value = diceList[0].minPower; // Ustawienie początkowej wartości
        }
    }

    void Update()
    {
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
            List <DiceData> results = GatherAllResults();
            VerifyCombos(results);
        }
    }

    private void VerifyCombos(List<DiceData> results)
    {
        bool result = comboList.MatchesCombo(results.Select(x => x.Side).ToList());
        
    }

    private List<DiceData> GatherAllResults()
    {
        var results = new List<DiceData>();
    
        foreach (var dice in diceList)
        {
            var result = dice.GetResult();
            if(result==null) continue;
            results.Add(result);
        }
        return results;
    }

    private bool AreAllDiceResting()
    {
        return diceList.All(x => x.HasResult);
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
}