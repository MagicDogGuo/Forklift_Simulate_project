using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public static class JoyStickHelper
{

    // <summary>
    // 存储摇杆信息
    // </summary>
    public static JoyStickInfo infoEx = new JoyStickInfo();

    // <summary>
    // 本次更新后按钮按住的状态
    // </summary>
    public static int ButtonState;

    // <summary>
    // 本次更新后按钮抬起的状态
    // </summary>
    public static int ButtonUpState;

    // <summary>
    // 本次更新后按钮按下的状态
    // </summary>
    public static int ButtonDownState;

    // <summary>
    // 存储各轴的值
    // </summary>
    public static int[] AxisState = new int[6];

    // <summary>
    // 枚举按钮键值
    // </summary>
    public enum JoyStickButtonCode
    {
        Button01 = 0x1,
        Button02 = 0x2,
        Button03 = 0x4,
        Button04 = 0x8,
        Button05 = 0x10,
        Button06 = 0x20,
        Button07 = 0x40,
        Button08 = 0x80,
        Button09 = 0x100,
        Button10 = 0x200,
        Button11 = 0x400,
        Button12 = 0x800,
        Button13 = 0x1000,
        Button14 = 0x2000,
        Button15 = 0x4000,
        Button16 = 0x8000,
        Button17 = 0x10000,
        Button18 = 0x20000,
        Button19 = 0x40000,
        Button20 = 0x80000,
        Button21 = 0x100000,
        Button22 = 0x200000,
        Button23 = 0x400000,
        Button24 = 0x800000,
        Button25 = 0x1000000,
        Button26 = 0x2000000,
        Button27 = 0x4000000,
        Button28 = 0x8000000,
        Button29 = 0x10000000,
        Button30 = 0x20000000,
        Button31 = 0x40000000,
        Button32 = -0x80000000,
        none = -1
    }

    // <summary>
    // 枚举控制方向
    // </summary>
    public enum ControlDirection
    {
        X = 0,
        Y = 1,
        Z = 2,
        U = 3,
        V = 4,
        R = 5
    }

    // <summary>
    // 获取摇杆参数
    // </summary>
    // <param name="uJoyID"></param>
    // <param name="pji">摇杆信息，是个自定义类</param>
    // <returns></returns>
    [DllImport("winmm")]
    private static extern int joyGetPosEx(int uJoyID, ref JoyStickInfo pji);

    // <summary>
   // 从windowsAPI获取JoyStick的状态
    // </summary>
    // <returns></returns>
    public static int UpdateInfoEx()
    {
        if (infoEx.dwSize == 0)
        {
            infoEx.dwSize = Marshal.SizeOf(typeof(JoyStickInfo));
            infoEx.dwFlags = 0x00000080;
        }

        int beforeBtnState = ButtonState;

        int result = joyGetPosEx(0, ref infoEx);

        ButtonState = infoEx.dwButtons;
        Debug.Log("JoyStickHelperUpdate， infoEx.dwButtons" + infoEx.dwButtons);

        ButtonUpState = beforeBtnState & (beforeBtnState ^ ButtonState);

        ButtonDownState = (~beforeBtnState) & (beforeBtnState ^ ButtonState);

        AxisState[0] = infoEx.dwXpos;
        AxisState[1] = infoEx.dwYpos;
        AxisState[2] = infoEx.dwZpos;
        AxisState[3] = infoEx.dwUpos;
        AxisState[4] = infoEx.dwVpos;
        AxisState[5] = infoEx.dwRpos;

        return result;
    }

    // <summary>
    // 获取某一个轴按的动作大小,请获取前注意刷新
    // </summary>
    // <param name="axis">轴</param>
    // <returns></returns>
    public static int GetAxisPosition(ControlDirection axis)
    {
        return AxisState[Convert.ToInt32(axis)];
    }

    // <summary>
    // 获取某按钮是否按住
    // </summary>
    // <param name="code"></param>
    // <returns></returns>
    public static bool GetButton(JoyStickButtonCode code)
    {
        if (code == JoyStickButtonCode.none)
            return false;
        string buttonName = Enum.GetName(typeof(JoyStickButtonCode), code);
        int index = int.Parse(buttonName.Substring(buttonName.Length - 2));
        int mask = 1 << (index - 1);
        return (ButtonState & mask) > 0;
    }

    // <summary>
    // 获取某按钮是否抬起
    // </summary>
    // <param name="code"></param>
    // <returns></returns>
    public static bool GetUpButton(JoyStickButtonCode code)
    {
        if (code == JoyStickButtonCode.none)
            return false;
        string buttonName = Enum.GetName(typeof(JoyStickButtonCode), code);
        int index = int.Parse(buttonName.Substring(buttonName.Length - 2));
        int mask = 1 << (index - 1);
        return (ButtonUpState & mask) > 0;
    }

    // <summary>
    // 获取某按钮是否按下
    // </summary>
    // <param name="code"></param>
    // <returns></returns>
    public static bool GetDownButton(JoyStickButtonCode code)
    {
        if (code == JoyStickButtonCode.none)
            return false;
        string buttonName = Enum.GetName(typeof(JoyStickButtonCode), code);
        int index = int.Parse(buttonName.Substring(buttonName.Length - 2));
        int mask = 1 << (index - 1);
        return (ButtonDownState & mask) > 0;
    }

    // <summary>
    // 按键信息结构
    // </summary>
    public struct JoyStickInfo
    {
        // <summary>  
        // Size, in bytes, of this structure.  
        // </summary>  
        public int dwSize;
        // <summary>  
        // Flags indicating the valid information returned in this structure. Members that do not contain valid information are set to zero.  
        // </summary>  
        public int dwFlags;
        // <summary>  
        // Current X-coordinate.  
        // </summary>  
        public int dwXpos;
        // <summary>  
        // Current Y-coordinate.  
        // </summary>  
        public int dwYpos;
        // <summary>  
        // Current Z-coordinate.  
        // </summary>  
        public int dwZpos;
        // <summary>  
        // Current position of the rudder or fourth joystick axis.  
        // </summary>  
        public int dwRpos;
        // <summary>  
        // Current fifth axis position.  
        // </summary>  
        public int dwUpos;
        // <summary>  
        // Current sixth axis position.  
        // </summary>  
        public int dwVpos;
        // <summary>  
        // Current state of the 32 joystick buttons. The value of this member can be set to any combination of JOY_BUTTONn flags, where n is a value in the range of 1 through 32 corresponding to the button that is pressed.  
        // </summary>  
        public int dwButtons;
        // <summary>  
        // Current button number that is pressed.  
        // </summary>  
        public int dwButtonNumber;
        // <summary>  
        // Current position of the point-of-view control. Values for this member are in the range 0 through 35,900. These values represent the angle, in degrees, of each view multiplied by 100.  
        // </summary>  
        public int dwPOV;
        // <summary>  
        // Reserved; do not use.  
        // </summary>  
        public int dwReserved1;
        // <summary>  
        // Reserved; do not use.  
        // </summary>  
        public int dwReserved2;
    }
}