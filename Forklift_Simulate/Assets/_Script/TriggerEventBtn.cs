using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerEventBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    EventTrigger eventTrigger;

    private void Start()
    {
        eventTrigger = this.gameObject.AddComponent<EventTrigger>();
    }
    public void OnItTrigger()
    {
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1.1f);
    }

    public void ExitItTrigger()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector2.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnItTrigger();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ExitItTrigger();
    }
}
