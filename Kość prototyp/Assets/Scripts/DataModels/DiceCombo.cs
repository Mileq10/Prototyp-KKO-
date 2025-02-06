using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DiceCombo", menuName = "DicePoker/Combo", order = 1)]
public class DiceCombo : ScriptableObject
{
    public string comboName;          // Name of the combo (e.g., Pair, Three of a Kind)
    public int rank;                  // Ranking of the combo (higher is better)
    public ComboType comboType;       // Type of combo (Pair, Three of a Kind, etc.)
    public int requiredCount;         // Number of identical dice required

    public bool MatchesCombo(List<int> diceValues)
    {
        var grouped = diceValues.GroupBy(d => d).Select(g => g.Count()).ToList();

        switch (comboType)
        {
            case ComboType.Pair:
                return grouped.Any(count => count >= requiredCount);

            case ComboType.ThreeOfAKind:
                return grouped.Any(count => count >= requiredCount);

            case ComboType.FullHouse:
                return grouped.Contains(3) && grouped.Contains(2);

            case ComboType.FourOfAKind:
                return grouped.Any(count => count >= requiredCount);

            case ComboType.FiveOfAKind:
                return grouped.Any(count => count == 5);

            default:
                return false;
        }
    }
}

public enum ComboType
{
    Pair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}