using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    private GameObject _charactor;
    private GameObject _charactor2;
    private GameObject _charactor3;

    private List<Satellite> sat_list = new List<Satellite>();
    //打ち上げ時間
    private DateTime observe_time = new DateTime(2008, 5, 15, 2, 0, 0);

    // Use this for initialization
    void Start()
    {
        // キャラクターを取得する
        this._charactor = GameObject.Find("Satellite");
        this._charactor2 = GameObject.Find("Satellite2");
        this._charactor3 = GameObject.Find("Satellite3");
		// aaa

        double M1 = 1.002744;    //平均運動
        double M2 = 0.0; //平均運動変化係数
        double M0 = 30.0;   //平均近点角
        double e = 0.075;   //離心率
        double i = 43.0; //軌道傾斜角
        double s_omg0 = 270.0;    //近地点引数
        double L_omg0 = 15.0;   //昇交点赤経
        double ET = 08075.0;    //元期

        Satellite test_sat = new Satellite(0.0f,0.0f,M0,M1,M2,e,i,s_omg0,L_omg0,ET);
        sat_list.Add(test_sat);


        M1 = 1.002744;    //平均運動
         M2 = 0.0; //平均運動変化係数
         M0 = 150.0;   //平均近点角
         e = 0.075;   //離心率
         i = 43.0; //軌道傾斜角
         s_omg0 = 270.0;    //近地点引数
         L_omg0 = 255.0;   //昇交点赤経
         ET = 08075.0;    //元期

        Satellite test_sat2 = new Satellite(0.0f, 0.0f, M0, M1, M2, e, i, s_omg0, L_omg0, ET);
        sat_list.Add(test_sat2);

        
         M1 = 1.002744;    //平均運動
         M2 = 0.0; //平均運動変化係数
         M0 = 270.0;   //平均近点角
         e = 0.075;   //離心率
         i = 43.0; //軌道傾斜角
         s_omg0 = 270.0;    //近地点引数
         L_omg0 = 135.0;   //昇交点赤経
         ET = 08075.0;    //元期

        Satellite test_sat3 = new Satellite(0.0f, 0.0f, M0, M1, M2, e, i, s_omg0, L_omg0, ET);

        sat_list.Add(test_sat3);


    }

    // Update is called once per frame
    void Update()
    {
        // 描画
        int count = 0;
        foreach (Satellite sat in sat_list)
        {
            sat.update_locate(observe_time);
            if(count == 0)_charactor.transform.localPosition = new Vector3(sat.get_locate_x(), sat.get_locate_y(), 0.0f);
            else if (count == 1) _charactor2.transform.localPosition = new Vector3(sat.get_locate_x() , sat.get_locate_y() , 0.0f);
            else if (count == 2) _charactor3.transform.localPosition = new Vector3(sat.get_locate_x() , sat.get_locate_y() , 0.0f);
            count++;
        }
        observe_time = observe_time.AddMinutes(10);
    }
}
