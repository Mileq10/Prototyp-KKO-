using DG.Tweening;
using UnityEngine;

public class HelpView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeTime;
    [SerializeField] DiceManager diceManager;

    public void Show()
    {
        canvasGroup.DOFade(1.0f, fadeTime);
        diceManager.IsEnabled = false;
    }
    public void Hide()
    {
        canvasGroup.DOFade(0.0f, fadeTime);
        diceManager.IsEnabled = true;
    }
}
