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
 *　・もらった情報を地上に送信(同じ情報は一度のみ)
 *　・一日ごとに放送局からもらう情報が更新される
 */

/// <summary>
/// 放送衛星クラス
/// </summary>


public class Broadcasting_Satellite : SatelliteComponent{

    public override void Start()
    {
        base.Start();
        //     StartCoroutine("SatObject");
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
                    if (!GameMaster.Map[i, j].Observe)
                    {
                        num++;
                        GameMaster.Map[i, j].Observe = true;
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
