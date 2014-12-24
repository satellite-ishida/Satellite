using UnityEngine;
using System.Collections;

public class CityComponent : MonoBehaviour {

    /// <summary>
    /// 都市から近くの衛星の数を数える
    /// </summary>
    public int SatelliteNum() 
    {

        int num = 0;

        foreach (GameObject g in GameMaster.Satellitelist)
        {
            if (g.GetComponent<GPS_Satellite>() && g.GetComponent<SatelliteComponent>().Launch)
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

        GameMaster.AddScore(num);
        return num;
    }

    // Use this for initialization
    void Start()
    {

    }
}
