using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public enum Type
    {
        Default,
        Advanced
    }
    public Type type;
    public bool isDown;

    PlayerAttackController PAC;

    private void Start()
    {
        PAC = FindObjectOfType<PlayerAttackController>();
    }

    public void Click()
    {
        if(type == Type.Default)
        {
            PAC.AttackTouchDefault();
        }
        if(type == Type.Advanced)
        {
            PAC.AttackTouchAdvanced();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }
}
