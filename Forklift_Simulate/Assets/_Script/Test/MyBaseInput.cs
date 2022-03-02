﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyBaseInput : BaseInput
{
    public Vector2 CursorPosition = Vector2.zero;

    public override Vector2 mousePosition
    {
        get { return this.CursorPosition; }
    }
}
