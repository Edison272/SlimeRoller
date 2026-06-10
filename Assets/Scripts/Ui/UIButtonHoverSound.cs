using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public MainMenu menu;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (menu != null)
        {
            menu.PlayHoverSound();
        }
    }
}