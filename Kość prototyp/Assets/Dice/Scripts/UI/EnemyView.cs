using DG.Tweening;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image minTMP;
    [SerializeField] private Image critTMP;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CombatResolver combatResolver;
    [SerializeField] private ComboList comboList;
    private RuntimeEnemy _enemy;
    private float fadeTime;

    public void Awake()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        combatResolver.OnNewEnemy += Show;
        combatResolver.OnEnemyDestroyed += (e) => Show(null);
    }
    public void OnDestroy()
    {
        combatResolver.OnNewEnemy -= Show;
    }
    public void Show(RuntimeEnemy enemy)
    {
        _enemy = enemy;
        if(_enemy == null)
        {
            canvasGroup.DOFade(0.0f, fadeTime).SetEase(Ease.InOutCubic);
            return;
        }
        canvasGroup.DOFade(1.0f, fadeTime).SetEase(Ease.InOutCubic);
        minTMP.sprite = (comboList.GetSprite(_enemy.EnemyData.MinimumRank));
        critTMP.sprite = (comboList.GetSprite(_enemy.EnemyData.CriticalRank));
        nameTMP.SetText(_enemy.EnemyData.Name.ToString());
    }

    private void UpdateView()
    {
        slider.value = _enemy.HealthModel.GetPercentage();
    }
    private void LateUpdate()
    {
        if (_enemy == null)
        {
            return;
        }
        UpdateView();
    }


}
