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
    public enum SatelliteCost
    {
        GPS = 3000,
        Weather = 200000,
        BS = 20000
    }

    //衛星のID(生成された順番)
    private static int Satellite_ID = 0;
    public static int Get_Satellite_ID()
    {
        Satellite_ID++;

        return Satellite_ID - 1;
    }
    //スコア（仮）
    private static int Score = 1000000000;

    public static void AddScore(int add_num)
    {
        Score += add_num;


        //スコアの表示
        GameObject date = GameObject.Find("ScoreText");
        Text t = date.GetComponent<Text>();
        t.text = Score.ToString();


    }

    /// <summary>
    /// スコアの使用
    /// </summary>
    /// <param name="sub_num"></param>
    /// <returns>
    /// スコアが足りていたらtrue
    /// 足りていなかったらfalse
    /// </returns>
    public static Boolean SubScore(int sub_num)
    {
        if (Score >= sub_num)
        {
            Score -= sub_num;
            return true;
        }
        else
        {
            return false;
        }
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
        get
        {
            return SatelliteList;
        }
    }
    //衛星の追加
    public static void AddSatelliteList(GameObject g)
    {
        GUI_Manager.Set_Log(Global_Time.ToString() + " : " + "衛星追加");
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
        GameObject g = SatelliteList.Find(x => x.GetComponent<SatelliteComponent>().Fail);
        if (g != null)
        {
            GUI_Manager.Destroy_Sat_Node(g); //GUI
            SatelliteList.Remove(g);
            Destroy(g);
            string POP = "ID:" + g.GetComponent<SatelliteComponent>().ID.ToString() + "の衛星が壊れました";
            GameMaster.POPUP(POP);
        }
    }

    //スライダー(タイムスパン)の値
    private static float Span_Value = 1.0f;

    public static float SpanValue
    {
        get { return Span_Value; }
        set
        {
            Span_Value = value;
        }
    }

    private static DateTime Global_Time = new DateTime(2000, 1, 1, 0, 0, 0);

    public static DateTime GlobalTime
    {
        get { return Global_Time; }
        set { Global_Time = value; }
    }

    //テキストのログ
    private static Queue<String> Log_Queue = new Queue<string>();
    public static int Queue_Size = 8;
    public static void Add_Log(String log)
    {
        Log_Queue.Enqueue(log);
        if (Log_Queue.Count > 8)
        {
            Log_Queue.Dequeue();
        }
    }
    public static Queue<String> Get_Log()
    {
        return Log_Queue;
    }

    public static void POPUP(String s)
    {
        GameObject top_p = GameObject.Find("TOP_Panel");
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Popup_Panel");
        var item = GameObject.Instantiate(prefab.transform) as RectTransform;
        Popup_Manager pm = item.GetComponent<Popup_Manager>();

        item.SetParent(top_p.transform, false);

        pm.UP(s);
    }
    /*
    public static void SetSpanValue(float f)
    {
        Span_Value = f;
    }
    public static float GetSpanValue()
    {
        return Span_Value;
    }*/
}
