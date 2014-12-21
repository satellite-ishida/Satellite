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

    /// <summary>
    /// 現在位置_x軸
    /// </summary>
    private float locate_x;

    /// <summary>
    /// 現在位置_y軸
    /// </summary>
    private float locate_y;

	// Update is called once per frame
	void Update () {
	
	}
}
