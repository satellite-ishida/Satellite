using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 *　とりあえずの仕様 
 *　・都市を観測することで得点(自国の都市のみでもいいかも)
 *　・海、陸に関しては調整
 */

/// <summary>
/// GPS衛星クラス
/// </summary>

public class GPS_Satellite : SatelliteComponent
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        //     StartCoroutine("SatObject");
    }

    public Sprite StanbySprite;
    public Sprite SatelliteSprite;

    protected override void LaunchSat()
    {

        if ((GameMaster.GlobalTime - createtime).Days > necessary_time)
        {
            launch = true;
            SpriteRenderer MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            MainSpriteRenderer.color = Color.red;
            gameObject.transform.localScale = new Vector3(30,30,1);
            MainSpriteRenderer.sprite = SatelliteSprite;
            start = 1;
            GameObject g = GameObject.Find("Sat_List");
            var item = g.transform as RectTransform;

            foreach (RectTransform child in item)
            {
                if (child.GetComponent<Node_Info>().ID == ID)
                {
                    var text = child.GetComponentInChildren<Text>();
                    text.text = "ID:" + ID.ToString();
                    break;
                }

            }
        }
        else
        {
            GameObject g = GameObject.Find("Sat_List");
            var item = g.transform as RectTransform;

            int time_remaining = necessary_time - (GameMaster.GlobalTime - createtime).Days;

            foreach (RectTransform child in item)
            {
                if (child.GetComponent<Node_Info>().ID == ID)
                {
                    var text = child.GetComponentInChildren<Text>();
                    text.text = "ID:" + ID.ToString() + ":" + time_remaining.ToString() + "日";
                    break;
                }

            }

        }

    }

//    /// <summary>
//    /// スコア計算関数
//    /// </summary>
//    protected override void CalcScore()
//    {
        
//        int citynum = 0;
//        int landnum = 0;
//        int seanum = 0;

//        GameObject sensor = transform.FindChild("Sensor").gameObject;

//        citynum = 0;
//        landnum = 0;
//        seanum = 0;

//        int x = (int)transform.position.x;
//        int y = (int)transform.position.y;
//        int a = (int)((sensor.transform.lossyScale.x) * 0.09);
//        int b = (int)((sensor.transform.lossyScale.y) * 0.09);


//        for (int i = x - a; i <= x + a; i++)
//        {
//            for (int j = y - b; j < y + b; j++)
//            {
//                //とりあえず円で
//                //   if ((x - i) * (x - i) + (y - j) * (y - j) <= a * a)
//                if (((i - x) * (i - x)) * (b * b) + ((j - y) * (j - y)) * (a * a) <= a * a * b * b)
//                {
//                    if (!String.IsNullOrEmpty(GameMaster.Map[i, j].City))
//                    {
//                        citynum++;
//                    }
//                    if (GameMaster.Map[i, j].Land)
//                    {
//                        landnum++;
//                    }
//                    else
//                    {
//                        seanum++;
//                    }
//                }
//            }
//        }
//        //ゲームマスターにスコアの通知
//        GameMaster.AddScore(citynum);
//    }
}