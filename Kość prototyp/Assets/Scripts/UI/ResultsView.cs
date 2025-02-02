using DataModels;
using DG.Tweening;
using NUnit.Framework;
using ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ResultsView : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textTMP;
    [SerializeField] private TextMeshProUGUI strongestComboTMP;
    [SerializeField] private CanvasGroup comboCanvasGroup;
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private float delayTime = 5.0f;

    private void Start()
    {
        diceManager.OnResults += UpdateView;
        diceManager.OnClearResults += UpdateView;
    }



    private void OnDestroy()
    {
        diceManager.OnResults -= UpdateView;
        diceManager.OnClearResults -= UpdateView;
    }

    private void UpdateView()
    {
        comboCanvasGroup.DOFade(0.0f, fadeTime);
    }
    private void UpdateView(List<DiceData> results, ComboList.Combo combo)
    {
        var builder = new StringBuilder();

        foreach (var diceData in results)
        {
            builder.Append(diceData.ToString());
            builder.Append(' ');
        }
        if(combo != null)
        comboCanvasGroup.DOFade(1.0f, fadeTime).OnComplete(() => { comboCanvasGroup.DOFade(0.0f, fadeTime).SetDelay(delayTime); });
        textTMP.SetText(builder.ToString());
        if(combo == null)
        {
            strongestComboTMP.SetText(string.Empty);
        }
        else
        {
            strongestComboTMP.SetText(combo.ToString());
        }
    }

    
}
