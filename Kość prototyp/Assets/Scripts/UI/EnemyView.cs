using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI minTMP;
    [SerializeField] private TextMeshProUGUI critTMP;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CombatResolver combatResolver;
    private RuntimeEnemy _enemy;
    private float fadeTime;

    public void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        combatResolver.OnNewEnemy += Show;
        combatResolver.OnEnemyDestroyed += (e) => Show(null);
        Show(null);
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
        minTMP.SetText(_enemy.EnemyData.MinimumRank.ToString());
        critTMP.SetText(_enemy.EnemyData.CriticalRank.ToString());
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
