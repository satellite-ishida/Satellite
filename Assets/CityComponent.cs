using UnityEngine;
using System.Collections;

public class CityComponent : MonoBehaviour {

    /// <summary>
    /// 都市の観測範囲
    /// </summary>
    private int range = 70;

    /// <summary>
    /// 都市から近くの衛星の数を数える
    /// </summary>
    public int SatelliteNum() 
    {

        int num = 0;

        foreach (GameObject g in GameMaster.Satellitelist)
        {
            Vector3 Cpos = transform.position;
            Vector3 Spos = g.transform.position;
            float dis = Vector3.Distance(Cpos, Spos);
            if (dis < range) 
            {
                num++;
            }
        }
        print(num);
        return num;
    }






    // Use this for initialization
    void Start()
    {

    }
}
