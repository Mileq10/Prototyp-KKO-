using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textTMP;
    public Sprite Sprite {get; set;}
    public string Name { get; set;}
    public void UpdateValues()
    {
        image.sprite = Sprite;
        image.color = image.sprite == null ? Color.clear : Color.white;
        textTMP.text = Name;

    }
}
