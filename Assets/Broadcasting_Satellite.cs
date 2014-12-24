using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/*
 *　とりあえずの放送衛星の仕様 
 *　・放送局(基地局)から情報をもらう
 *　・もらった情報を地上に送信
 */

/// <summary>
/// 放送衛星クラス
/// </summary>


public class Broadcasting_Satellite : SatelliteComponent{

    public override void Awake()
    {
        base.Awake();
    }


    public override void Start()
    {
        base.Start();
    }

    private DateTime info = GameMaster.GlobalTime;

    private void Get_info() 
    {
        GameObject sensor = transform.FindChild("Sensor").gameObject;

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
                    if (GameMaster.Map[i, j].GS)
                    {
                        info = GameMaster.GlobalTime;
                    }
                }
            }
        }
    
    }

    /// <summary>
    /// スコア計算関数
    /// </summary>
    protected override void CalcScore()
    {
        Get_info();
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
                    if (GameMaster.Map[i,j].Land && (info - GameMaster.Map[i, j].Info).TotalDays > 1 )
                    {
                        if (!String.IsNullOrEmpty(GameMaster.Map[i, j].City)) { num++; }
                        num++;
                        GameMaster.Map[i, j].Info = GameMaster.GlobalTime;
                    }
                }
            }
        }

        //楕円の面積による得点補正(少し誤差がある)
        double ratio = (Math.PI * b * b) / (Math.PI * a * b);
        //ゲームマスターにスコアの通知
        GameMaster.AddScore((int)(num * ratio));
    }

    public Sprite StanbySprite;
    public Sprite SatelliteSprite;

    protected override void LaunchSat()
    {

        if ((GameMaster.GlobalTime - createtime).Days > necessary_time)
        {
            launch = true;
            SpriteRenderer MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            MainSpriteRenderer.color = Color.yellow;
            gameObject.transform.localScale = new Vector3(30, 30, 1);
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
}
