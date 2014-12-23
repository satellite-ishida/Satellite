using UnityEngine;
using System.Collections;
using System;

public class Sat_Info : MonoBehaviour {

    /// <summary>
    /// 平均近点角
    /// </summary>
    public double M0;

    /// <summary>
    /// 平均運動
    /// </summary>
    public double M1;

    /// <summary>
    /// 平均運動変化係数
    /// </summary>
    public double M2;

    /// <summary>
    /// 離心率
    /// </summary>
    public double e;

    /// <summary>
    /// 軌道傾斜角
    /// </summary>
    public double i;

    /// <summary>
    /// 近地点引数
    /// </summary>
    public double s_omg0;

    /// <summary>
    /// 昇交点赤経
    /// </summary>
    public double L_omg0;

    /// <summary>
    /// 元期
    /// </summary>
    public double ET;

    /// <summary>
    /// 衛星の名前
    /// </summary>
    public String NAME;

    /// <summary>
    /// 衛星のID
    /// </summary>
    public int ID;


    public void set_Info(SatelliteComponent sc)
    {
        M0 = sc.M0;
        M1 = sc.M1;
        M2 = sc.M2;
        e = sc.e;
        i = sc.i;
        s_omg0 = sc.s_omg0;
        L_omg0 = sc.L_omg0;
        ET = sc.ET;
        NAME = sc.NAME;
        ID = sc.ID;
    }

    public void Click_Sat_Node()
    {
        Sat_Info sc = gameObject.GetComponent<Sat_Info>();
        Info_Panel_Manager.target_Change(sc.ID);
    }
}
