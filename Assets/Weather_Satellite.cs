using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 *　とりあえずの気象衛星の仕様 
 *　・マップの地形に関係なく情報を得られる
 *　・観測したセルは一定時間、気象衛星による観測得点が入らない
 */

/// <summary>
/// 気象衛星クラス
/// </summary>

public class Weather_Satellite : SatelliteComponent 
{

    public override void Awake()
    {
        base.Awake();
    }


    public override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// スコア計算関数
    /// </summary>
    protected override void CalcScore()
    {
        GameObject sensor = transform.FindChild("Sensor").gameObject;

        int num = 0;

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        int a = (int)((sensor.transform.lossyScale.x) * 0.09);
        int b = (int)((sensor.transform.lossyScale.y) * 0.09);


            for (int i = x - a; i <= x + a; i++)
            {
                for (int j = y - b; j < y + b; j++)
                {
                    if (((i - x) * (i - x)) * (b * b) + ((j - y) * (j - y)) * (a * a) <= a * a * b * b)
                    {
                        if ((GameMaster.GlobalTime - GameMaster.Map[i, j].Observe).TotalDays > 0.5)
                        {
                            num += 3;
                            GameMaster.Map[i, j].Observe = GameMaster.GlobalTime;
                        }
                    }
                }
            }

         
        //楕円の面積による得点補正(少し誤差がある)
        double ratio = (Math.PI * b * b) / (Math.PI * a * b);
        //ゲームマスターにスコアの通知
        GameMaster.AddScore((int)(num*ratio));
    }



    private double necessary_time = 1;

    /*
    protected override void CalcNecessaryTime() 
    {
        necessary_time = 5 + (base.Sensor_Performance-5) * 0.5 + base.Body_Performance * 5;
    }
    */
    public Sprite StanbySprite;
    public Sprite SatelliteSprite;

    protected override void LaunchSat()
    {

        if ((GameMaster.GlobalTime - createtime).TotalDays > necessary_time)
        {
            launch = true;
            SpriteRenderer MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            MainSpriteRenderer.color = Color.blue;
            gameObject.transform.localScale = new Vector3(30, 30, 1);
            MainSpriteRenderer.sprite = SatelliteSprite;
            start = 1;

            GameObject g = GameObject.Find("Sat_List");
            var item = g.transform as RectTransform;

            String POP = "ID:" + ID.ToString() + "の気象衛星が打ち上げられました";

            GameMaster.POPUP(POP);

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

            int time_remaining = (int)(necessary_time - (GameMaster.GlobalTime - createtime).TotalDays);

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
}
