using UnityEngine;
using UnityEngine.EventSystems;

//把滑鼠鎖在中間，去對UI
//https://forum.unity.com/threads/custom-ui-cursor-position-and-ui-interaction.640435/
//只能有一個EventSystem

[RequireComponent(typeof(MyBaseInput))]
public class MyInputModule : StandaloneInputModule
{
    public CursorLockMode CursorLockMode = CursorLockMode.Locked;
    protected override void Start()
    {
        this.inputOverride = GetComponent<MyBaseInput>();
        base.Start();
    }
    public override void Process()
    {
        Cursor.lockState = CursorLockMode.None;

        base.Process();

        bool isDragging = false;
        foreach (PointerEventData p in this.m_PointerData.Values)
        {
            if (p.dragging)
            {
                isDragging = true;
                break;
            }
        }

        if (!isDragging)
            Cursor.lockState = this.CursorLockMode;

    }
}


