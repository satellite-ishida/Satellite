using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 地図のセル情報を扱うクラス
/// </summary>

public class Map
{
    /// <summary>
    /// セル情報を格納する配列
    /// </summary>
    /// 
    private Cell_Data[,] cd;

    /// <summary>
    /// セル情報を格納する配列
    /// </summary>
    public Cell_Data this[double longitude, double latitude]
    {
        get
        {

            int x = (int)longitude + 180;
            int y = (int)latitude * (-1) + 90;

            //地図外のポイントを入力された時の処理

            if (x < 0)
            {
                x = 359 - (-x);
            }
            else if (360 <= x)
            {
                x = -(360 - x);
            }

            //y軸がはみ出した場合、地球の反対側を観測
            if (y < 0)
            {
                y = -y;
                x = (x < 180) ? x + 180 : x - 180;
            }
            else if (180 <= y)
            {
                y = 179 - (y - 180);    //(y-180)がオーバーしたセルの数
                x = (x < 180) ? x + 180 : x - 180;
            }

            return this.cd[x, y];

        }
        private set { this.cd[(int)longitude + 180, (int)latitude * (-1) + 90] = value; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Map()
    {

        this.cd = new Cell_Data[360, 180];


        for (int i = 0; i < 180; i++)
        {
            for (int j = 0; j < 360; j++)
            {
                this.cd[j, i] = new Cell_Data();
            }
        }

        /*
            csvファイルから地図にデータを埋め込む
         */
        //文字コードはUTF-8じゃないと文字化けする
        TextAsset citydata = Resources.Load("csv/city") as TextAsset;
        StringReader reader = new StringReader(citydata.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            string[] values = line.Split(',');

            this[int.Parse(values[2]), int.Parse(values[3])].Country = values[0];
            this[int.Parse(values[2]), int.Parse(values[3])].City = values[1];
        }

        /*
           csvファイルから陸か海を埋め込む
        */

        //1が陸、0が海として書いてある
        TextAsset mapdata = Resources.Load("csv/map") as TextAsset;
        reader = new StringReader(mapdata.text);

        int k = 0;
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            string[] values = line.Split(',');

            for (int i = 0; i < 360; i++)
            {
                if (int.Parse(values[i]) == 1)
                {
                    this.cd[i, k].Land = true;

                }
                else
                {
                    this.cd[i, k].Land = false;
                }
            }
            k++;
        }
    }


    /// <summary>
    /// 都市データの再読み込み
    /// </summary>
    /// <param name="filename">都市データのファイル名</param> 

    public void City_Input(string filename)
    {

        for (int i = 0; i < 180; i++)
        {
            for (int j = 0; j < 360; j++)
            {
                this.cd[j, i].Country = null;
                this.cd[j, i].City = null;
            }
        }


        using (System.IO.StreamReader sr = new System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("shift_jis")))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                string[] values = line.Split(',');

                this[int.Parse(values[2]), int.Parse(values[3])].Country = values[0];
                this[int.Parse(values[2]), int.Parse(values[3])].City = values[1];
            }

        }
    }


    /// <summary>
    /// 地図データの再読み込み
    /// </summary>
    /// <param name="filename">地図データのファイル名</param>
    /// 
    public void Map_Input(string filename)
    {

        using (System.IO.StreamReader sr = new System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("shift_jis")))
        {
            int j = 0;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                string[] values = line.Split(',');

                for (int i = 0; i < 360; i++)
                {
                    if (int.Parse(values[i]) == 1)
                    {
                        this.cd[i, j].Land = true;

                    }
                    else
                    {
                        this.cd[i, j].Land = false;
                    }
                }
                j++;
            }
        }

    }


    /// <summary>
    /// 基地局を追加
    /// </summary>
    /// <param name="x">経度</param>
    /// <param name="y">緯度</param>>
    /// <return>基地建設の成功判定</return>
    /// 
    public bool Set_BaseStation(double x, double y)
    {
        if (this[x, y].Land)
        {
            this[x, y].GS = true;
            return true;

        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 観測状況のリセット
    /// </summary>
    public void Reset_Observe()
    {
        for (int i = 0; i < 360; i++) 
        {
            for (int j = 0; j < 180; j++) 
            {

                cd[i, j].Observe_Status = false;


            }
        }
    }

    /// <summary>
    /// 全衛星の位置更新
    /// </summary>
    /// 
    public void Satellite_Update()
    {
        //foreach (GameObject g in satelliteobject)
        //{
        //    // GameObjectのSatelliteComponentを取得

        ///// <summary>
        ///// 全衛星の位置更新
        ///// </summary>
        ///// 
        //public void Satellite_Updata()
        //{

        //    for (int i = 0; i < satellite.Count; i++)
        //    {
        //        satellite[i].update_locate(satellite[i].TIME);
        //        satellite[i].transform.position = new Vector3(satellite[i].X, satellite[i].Y, 0);
        //        satellite[i].TIME = satellite[i].TIME.AddMinutes(10);
        //   //     print(satellite[i].X + "," + satellite[i].Y);

        //        //セル情報表示関係
        //        /*
        //           String name = Current_Country(satellite[i]);
        //           if (string.Compare(name, null) != 0 && i == 0) 
        //           {
        //               print(name);
        //           }*/


        //        if (breakjudg(satellite[i])) 
        //        {
        //            satellite.RemoveAt(i);
        //            Destroy(satelliteobject[i]);
        //        }
        //    }


        //    SatelliteComponent component = g.GetComponent<SatelliteComponent>();
        //    //
        //    if (component.breakjudg())
        //    {
        //        satelliteobject.Remove(g);
        //        Destroy(g);
        //    }
        //}
        GameMaster.RemoveFailSatelliteList();



        //for (int i = 0; i < satellite.Count; i++)
        //{
        //    //satellite[i].update_locate(satellite[i].TIME);
        //    //satellite[i].transform.position = new Vector3(satellite[i].X, satellite[i].Y, 0);
        //    //satellite[i].TIME = satellite[i].TIME.AddMinutes(10);
        //    //     print(satellite[i].X + "," + satellite[i].Y);

        //    //セル情報表示関係
        //    /*
        //       String name = Current_Country(satellite[i]);
        //       if (string.Compare(name, null) != 0 && i == 0) 
        //       {
        //           print(name);
        //       }*/



        //}

        /*
        foreach (SatelliteComponent s in satellite) 
        {
            s.update_locate(s.TIME);
            s.transform.position = new Vector3(s.X, s.Y, 0);
            s.TIME = s.TIME.AddMinutes(10);
        }
        */
    }
    
}
