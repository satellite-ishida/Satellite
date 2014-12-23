using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Input_Easy_Sat_Component : MonoBehaviour {

    public Toggle GPS_Toggle;
    public Toggle Weather_Toggle;

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
        if (t1.isOn && !t2.isOn)
        {
            type = "GPS";
        }
        else if (!t1.isOn && t2.isOn)
        {
            type = "Weather";
        }
        //チェックがどこにも付いていない場合
        else if (!t1.isOn && !t2.isOn)
        {
            //t1.isOn = true;
            return;
        }
        else{
            type = "GPS";
        }

        if ((Pegman = GameObject.Find("Pegman(Clone)")) != null)
        {
            /////↓森田定数を使った静止軌道
            // 緯度
            double latitude = Pegman.transform.localPosition.y;

            // 経度
            double longitude = Pegman.transform.localPosition.x;

            double s_omg0 = 269.9725;
            double M0 = 309.9023;
            double ET = 14343.49492826;
            // ズレ修正
            double M1 = 1.002737;
            // 位相?
            double L_omg0 = (36 + longitude);
            // 経度
            double i = latitude;
            double e = Math.Abs((0.0746703 / 40.5968) * latitude);

            GameManager.CreateNewSat(M0, M1, 0, e, i, s_omg0, L_omg0, ET ,type);
            //ペグマンをデストロイ
            Destroy(Pegman);
        }
    }
}
