using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/// <summary>
/// ゲーム全体の情報をstaticで持つクラス
/// </summary>
public class GameMaster : MonoBehaviour
{

    //衛星のID(生成された順番)
    private static int Satellite_ID = 0;
    public static int Get_Satellite_ID()
    {
        Satellite_ID++;

        return Satellite_ID - 1;
    }
    //スコア（仮）
    private static int Score = 0;
    public static void AddScore(int add_num)
    {
        Score += add_num;

        //スコアの表示
        GameObject data = GameObject.Find("Score");
        Text t = data.GetComponent<Text>();
        t.text = Score.ToString();
    }
    public static int Get_Score()
    {
        return (Score);
    }

    /// <summary>
    /// マップクラス
    /// </summary>
    private static Map map;

    public static Map Map
    {
        get { return map; }
        set { map = value; }
    }

    //衛星リスト
    private static List<GameObject> SatelliteList = new List<GameObject>();

    public static List<GameObject> Satellitelist
    {
        get {
            return SatelliteList;
        }
    }
    //衛星の追加
    public static void AddSatelliteList(GameObject g)
    {
        SatelliteList.Add(g);
        GUI_Manager.Add_Sat_Node(g.GetComponent<SatelliteComponent>()); //GUI
    }
    //衛星の取得（衛星のID番号）
    public static GameObject GetSatelliteByID(int ID)
    {
        GameObject g = SatelliteList.Find(x => (x.GetComponent<SatelliteComponent>().ID == ID));
        return g;
    }
    //Failがtrueの衛星をリストから削除
    public static void RemoveFailSatelliteList()
    {
        GUI_Manager.Destroy_Sat_Node(SatelliteList.FindAll(x => x.GetComponent<SatelliteComponent>().Fail)); //GUI
        SatelliteList.RemoveAll(x => x.GetComponent<SatelliteComponent>().Fail);
    }

    //スライダー(タイムスパン)の値
    private static float Span_Value = 1.0f;
    public static void SetSpanValue(float f)
    {
        Span_Value = f;
    }
    public static float GetSpanValue()
    {
        return Span_Value;
    }
}
