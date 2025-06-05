using DataModels;
using DG.Tweening;
using ScriptableObjects;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsView : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private ComboList comboList;
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

        if (spawnedList.Count > 0)
        {
            for (var i = spawnedList.Count - 1; i >= 0; i--)
            {
                spawnedList[i].gameObject.SetActive(false);
                Destroy(spawnedList[i]);
                spawnedList.RemoveAt(i);
            }
        }
    }
    private void UpdateView(List<DiceData> results, PokerHand hand)
    {
        if(spawnedList.Count > 0)
        {
            for (var i = spawnedList.Count - 1; i >= 0; i--)
            {
                spawnedList[i].gameObject.SetActive(false);
                Destroy(spawnedList[i]);
                spawnedList.RemoveAt(i);
            }
        }

        foreach (var diceData in results)
        {
            var item = Instantiate(prefab, root);
            spawnedList.Add(item);
            item.Name = diceData.Side.ToString();
            item.Sprite = null;
            item.UpdateValues();
        }
        
        comboCanvasGroup.DOFade(1.0f, fadeTime).OnComplete(() => { comboCanvasGroup.DOFade(0.0f, fadeTime).SetDelay(delayTime); });
        


       
        strongestComboTMP.SetText(comboList.GetName(hand));
        strongestComboImage.sprite = comboList.GetSprite(hand);
        
        strongestComboImage.color = Color.white;
    }

    
}
