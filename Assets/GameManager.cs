using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

class GameManager : MonoBehaviour
{

    private GameObject _charactor;
    private GameObject _charactor2;
    private GameObject _charactor3;

    private DateTime Global_Time = new DateTime(2000, 1, 1, 0, 0, 0);

    /// <summary>
    /// 都市管理用ハッシュマップ(key:都市名,value:GameObject)
    /// </summary>
    private Dictionary<string, GameObject> Citydict = new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start()
    {
        GameMaster.Map = new Map();

       

        //都市オブジェクトの作成
        GameObject prefab2 = (GameObject)Resources.Load("Prefabs/point");

        for (int i = -180; i < 180; i++)
        {
            for (int j = -89; j < 90; j++)
            {
                if (!String.IsNullOrEmpty(GameMaster.Map[i, j].City))
                {
                    GameObject point = Instantiate(prefab2) as GameObject;
                    point.transform.position = new Vector3(i, j, 1);

                    if (!Citydict.ContainsKey(GameMaster.Map[i, j].City))
                    {
                        Citydict.Add(GameMaster.Map[i, j].City, point);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameMaster.Map.Satellite_Update();


        Citydict["東京"].GetComponent<CityComponent>().SatelliteNum();
        

        //グローバルタイムの更新と表示
        Global_Time = Global_Time.AddMinutes(Math.Floor(GameMaster.GetSpanValue()));
        //time = time.AddSeconds(10*s.value);
        GameObject date = GameObject.Find("Date");
        Text t = date.GetComponent<Text>();
        t.text = Global_Time.ToString();
    }

    //新しい衛星の作成(引数String)
    public static void CreateNewSat(String M0, String M1, String M2, String e, String i, String s_omg, String L_omg, String ET,String type)
    {
        double _M0 = double.Parse(M0);
        double _M1 = double.Parse(M1);
        double _M2 = double.Parse(M2);
        double _e = double.Parse(e);
        double _i = double.Parse(i);
        double _s_omg = double.Parse(s_omg);
        double _L_omg = double.Parse(L_omg);
        double _ET = double.Parse(ET);

        GameObject prefab = null;
        GameObject satellite = null;
        SatelliteComponent component = null;
        if (type.Equals("Weather"))
        {
            prefab = (GameObject)Resources.Load("Prefabs/QZStest");
            satellite = Instantiate(prefab) as GameObject;
            component = satellite.GetComponent<Weather_Satellite>();
        }
        else if (type.Equals("GPS"))
        {
            prefab = (GameObject)Resources.Load("Prefabs/GPS");
            satellite = Instantiate(prefab) as GameObject;
            component = satellite.GetComponent<GPS_Satellite>();
        }
       
        GameMaster.AddSatelliteList(satellite);

        component.i = _i;
        component.e = _e;
        component.s_omg0 = _s_omg;
        component.M0 = _M0;
        component.ET = _ET;
        component.L_omg0 = _L_omg;
    }

    //新しい衛星の作成(引数double)
    public static void CreateNewSat(double M0, double M1, double M2, double e, double i, double s_omg, double L_omg, double ET,String type)
    {

        GameObject prefab = null;
        GameObject satellite = null;
        SatelliteComponent component = null;
        if (type.Equals("Weather"))
        {
            prefab = (GameObject)Resources.Load("Prefabs/QZStest");
            satellite = Instantiate(prefab) as GameObject;
            component = satellite.GetComponent<Weather_Satellite>();
        }
        else if (type.Equals("GPS"))
        {
            prefab = (GameObject)Resources.Load("Prefabs/GPS");
            satellite = Instantiate(prefab) as GameObject;
            component = satellite.GetComponent<GPS_Satellite>();
        }

        GameMaster.AddSatelliteList(satellite);

        component.i = i;
        component.e = e;
        component.s_omg0 = s_omg;
        component.M0 = M0;
        component.M1 = M1;
        component.M2 = M2;
        component.ET = ET;
        component.L_omg0 = L_omg;
    }

    //新しい衛星の作成(衛星の指定なし)
    public static void CreateNewSat(double M0, double M1, double M2, double e, double i, double s_omg, double L_omg, double ET)
    {

        GameObject prefab = null;
        prefab = (GameObject)Resources.Load("Prefabs/QZStest");
       
        GameObject satellite = Instantiate(prefab) as GameObject;
        SatelliteComponent component = satellite.GetComponent<Weather_Satellite>();

        GameMaster.AddSatelliteList(satellite);

        component.i = i;
        component.e = e;
        component.s_omg0 = s_omg;
        component.M0 = M0;
        component.M1 = M1;
        component.M2 = M2;
        component.ET = ET;
        component.L_omg0 = L_omg;
    }


    /*
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
                    if (string.Compare(GameMaster.Map[i, j].City, null) != 0)
                    {
                        citynum++;
                    }
                    if (GameMaster.Map[i, j].Land)
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
    */

}
