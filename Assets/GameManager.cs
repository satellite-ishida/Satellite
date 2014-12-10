using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    private GameObject _charactor;
    private GameObject _charactor2;
    private GameObject _charactor3;


    //打ち上げ時間(軌道要素の元期より後の時間にすること)
    private DateTime observe_time = new DateTime(2014, 12, 31, 23, 55, 28);

    //private DateTime observe_time = new DateTime(2009, 1, 1, 0, 0, 0);

    // Use this for initialization
    void Start()
    {
        // キャラクターを取得する
        this._charactor = GameObject.Find("Satellite");
        this._charactor2 = GameObject.Find("Satellite2");
        this._charactor3 = GameObject.Find("Satellite3");


        GameObject prefab = (GameObject)Resources.Load("Prefabs/QZS");

        // 緯度
        double latitude = _charactor3.transform.localPosition.y;


        // 経度
        double longitude = _charactor3.transform.localPosition.x;

        // 衛星の個数
        int n = 5;

        for (int i = 0; i < 360; i += (360 / n))
        {
            GameObject satellite = Instantiate(prefab) as GameObject;
            SatelliteComponent component = satellite.GetComponent<SatelliteComponent>();
            component.TIME = observe_time;

            // ////真の衛星軌道
            //component.M1 = 1.002935;
            //component.i = 40.6301;
            //component.e = 0.0756537;
            //component.s_omg0 = 269.8983;
            //component.M0 = 236.1610;
            //component.ET = 13155.82828148;
            //component.L_omg0 = 180.7852;

            ////真の真ののの衛星軌道
            ////component.M1 = 1.00287879;
            //component.i = 40.5968;
            //component.e = 0.0746703;
            //component.s_omg0 = 269.9725;
            //component.M0 = 309.9023;
            //component.ET = 14343.49492826;
            //component.L_omg0 = 172.75;

            component.i = 40.5968;
            component.e = 0.0746703;
            component.s_omg0 = 269.9725;
            component.M0 = 309.9023 + i;
            component.ET = 14343.49492826;
            component.L_omg0 = 172.75;
            // ズレ修正
            component.M1 = 1.002737;
            // 位相?
            component.L_omg0 = (36 + longitude) - i;
            // 経度
            component.i = latitude;
            component.e = Math.Abs((0.0746703 / 40.5968) * latitude);

        DkLogEditor.Open();
        }

        // 古の愉快なパラメータ達

        //SatelliteComponent component = this._charactor.AddComponent<SatelliteComponent>();
        //component.M0 = 30.0;
        //component.M1 = 1.002744;
        //component.M2 = 0.0;
        //component.e = 0.075;
        //component.i = 43.0;
        //component.s_omg0 = 270.0;
        //component.L_omg0 = 15.0;
        //component.ET = 08075.0;
        //SatelliteComponent component = this._charactor.GetComponent<SatelliteComponent>();
        //component.TIME = observe_time;

        //SatelliteComponent component2 = this._charactor2.AddComponent<SatelliteComponent>();
        //component2.M0 = 150.0;
        //component2.M1 = 1.002744;
        //component2.M2 = 0.0;
        //component2.e = 0.075;
        //component2.i = 43.0;
        //component2.s_omg0 = 270.0;
        //component2.L_omg0 = 255.0;
        //component2.ET = 08075.0;
        //component2.TIME = observe_time;

        //SatelliteComponent component3 = this._charactor3.AddComponent<SatelliteComponent>();
        //component3.M0 = 270.0;
        //component3.M1 = 1.002744;
        //component3.M2 = 0.0;
        //component3.e = 0.075;
        //component3.i = 43.0;
        //component3.s_omg0 = 270.0;
        //component3.L_omg0 = 135.0;
        //component3.ET = 08075.0;
        //component3.TIME = observe_time;
    }

    // Update is called once per frame
    void Update()
    {
        DkLog.V("verbose log", false);
        DkLog.D("debug log", false);
        DkLog.I("information log", false);
        DkLog.W("warning log", false);
        DkLog.E("error log", false);
    }
}
