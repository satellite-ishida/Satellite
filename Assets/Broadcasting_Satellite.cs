using UnityEngine;
using System.Collections;
using System;


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

    private Boolean info = false;

    public void Reset_info() 
    {
        info = false;
    }

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
                        info = true;
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
                    if (GameMaster.Map[i,j].Land && !GameMaster.Map[i, j].Info && info)
                    {
                        num++;
                        GameMaster.Map[i, j].Info = true;
                    }
                }
            }
        }

        //楕円の面積による得点補正(少し誤差がある)
        double ratio = (Math.PI * b * b) / (Math.PI * a * b);
        //ゲームマスターにスコアの通知
        GameMaster.AddScore((int)(num * ratio));
    }
}
