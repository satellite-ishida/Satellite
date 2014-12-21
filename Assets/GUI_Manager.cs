﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GUI_Manager : MonoBehaviour
{

    //スライダーの値が変更されたとき
    public void Set_Slider_Event()
    {
        GameObject span = GameObject.Find("Span");
        Slider s = span.GetComponent<Slider>();
        GameMaster.SetSpanValue(s.value);

        GameObject spanText = GameObject.Find("SpanText");
        Text st = spanText.GetComponent<Text>();
        st.text = "Update Time : " + s.value.ToString() + " Hours";
    }
    //アドバンスパネルを前面に
    public void Set_Advanced_Panel()
    {
        GameObject g = GameObject.Find("Input_SatComp_Panel");
        g.transform.SetAsLastSibling();
        //ペグマンをですとろい
        GameObject Pegman = GameObject.Find("Pegman(Clone)");
        Destroy(Pegman);
    }
    //イージーパネルを前面に
    public void Set_Easy_Panel()
    {
        GameObject g = GameObject.Find("Input_Easy_Sat_Panel");
        g.transform.SetAsLastSibling();
    }
}
    