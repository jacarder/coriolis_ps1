using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class DialogButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text text;
    private Color originalTextColor;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        originalTextColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = new Color(255, 219, 212);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originalTextColor;
    }
}
