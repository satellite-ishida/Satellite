using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    private GameObject _charactor;
    private GameObject _charactor2;
    private GameObject _charactor3;

    private static Map map;

    // Use this for initialization
    void Start()
    {
        map = new Map();

        /////////////////////↓デバック用
        // キャラクターを取得する
        this._charactor = GameObject.Find("Sattellite");
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

            map.SatelliteObject = satellite;
            //map.Satellite = component;
  
            component.ID = i;
            // ////真の衛星軌道
            component.M1 = 14.117117471;
            component.i = 99.0090;
            component.e = 0.0008546;
            component.s_omg0 = 223.1686;
            component.M0 = 136.8816;
            component.ET = 97320.90946019;
            component.L_omg0 = 272.6745;

            ////真の真ののの衛星軌道
            ////component.M1 = 1.00287879;
            //component.i = 40.5968;
            //component.e = 0.0746703;
            //component.s_omg0 = 269.9725;
            //component.M0 = 309.9023;
            //component.ET = 14343.49492826;
            //component.L_omg0 = 172.75;

            //component.i = 40.5968;
            //component.e = 0.0746703;
            //component.s_omg0 = 269.9725;
            //component.M0 = 309.9023 + i;
            //component.ET = 14343.49492826;
            //component.L_omg0 = 172.75;
            //// ズレ修正
            //component.M1 = 1.002737;
            //// 位相?
            //component.L_omg0 = (36 + longitude) - i;
            //// 経度
            //component.i = latitude;
            //component.e = Math.Abs((0.0746703 / 40.5968) * latitude);
            
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

        ///////////////////↑デバック用ここまで
    }

    // Update is called once per frame
    void Update()
    {
        map.Satellite_Update();
    }

    public static void CreateNewSat(String M0, String M1, String M2, String e, String i, String s_omg, String L_omg, String ET)
    {
        double _M0 = double.Parse(M0);
        double _M1 = double.Parse(M1);
        double _M2 = double.Parse(M2);
        double _e = double.Parse(e);
        double _i = double.Parse(i);
        double _s_omg = double.Parse(s_omg);
        double _L_omg = double.Parse(L_omg);
        double _ET = double.Parse(ET);

        GameObject prefab = (GameObject)Resources.Load("Prefabs/QZS");
        GameObject satellite = Instantiate(prefab) as GameObject;
        SatelliteComponent component = satellite.GetComponent<SatelliteComponent>();

        map.SatelliteObject = satellite;

        component.ID = GameMaster.Get_Satellite_ID();
        component.i = _i;
        component.e = _e;
        component.s_omg0 = _s_omg;
        component.M0 = _M0;
        component.ET = _ET;
        component.L_omg0 = _L_omg;
    }

    public static void CreateNewSat(double M0, double M1, double M2, double e, double i, double s_omg, double L_omg, double ET)
    {

        GameObject prefab = (GameObject)Resources.Load("Prefabs/QZS");
        GameObject satellite = Instantiate(prefab) as GameObject;
        SatelliteComponent component = satellite.GetComponent<SatelliteComponent>();

        map.SatelliteObject = satellite;

        component.ID = GameMaster.Get_Satellite_ID();
        component.i = i;
        component.e = e;
        component.s_omg0 = s_omg;
        component.M0 = M0;
        component.ET = ET;
        component.L_omg0 = L_omg;
    }
}
