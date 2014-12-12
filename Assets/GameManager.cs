using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    private GameObject _charactor;
    private GameObject _charactor2;
    private GameObject _charactor3;

    private DateTime observe_time = new DateTime(2014, 12, 31, 23, 55, 28);

    private Map map;

    //private DateTime observe_time = new DateTime(2009, 1, 1, 0, 0, 0);

    // Use this for initialization
    void Start()
    {
        // キャラクターを取得する
        this._charactor = GameObject.Find("Sattellite");
        this._charactor2 = GameObject.Find("Satellite2");
        this._charactor3 = GameObject.Find("Satellite3");

        GameObject prefab = (GameObject)Resources.Load("Prefabs/QZS");

        GameObject prefab2 = (GameObject)Resources.Load("Prefabs/point");

        // 緯度
        double latitude = _charactor3.transform.localPosition.y;

        // 経度
        double longitude = _charactor3.transform.localPosition.x;

        // 衛星の個数
        int n = 5;

        map = new Map();

        for (int i = 0; i < 360; i += (360 / n))
        {
            GameObject satellite = Instantiate(prefab) as GameObject;
            SatelliteComponent component = satellite.GetComponent<SatelliteComponent>();

            map.SatelliteObject = satellite;
            //map.Satellite = component;
  
            component.TIME = observe_time;
            component.ID = i;
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
            
        }

        for (int i = -180; i < 180; i++) 
        {
            for (int j = -89; j < 90; j++) 
            {
                if (string.Compare(map[i, j].City, null) != 0) 
                {
                    GameObject point = Instantiate(prefab2) as GameObject;
                    point.transform.position = new Vector3(i,j,1);
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
    }

    // Update is called once per frame
    void Update()
    {
        map.Satellite_Update();

    }


    private Vector3 start;


    void OnMouseDown()
    {
        //GameObject prefab3 = (GameObject)Resources.Load("Prefabs/base_station");

    
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        start = new Vector3(x, y, 0);
        

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
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        Vector3 currentPosition = new Vector3(x, y, 1);


        GameObject prefab3 = (GameObject)Resources.Load("Prefabs/base_station");

        GameObject aa = Instantiate(prefab3) as GameObject;
        aa.transform.position = Camera.main.ScreenToWorldPoint(currentPosition/2 + start/2);
        aa.transform.localScale = (currentPosition  - start )*2;

        print(currentPosition.x - start.x);
        
         double a = (currentPosition.x  - start.x)/4;
         double b = (currentPosition.y  - start.y)/4;
        Vector3 O = Camera.main.ScreenToWorldPoint(currentPosition/2 + start/2);
        
        int citynum = 0;
        int landnum = 0;
        int seanum  = 0;

        for (int i = (int)O.x - (int)Math.Abs(currentPosition.x - start.x) / 4; i < (int)O.x + (int)Math.Abs(currentPosition.x - start.x); i++) 
        {
            for (int j = (int)O.y - (int)Math.Abs(currentPosition.y - start.y) / 4; j < (int)O.y + (int)Math.Abs(currentPosition.y - start.y) / 4; j++) 
            {
                if (((i - O.x) * (i - O.x)) / (a * a) + ((j - O.y) * (j - O.y)) / (b * b) <= 1) 
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

        print(citynum + "," + landnum + "," + seanum);

    }
}
