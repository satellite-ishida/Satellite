﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Input_Satellite_Component : MonoBehaviour
{
    public InputField field1;
    public InputField field2;
    public InputField field3;
    public InputField field4;
    public InputField field5;
    public InputField field6;
    public InputField field7;
    public InputField field8;

    public void OnClick()
    {

        String f1 = field1.text;
        String f2 = field2.text;
        String f3 = field3.text;
        String f4 = field4.text;
        String f5 = field5.text;
        String f6 = field6.text;
        String f7 = field7.text;
        String f8 = field8.text;

        GameManager.CreateNewSat(f1, f2, f3, f4, f5, f6, f7, f8);
    }
 
}