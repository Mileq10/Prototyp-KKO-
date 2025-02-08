using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Enum for the six dice faces, with an underlying numeric value that reflects rank.
/*public enum DiceSide
{
    Eight = 1,
    Nine = 2,
    Ten = 3,
    Jack = 4,
    Queen = 5,
    King = 6
}*/

// Enum for the poker hand outcomes.
// The order of the enum values reflects their ranking.
public enum PokerHand
{
    Nothing = 0,
    OnePair = 1,
    TwoPairs = 2,
    ThreeOfAKind = 3,
    Straight = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7
}

public static class PokerDiceEvaluator
{
    /// <summary>
    /// Evaluates a dice hand and returns the corresponding PokerHand.
    /// Assumes exactly 5 dice are provided.
    /// </summary>
    /// <param name="dice">List of dice results (DiceSide values).</param>
    /// <returns>The PokerHand corresponding to the dice combination.</returns>
    public static PokerHand EvaluateHand(List<EDiceSide> dice)
    {
        if (dice == null || dice.Count != 5)
            throw new ArgumentException("Exactly 5 dice must be provided.");

        // Count the frequency of each dice face.
        Dictionary<EDiceSide, int> counts = new Dictionary<EDiceSide, int>();
        foreach (var die in dice)
        {
            if (counts.ContainsKey(die))
                counts[die]++;
            else
                counts[die] = 1;
        }

        // Check for Five of a Kind.
        if (counts.Values.Any(x => x == 5))
            return PokerHand.FiveOfAKind;

        // Check for Four of a Kind.
        if (counts.Values.Any(x => x == 4))
            return PokerHand.FourOfAKind;

        // Check for Full House (a three-of-a-kind and a pair).
        if (counts.Values.Contains(3) && counts.Values.Contains(2))
            return PokerHand.FullHouse;

        // Check for Straight: exactly five distinct dice that form a consecutive sequence.
        if (counts.Count == 5)
        {
            // Map each dice face to its integer value and sort them.
            var sortedValues = dice.Select(d => (int)d).OrderBy(v => v).ToList();
            // Possible straight sequences: 
            // Sequence 1: 1,2,3,4,5 (Eight, Nine, Ten, Jack, Queen)
            // Sequence 2: 2,3,4,5,6 (Nine, Ten, Jack, Queen, King)
            List<int> straight1 = new List<int> { 1, 2, 3, 4, 5 };
            List<int> straight2 = new List<int> { 2, 3, 4, 5, 6 };
            if (sortedValues.SequenceEqual(straight1) || sortedValues.SequenceEqual(straight2))
                return PokerHand.Straight;
        }

        // Check for Three of a Kind.
        if (counts.Values.Any(x => x == 3))
            return PokerHand.ThreeOfAKind;

        // Check for Two Pairs (exactly two distinct pairs).
        if (counts.Values.Count(x => x == 2) == 2)
            return PokerHand.TwoPairs;

        // Check for One Pair.
        if (counts.Values.Any(x => x == 2))
            return PokerHand.OnePair;

        // If no other hand is found, return Nothing.
        return PokerHand.Nothing;
    }

    /// <summary>
    /// Compares the evaluated hand against given thresholds.
    /// Returns:
    /// - -1 if hand is below the minimum threshold,
    /// -  0 if hand is equal or above minimum but below critical,
    /// -  1 if hand is equal or above the critical threshold.
    /// </summary>
    /// <param name="hand">The evaluated PokerHand.</param>
    /// <param name="minimum">Minimum threshold (PokerHand).</param>
    /// <param name="critical">Critical threshold (PokerHand).</param>
    /// <returns>-1, 0, or 1 based on the comparison.</returns>
    public static int EvaluateResult(PokerHand hand, PokerHand minimum, PokerHand critical)
    {
        if (hand < minimum)
            return -1;
        else if (hand == critical)
            return 1;
        else
            return 0;
    }
}