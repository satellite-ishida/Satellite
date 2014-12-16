using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private GameObject _charactor;
    private GameObject _charactor2;
    private GameObject _charactor3;

    private static Map map;

    private DateTime Global_Time = new DateTime(2000, 1, 1, 0, 0, 0);

    // Use this for initialization
    void Start()
    {
        map = new Map();

        /////////////////////↓デバック用
        // キャラクターを取得する
        //this._charactor = GameObject.Find("Sattellite");
        //this._charactor2 = GameObject.Find("Satellite2");
        //this._charactor3 = GameObject.Find("Satellite3");

        GameObject prefab = (GameObject)Resources.Load("Prefabs/QZStest");

        //// 緯度
        //double latitude = _charactor3.transform.localPosition.y;

        //// 経度
        //double longitude = _charactor3.transform.localPosition.x;

        // 衛星の個数
        int n = 1;

        for (int i = 0; i < 360; i += (360 / n))
        {
            GameObject satellite = Instantiate(prefab) as GameObject;
            SatelliteComponent component = satellite.GetComponent<SatelliteComponent>();

            GameMaster.AddSatelliteList(satellite);
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
        GameObject prefab2 = (GameObject)Resources.Load("Prefabs/point");

        for (int i = -180; i < 180; i++)
        {
            for (int j = -89; j < 90; j++)
            {
                if (string.Compare(map[i, j].City, null) != 0)
                {
                    GameObject point = Instantiate(prefab2) as GameObject;
                    point.transform.position = new Vector3(i, j, 1);
                }
            }
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

        //グローバルタイムの更新と表示
        Global_Time = Global_Time.AddMinutes(Math.Floor(GameMaster.GetSpanValue()));
        //time = time.AddSeconds(10*s.value);
        GameObject date = GameObject.Find("Date");
        Text t = date.GetComponent<Text>();
        t.text = Global_Time.ToString();
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

        GameObject prefab = (GameObject)Resources.Load("Prefabs/QZStest");
        GameObject satellite = Instantiate(prefab) as GameObject;
        SatelliteComponent component = satellite.GetComponent<SatelliteComponent>();

        GameMaster.AddSatelliteList(satellite);

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

        GameObject prefab = (GameObject)Resources.Load("Prefabs/QZStest");
        GameObject satellite = Instantiate(prefab) as GameObject;
        SatelliteComponent component = satellite.GetComponent<SatelliteComponent>();

        GameMaster.AddSatelliteList(satellite);

        component.ID = GameMaster.Get_Satellite_ID();
        component.i = i;
        component.e = e;
        component.s_omg0 = s_omg;
        component.M0 = M0;
        component.M1 = M1;
        component.M2 = M2;
        component.ET = ET;
        component.L_omg0 = L_omg;
    }

    public static void CalcScore(GameObject g)
    {
        int citynum = 0;
        int landnum = 0;
        int seanum = 0;


        SatelliteComponent satellite = g.GetComponent<SatelliteComponent>();

        GameObject sensor = g.transform.FindChild("Sensor").gameObject;

        citynum = 0;
        landnum = 0;
        seanum = 0;

        int x = (int)g.transform.position.x;
        int y = (int)g.transform.position.y;
        int a = (int)((sensor.transform.lossyScale.x) * 0.09);
        int b = (int)((sensor.transform.lossyScale.y) * 0.09);


        for (int i = x - a; i <= x + a; i++)
        {
            for (int j = y - b; j < y + b; j++)
            {
                //とりあえず円で
                //   if ((x - i) * (x - i) + (y - j) * (y - j) <= a * a)
                if (((i - x) * (i - x)) * (b * b) + ((j - y) * (j - y)) * (a * a) <= a * a * b * b)
                {
                    if (string.Compare(map[i, j].City, null) != 0)
                    {
                        citynum++;
                    }
                    if (map[i, j].Land)
                    {
                        landnum++;
                    }
                    else
                    {
                        seanum++;
                    }
                }
            }
        }
        //ゲームマスターにスコアの通知
        GameMaster.AddScore(citynum);
    }

    private Vector3 start;


    void OnMouseDown()
    {
        //GameObject prefab3 = (GameObject)Resources.Load("Prefabs/base_station");


        //float x = Input.mousePosition.x;
        //float y = Input.mousePosition.y;

        //start = new Vector3(x, y, 0);


        //double longitude = Math.Round(.x);
        //double latitude = Math.Round(mousePos.y);

        //if (!map[longitude, latitude].GS && map.Set_BaseStation((int)longitude, (int)latitude))
        //{
        //    GameObject base_sation = Instantiate(prefab3) as GameObject;
        //    base_sation.transform.position = new Vector3((int)longitude, (int)latitude, 1);
        //}

        //print(map[longitude,latitude].GS);
        //print((int)longitude + "," + (int)latitude);

        //  print(map[(int)longitude,(int)latitude].City);
    }

    void OnMouseUp()
    {
        //    float x = Input.mousePosition.x;
        //    float y = Input.mousePosition.y;

        //    Vector3 currentPosition = new Vector3(x, y, 1);


        //    GameObject prefab3 = (GameObject)Resources.Load("Prefabs/base_station");

        //    GameObject aa = Instantiate(prefab3) as GameObject;
        //    aa.transform.position = Camera.main.ScreenToWorldPoint(currentPosition / 2 + start / 2);
        //    aa.transform.localScale = (currentPosition - start) * 2;

        //    print(currentPosition.x - start.x);

        //    double a = (currentPosition.x - start.x) / 4;
        //    double b = (currentPosition.y - start.y) / 4;
        //    Vector3 O = Camera.main.ScreenToWorldPoint(currentPosition / 2 + start / 2);

        //    int citynum = 0;
        //    int landnum = 0;
        //    int seanum = 0;

        //    for (int i = (int)O.x - (int)Math.Abs(currentPosition.x - start.x) / 4; i < (int)O.x + (int)Math.Abs(currentPosition.x - start.x); i++)
        //    {
        //        for (int j = (int)O.y - (int)Math.Abs(currentPosition.y - start.y) / 4; j < (int)O.y + (int)Math.Abs(currentPosition.y - start.y) / 4; j++)
        //        {
        //            if (((i - O.x) * (i - O.x)) / (a * a) + ((j - O.y) * (j - O.y)) / (b * b) <= 1)
        //            {
        //                if (string.Compare(map[i, j].City, null) != 0)
        //                {
        //                    citynum++;
        //                }
        //                if (map[i, j].Land)
        //                {
        //                    landnum++;
        //                }
        //                else
        //                {
        //                    seanum++;
        //                }
        //            }
        //        }

        //    }

        //    print(citynum + "," + landnum + "," + seanum);
    }
}
