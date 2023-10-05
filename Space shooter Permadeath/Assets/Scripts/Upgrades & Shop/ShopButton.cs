using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string description;
    public Text tooltipWindow;
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        tooltipWindow.text = description;
        tooltipWindow.enabled = true;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        tooltipWindow.enabled = false;
    }
}