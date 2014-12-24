using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CityComponent : MonoBehaviour
{

    private String countryname;
    public String CountryName {
        set { countryname = value; }
    }

    /// <summary>
    /// 都市から近くの衛星の数を数える
    /// </summary>
    public int SatelliteNum()
    {

        int num = 0;

        foreach (GameObject g in GameMaster.Satellitelist)
        {

            if (g.GetComponent<GPS_Satellite>()
                && g.GetComponent<SatelliteComponent>().Launch)
            {

                GameObject sensor = g.transform.FindChild("Sensor").gameObject;
                int range = (int)((sensor.transform.lossyScale.x) * 0.09);

                Vector3 Cpos = transform.position;
                Vector3 Spos = g.transform.position;
                float dis = Vector3.Distance(Cpos, Spos);
                if (dis < range)
                {
                    num++;
                }
            }
        }
        int score = 0;

        if (num == 1) 
        {
            score = 1;
        }
        else if (num == 2)
        {
            score = 4;
        }
        else if(num > 2)
        {
            score = 9;
        }

        if (countryname.Equals("日本")) 
        {
            score = (int)(score * 2);
        }
        GameMaster.AddScore(score);
        return num;
    }
    
    // Use this for initialization
    void Start()
    {

    }
}
