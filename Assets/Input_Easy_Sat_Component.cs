using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Input_Easy_Sat_Component : MonoBehaviour {

    enum SatelliteCost
    {
        GPS = 2,
        Weather = 3,
        BS = 0
    }

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

            GameObject sensor = Pegman.transform.FindChild("Pegman_Sensor").gameObject;
            SpriteRenderer sr = sensor.GetComponent<SpriteRenderer>();


            GameObject sensorbar = GameObject.Find("Sensor_Scrollbar");
            Scrollbar s = sensorbar.GetComponent<Scrollbar>();
            // スケール比
            float rate = 30 / 5;
            sensor.transform.localScale = new Vector3((s.value * 10 + 5) * rate, (s.value * 10 + 5) * rate, 1);

            print(sr.color);

            Color c = new Color(1f, 1f, 0f, 0.2f);
            sr.color = c;

        }
    }

    public void Submit()
    {
        GameObject Pegman;

        GameObject[] st = new GameObject[3];
        Toggle[] t = new Toggle[3];

        st[0] = GameObject.Find("GPS_Toggle");
         t[0] = st[0].transform.GetComponent<Toggle>();
        st[1] = GameObject.Find("Weather_Toggle");
         t[1] = st[1].GetComponent<Toggle>();
        st[2] = GameObject.Find("BS_Toggle");
         t[2] = st[2].GetComponent<Toggle>();

        String type = "";
        if (t[0].isOn)
        {
            type = "GPS";
        }
        else if (t[1].isOn)
        {
            type = "Weather";
        }
        else if (t[2].isOn) 
        {
            type = "BS";
        }

        if ((Pegman = GameObject.Find("Pegman(Clone)")) != null)
        {
            GameObject Cost = GameObject.Find("Cost");
            Text c = Cost.GetComponent<Text>();
            int cost = int.Parse(c.text);
            if (GameMaster.SubScore(cost))
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
                param[0] = (int)(s.value * 10 + 5);

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

                //// ボタンを非表示にしたい
                //GameObject sb = GameObject.Find("Submit_Button");
                //Button b = sb.GetComponent<Button>();

                //ペグマンをデストロイ
                Destroy(Pegman);
            }
        }
    }

    public void Set_Sensor_Performance()
    {
        GameObject sensorbar = GameObject.Find("Sensor_Scrollbar");
        Scrollbar s = sensorbar.GetComponent<Scrollbar>();
   //     GameMaster.SpanValue = s.value;

        GameObject value = GameObject.Find("SensorValue");
        Text v = value.GetComponent<Text>();
        v.text = (s.value*10).ToString();

        
        GameObject Pegman;
        if ((Pegman = GameObject.Find("Pegman(Clone)")) != null)
        {
            GameObject sensor = Pegman.transform.FindChild("Pegman_Sensor").gameObject;
            // スケール比
            float rate = 30 / 5;
            sensor.transform.localScale = new Vector3((s.value * 10 + 5) * rate, (s.value * 10 + 5) * rate, 1);
        }
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
        GameObject g3 = GameObject.Find("BS_Toggle");
        Toggle t3 = g3.GetComponent<Toggle>();

        if (t1.isOn)
        {
            cost += 2;
        }
        else if (t2.isOn)
        {
            cost += 3;
        }
        else if (t3.isOn) 
        {
            cost += 1;
        }

        GameObject Cost = GameObject.Find("Cost");
        Text c = Cost.GetComponent<Text>();
        c.text = cost.ToString();
    }

}
