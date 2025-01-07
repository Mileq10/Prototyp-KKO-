using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiceManager : MonoBehaviour
{
    [SerializeField] private List<SingleDice> diceList = new List<SingleDice>();
    private Dictionary<SingleDice, int> diceResults = new Dictionary<SingleDice, int>();

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
    }

    public void RecordDiceResult(SingleDice dice, int value)
    {
        if (diceResults.ContainsKey(dice))
        {
            diceResults[dice] = value;
        }
        else
        {
            diceResults.Add(dice, value);
        }

        Debug.Log($"Dice {dice.name} rolled a value of {value}");
    }

    public void PrintAllResults()
    {
        Debug.Log("All Dice Results:");
        foreach (var entry in diceResults)
        {
            Debug.Log($"Dice {entry.Key.name} rolled a value of {entry.Value}");
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
}