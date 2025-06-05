using System;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private Camera currentCamera;
    [SerializeField] private CombatResolver combatResolver;
    [Header("Config")]
    [SerializeField] private List<SingleDice> diceList = new List<SingleDice>();
    [SerializeField] private ComboList comboList;
    [Header("UI")]
    [SerializeField] private Slider powerSlider;
    [SerializeField] private RectTransform combosRoot;
    [SerializeField] private TextMeshProUGUI comboItemPrefab;

    private bool _isCharging;
    private bool _wasTossed = false;
    private bool _combatResolved;
    private bool _isEnabled = true;

    private List<Vector3> _dicePositions = new List<Vector3>();
    private bool _resultsSent = false;

    public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }
    public event Action OnClearResults;

    public event Action<List<DiceData>, PokerHand> OnResults;

    private void Start()
    {
        Init();
        powerSlider.gameObject.SetActive(false);
        if (powerSlider != null)
        {
            powerSlider.minValue = diceList[0].minPower; // Ustawienie wartości minimalnej mocy
            powerSlider.maxValue = diceList[0].maxPower; // Ustawienie wartości maksymalnej mocy
            powerSlider.value = diceList[0].minPower; // Ustawienie początkowej wartości
        }

        FillComboList(comboList);
        combatResolver.Init();
    }

    private void Init()
    {
        foreach (SingleDice dice in diceList)
        {
            dice.diceManager = this;
            _dicePositions.Add(dice.transform.position);
        }
    }

    void Update()
    {
        if (!IsEnabled)
        {
            return;
        }

        if (_combatResolved == true)
        {
            if (Input.GetKey(KeyCode.R))
            {
                ResetAll();
            }
            return;
        }
        if (Input.GetKey(KeyCode.Space) && !_wasTossed)
        {
            _isCharging = true;
            ChargeAllDice();
            powerSlider.gameObject.SetActive(true);
            return;
        }

        if (Input.GetKeyUp(KeyCode.Space) && _isCharging && !_wasTossed)
        {
            _isCharging = false;
            ThrowAllDice();
            _wasTossed = true;
            return;
        }
        if (!AreAllDiceResting())
        {
            return;
        }

        powerSlider.gameObject.SetActive(false);


        if (Input.GetKey(KeyCode.R) && CanReroll())
        {
            ResetAll();
            combatResolver.SetReroll(false);

            return;
        }
        HandleAcceptResult();
        if (!_resultsSent)
        OnGatheredResults();

    }

    private void OnGatheredResults()
    {
        List<DiceData> results = GatherAllResults();
        var damage = VerifyCombos(results, combatResolver.CurrentEnemyData, out var combo);
        OnResults?.Invoke(results, combo);
        _resultsSent = true;
    }

    private void HandleAcceptResult()
    {
        if (Input.GetKey(KeyCode.F))
        {
            List<DiceData> results = GatherAllResults();
            var damage = VerifyCombos(results, combatResolver.CurrentEnemyData, out var combo);
            combatResolver.ResolveCombat(damage);
            _combatResolved = true;
            return;
        }
    }

    private bool CanReroll()
    {
        return combatResolver.CanReroll();
    }

    private void ResetAll()
    {
        ResetAllDice();
        powerSlider.value = powerSlider.minValue;
        OnClearResults?.Invoke();
        _combatResolved = false;
        _isCharging = false;
        _wasTossed = false;
        _resultsSent = false;
    }
    


    private int VerifyCombos(List<DiceData> results, EnemyData enemyData, out PokerHand pokerHand)
    {
        /*return comboList.MatchesCombo(results.Select(x => x.Side).ToList(), enemyData.MinimumRank, enemyData.CriticalRank, out combo);*/
        var rolled = results.Select(x => x.Side).ToList();
        pokerHand = PokerDiceEvaluator.EvaluateHand(rolled);
        return PokerDiceEvaluator.EvaluateResult(pokerHand, enemyData.MinimumRank, enemyData.CriticalRank);
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
            dice.transform.position = _dicePositions[i];
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
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 throwDirection = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            throwDirection = (hit.point - transform.position).normalized;
        }
       
        foreach (SingleDice dice in diceList)
        {
            dice.TossDice(throwDirection);
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
        var enumValues = (PokerHand[])Enum.GetValues(typeof(PokerHand));
        foreach (var hand in enumValues)
        {
            var item = Instantiate(comboItemPrefab, combosRoot);
            item.text = comboList.GetName(hand);

        }
    }

}