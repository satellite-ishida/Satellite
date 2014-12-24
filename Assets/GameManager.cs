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

    /// <summary>
    /// 都市管理用ハッシュマップ(key:都市名,value:GameObject)
    /// </summary>
    private Dictionary<string, GameObject> Citydict = new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start()
    {
        GameMaster.Map = new Map();


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
        //    GameObject satellite = Instantiate(prefab) as GameObject;
        ////    SatelliteComponent component = satellite.GetComponent<SatelliteComponent>();

        //    SatelliteComponent component = satellite.GetComponent<Weather_Satellite>();
        //    GameMaster.AddSatelliteList(satellite);
        //    //map.Satellite = component;
            
        //    component.ID =GameMaster.Get_Satellite_ID();
        //    // ////真の衛星軌道
        //    component.M1 = 14.117117471;
        //    component.i = 99.0090;
        //    component.e = 0.0008546;
        //    component.s_omg0 = 223.1686;
        //    component.M0 = 136.8816;
        //    component.ET = 97320.90946019;
        //    component.L_omg0 = 272.6745;
            
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


        //都市オブジェクトの作成
        GameObject prefab2 = (GameObject)Resources.Load("Prefabs/point");
        GameObject city = GameObject.Find("City");

        for (int i = -180; i < 180; i++)
        {
            for (int j = -89; j < 90; j++)
            {
                if (!String.IsNullOrEmpty(GameMaster.Map[i, j].City))
                {
                    GameObject point = Instantiate(prefab2) as GameObject;                    
                    point.transform.position = new Vector3(i, j, 1);
                    point.transform.parent = city.transform;

                    if (!Citydict.ContainsKey(GameMaster.Map[i, j].City))
                    {
                        Citydict.Add(GameMaster.Map[i, j].City, point);
                    }
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
        StartCoroutine("ManagerCoroutines");
    }

    private int saveday = 0;

    //コルーチン
    private IEnumerator ManagerCoroutines()
    {
        while (true)
        {
            //グローバルタイムの更新と表示
            GameMaster.GlobalTime = GameMaster.GlobalTime.AddMinutes(15);
            //time = time.AddSeconds(10 * s.value);
            GameObject date = GameObject.Find("DateText");
            Text t = date.GetComponent<Text>();

            String year = GameMaster.GlobalTime.Year.ToString() + "年";
            String Month = GameMaster.GlobalTime.Month.ToString() + "月";
            String Day = GameMaster.GlobalTime.Day.ToString() + "日";
            String Hour = GameMaster.GlobalTime.Hour.ToString() + "時";
            String Minute = GameMaster.GlobalTime.Minute.ToString() + "分";

            t.text = year + Month + Day + Hour;

            GameMaster.Map.Reset_Observe();

            if (GameMaster.GlobalTime.Day - saveday > 0) 
            {
                GameMaster.Map.Reset_Mapinfo();
                foreach (GameObject g in GameMaster.Satellitelist)
                {
                    Broadcasting_Satellite c;
                    if (c = g.GetComponent<Broadcasting_Satellite>()) 
                    {
                        c.Reset_info();
                    }
                }
                saveday = GameMaster.GlobalTime.Day;
            }

            // 故障している衛星を削除
            //GameMaster.RemoveFailSatelliteList();

            yield return new WaitForSeconds(0.03f);//0.03fで30fpsぐらい
        }
    }




    // Update is called once per frame
    void Update()
    {
        //スパンの値でタイムスケールの調整
        Time.timeScale = 1.0f * GameMaster.SpanValue;

     //   GameMaster.Map.Satellite_Update();

        Citydict["東京"].GetComponent<CityComponent>().SatelliteNum();


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
    public static void CreateNewSat(double M0, double M1, double M2, double e, double i, double s_omg, double L_omg, double ET,String type,int[] param)
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
        component.Sensor_Performance = param[0];
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

    /// <summary>
    /// 準天頂衛星作成
    /// </summary>
    /// <param name="longitude">経度</param>
    /// <param name="latitude">緯度</param>
    public static void CreateQZS(String type, double longitude, double latitude, int sensor, double body)
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
        else if (type.Equals("BS")) 
        {
            prefab = (GameObject)Resources.Load("Prefabs/BS");
            satellite = Instantiate(prefab) as GameObject;
            component = satellite.GetComponent<Broadcasting_Satellite>();
        }

        GameMaster.AddSatelliteList(satellite);

      //  component.ID = GameMaster.Get_Satellite_ID();
        component.i = latitude;
        component.e = Math.Abs((0.0746703 / 40.5968) * latitude);
        component.s_omg0 = 269.9725;
        component.M0 = 181.9023;
        component.M1 = 1.002737;
        component.M2 = 0;
        component.ET = 14343.49492826;
        component.L_omg0 = 165 + longitude;
        component.Sensor_Performance = sensor;

        // body=0で一か月、body=1で三か月で5割故障
        component.fail = 0.00024 - 0.00016 * body;
    }

    /// <summary>
    /// 極軌道衛星作成
    /// </summary>
    /// <param name="longitude">経度</param>
    /// <param name="latitude">緯度</param>
    public static void CreatePOS(String type, double longitude, double latitude, int sensor, double body)
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
        else if (type.Equals("BS"))
        {
            prefab = (GameObject)Resources.Load("Prefabs/BS");
            satellite = Instantiate(prefab) as GameObject;
            component = satellite.GetComponent<Broadcasting_Satellite>();
        }

        GameMaster.AddSatelliteList(satellite);

     //   component.ID = GameMaster.Get_Satellite_ID();
        component.i = latitude;
        component.e = 0.0008546;
        component.s_omg0 = 223.1686;
        //component.M0 = 136.8816;
        component.M0 = 136.8816 + 90;
        //component.M1 = 14.117117471;

        // 速度
        component.M1 = 5;

        component.M2 = 0;
        component.ET = 97320.90946019;
        //component.L_omg0 = 272.6745;
        component.L_omg0 = longitude - 60;
        component.Sensor_Performance = sensor;

        // body=0で一か月、body=1で三か月で5割故障
        component.fail = 0.00024 - 0.00016 * body;
    }

    /// <summary>
    /// モルニヤ軌道衛星作成
    /// </summary>
    /// <param name="longitude">経度</param>
    /// <param name="latitude">緯度</param>
    public static void CreateMOS(String type, double longitude, double latitude, int sensor, double body)
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
        else if (type.Equals("BS"))
        {
            prefab = (GameObject)Resources.Load("Prefabs/BS");
            satellite = Instantiate(prefab) as GameObject;
            component = satellite.GetComponent<Broadcasting_Satellite>();
        }

        GameMaster.AddSatelliteList(satellite);

   //     component.ID = GameMaster.Get_Satellite_ID();
        component.i = latitude;
        component.e = 0.7134453;
        component.s_omg0 = 295.0386;
        //component.M0 = 136.8816;
        component.M0 = 9.2449 + 100;
        component.M1 = 2.00613016;
        component.M2 = 0;
        component.ET = 13154.54631441;
        //component.L_omg0 = 153.0958;
        component.L_omg0 = longitude - 15;
        //component.i = 62.9125;
        component.Sensor_Performance = sensor;

        // body=0で一か月、body=1で三か月で5割故障
        component.fail = 0.00024 - 0.00016 * body;
    }
    //void OnMouseDown()
    //{
    //    Vector3 vec =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    vec.z = 0;

    //    if (GameMaster.Map[vec.x, vec.y].Land && !GameMaster.Map[vec.x,vec.y].GS)
    //    {
    //        GameObject prefab = (GameObject)Resources.Load("Prefabs/ground_station");
    //        GameObject Base = Instantiate(prefab) as GameObject;
    //        GameMaster.Map[vec.x, vec.y].GS = true;
    //        GameObject ground = GameObject.Find("GroundStation");
    //        Base.transform.parent = ground.transform;
    //        Base.transform.position = vec;
    //    }

    //}
}
