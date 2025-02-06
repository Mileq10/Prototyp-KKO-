using DataModels;
using DG.Tweening;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ScriptableObjects.ComboList;

public class ResultsView : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [Header("UI")]
    [SerializeField] private RectTransform root;
    [SerializeField] private ResultItem prefab;
    [SerializeField] private TextMeshProUGUI strongestComboTMP;
    [SerializeField] private Image strongestComboImage;
    [SerializeField] private CanvasGroup comboCanvasGroup;
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private float delayTime = 5.0f;

    private List<ResultItem> spawnedList = new();
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
    private void UpdateView(List<DiceData> results, Combo combo)
    {
        foreach (var diceData in results)
        {
            var item = Instantiate(prefab, root);
            spawnedList.Add(item);
            item.Name = diceData.Side.ToString();
            item.Sprite = null;
            item.UpdateValues();
        }
        if(combo != null)
        comboCanvasGroup.DOFade(1.0f, fadeTime).OnComplete(() => { comboCanvasGroup.DOFade(0.0f, fadeTime).SetDelay(delayTime); });
        


        if(combo == null)
        {
            strongestComboTMP.SetText(string.Empty);
            strongestComboImage.sprite = null;
        }
        else
        {
            strongestComboTMP.SetText(combo.ToString());
            strongestComboImage.sprite = combo.sprite;
        }
        strongestComboImage.color = combo == null ? Color.clear : Color.white;
    }

    
}
