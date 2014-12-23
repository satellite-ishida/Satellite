﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Input_Easy_Sat_Component : MonoBehaviour {

    void Start() 
    {

        Calc_Cost();
    }

    //ペグマンを置く
    public void Put_Locate()
    {
        if (GameObject.Find("Pegman(Clone)") == null)
        {
            GameObject prefab = (GameObject)Resources.Load("Prefabs/Pegman");
            GameObject Pegman = Instantiate(prefab) as GameObject;
        }
    }

    public void Submit()
    {
        GameObject Pegman;

        GameObject g1 = GameObject.Find("GPS_Toggle");
        Toggle t1 = g1.transform.GetComponent<Toggle>();
        GameObject g2 = GameObject.Find("Weather_Toggle");
        Toggle t2 = g2.GetComponent<Toggle>();

        String type = "";
        if (t1.isOn)
        {
            type = "GPS";
        }
        else if (t2.isOn)
        {
            type = "Weather";
        }

        if ((Pegman = GameObject.Find("Pegman(Clone)")) != null)
        {
            /////↓森田定数を使った静止軌道
            //// 緯度
            //double latitude = Pegman.transform.localPosition.y;

            //// 経度
            //double longitude = Pegman.transform.localPosition.x;

            //double s_omg0 = 269.9725;
            //double M0 = 309.9023;
            //double ET = 14343.49492826;
            //// ズレ修正
            //double M1 = 1.002737;
            //// 位相?
            //double L_omg0 = (36 + longitude);
            //// 経度
            //double i = latitude;
            //double e = Math.Abs((0.0746703 / 40.5968) * latitude);


            //衛星のパラメータ
            int[] param = new int[3];
            //センサ
            GameObject sensor = GameObject.Find("Sensor_Scrollbar");
            Scrollbar s = sensor.GetComponent<Scrollbar>();
            param[0] = (int)(s.value*10);

            GameObject g3 = GameObject.Find("QZS_Toggle");
            Toggle t3 = g3.transform.GetComponent<Toggle>();
            GameObject g4 = GameObject.Find("POS_Toggle");
            Toggle t4 = g4.GetComponent<Toggle>();
            GameObject g5 = GameObject.Find("MOS_Toggle");
            Toggle t5 = g5.GetComponent<Toggle>();

            // 経度
            double longitude = Pegman.transform.localPosition.x;
            // 緯度
            double latitude = Pegman.transform.localPosition.y;
            
            if (t3.isOn)
            {
                GameManager.CreateQZS(type, longitude, latitude);
            }
            else if (t4.isOn)
            {
                GameManager.CreatePOS(type, longitude, latitude);
            }
            else if (t5.isOn)
            {
                GameManager.CreateMOS(type, longitude, latitude);
            }

            //GameManager.CreateNewSat(M0, M1, 0, e, i, s_omg0, L_omg0, ET ,type,param);
            //ペグマンをデストロイ
            Destroy(Pegman);
        }
    }

    public void Set_Sensor_Performance()
    {
        GameObject sensor = GameObject.Find("Sensor_Scrollbar");
        Scrollbar s = sensor.GetComponent<Scrollbar>();
   //     GameMaster.SpanValue = s.value;

        GameObject value = GameObject.Find("SensorValue");
        Text v = value.GetComponent<Text>();
        v.text = (s.value*10).ToString();
    }

    public void Set_Body_Strength() 
    {
        GameObject body = GameObject.Find("Body_Scrollbar");
        Scrollbar b = body.GetComponent<Scrollbar>();
        //     GameMaster.SpanValue = s.value;

        GameObject value = GameObject.Find("BodyValue");
        Text v = value.GetComponent<Text>();
        v.text = (b.value * 10).ToString();
    
    
    }

    public void Calc_Cost() 
    {
        int cost = 0;

        //衛星のパラメータのコスト
        GameObject sensor = GameObject.Find("Sensor_Scrollbar");
        Scrollbar s = sensor.GetComponent<Scrollbar>();

        GameObject body = GameObject.Find("Body_Scrollbar");
        Scrollbar b = body.GetComponent<Scrollbar>();

        cost = (int)(s.value * 10) + (int)(b.value * 10);


        //衛星の種類によるコスト
        GameObject g1 = GameObject.Find("GPS_Toggle");
        Toggle t1 = g1.transform.GetComponent<Toggle>();
        GameObject g2 = GameObject.Find("Weather_Toggle");
        Toggle t2 = g2.GetComponent<Toggle>();

        if (t1.isOn)
        {
            cost += 2;
        }
        else if (t2.isOn)
        {
            cost += 3;
        }

        GameObject Cost = GameObject.Find("Cost");
        Text c = Cost.GetComponent<Text>();
        c.text = cost.ToString();
    }

}
