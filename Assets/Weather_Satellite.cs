using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 気象衛星クラス
/// </summary>

public class Weather_Satellite : SatelliteComponent 
{

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
        int citynum = 0;
        int landnum = 0;
        int seanum = 0;

        GameObject sensor = transform.FindChild("Sensor").gameObject;

        citynum = 0;
        landnum = 0;
        seanum = 0;

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        int a = (int)((sensor.transform.lossyScale.x) * 0.09);
        int b = (int)((sensor.transform.lossyScale.y) * 0.09);


        for (int i = x - a; i <= x + a; i++)
        {
            for (int j = y - b; j < y + b; j++)
            {
                //とりあえず円で
                //   if ((x - i) * (x - i) + (y - j) * (y - j) <= a * a)
                if (((i - x) * (i - x)) * (b * b) + ((j - y) * (j - y)) * (a * a) <= a * a * b * b)
                {
                    if (string.Compare(GameMaster.Map[i, j].City, null) != 0)
                    {
                        citynum++;
                    }
                    if (GameMaster.Map[i, j].Land)
                    {
                        landnum++;
                    }
                    else
                    {
                        seanum++;
                    }
                }
            }
        }
        //ゲームマスターにスコアの通知
        GameMaster.AddScore(citynum);
    }
}
