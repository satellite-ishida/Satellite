using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Input_Easy_Sat_Component : MonoBehaviour {

    

    //ペグマンを置く
    public void Put_Locate()
    {
        if (GameObject.Find("Pegman(Clone)") == null)
        {
            GameObject prefab = (GameObject)Resources.Load("Prefabs/Pegman");
            GameObject Pegman = Instantiate(prefab) as GameObject;
        }
    }

    public void Submit()
    {
        GameObject Pegman;
        if ((Pegman = GameObject.Find("Pegman(Clone)")) != null)
        {
            /////↓森田定数を使った静止軌道
            // 緯度
            double latitude = Pegman.transform.localPosition.y;

            // 経度
            double longitude = Pegman.transform.localPosition.x;

            //double s_omg0 = 269.9725;
            //double M0 = 309.9023;
            //double ET = 14343.49492826;
            //// ズレ修正
            //double M1 = 1.002737;
            //// 位相?
            //double L_omg0 = (36 + longitude);
            //// 経度
            //double i = latitude;
            //double e = Math.Abs((0.0746703 / 40.5968) * latitude);

            //GameManager.CreateNewSat(M0, M1, 0, e, i, s_omg0, L_omg0, ET);
            GameManager.CreateMOS(longitude, latitude);
            
            //ペグマンをデストロイ
            Destroy(Pegman);
        }
    }
}
