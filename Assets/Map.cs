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

class Map
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
                y =  -y ;
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
    /// 衛星オブジェクト
    /// </summary>
    /// 
    private List<GameObject> satelliteobject = new List<GameObject>();

    public GameObject SatelliteObject
    {
        set { satelliteobject.Add(value); }

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


    //現在いるセル(一マス)しか見ていない

    /// <summary>
    /// セルの情報を取得
    /// </summary>
    /// <param name="sa">衛星クラス</param>
    /// <return>セルデータ</return>
    /// 

    private Cell_Data Get_CellData(GameObject g)
    {
        /*
         *  セルはx軸方向は経度‐180を0として、東方向に360マスある
         *        y軸方向は緯度-90？(北極)を0として、南方向に180マスある
         */

        return this[g.GetComponent<SatelliteComponent>().X, g.GetComponent<SatelliteComponent>().Y];
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
        satelliteobject.RemoveAll(x => x.GetComponent<SatelliteComponent>().Fail);



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

    private int score = 0;

    public void CalcScore()
    {
        int citynum = 0;
        int landnum = 0;
        int seanum = 0;

        foreach (GameObject g in satelliteobject)
        {
            SatelliteComponent satellite = g.GetComponent<SatelliteComponent>();

            GameObject span = GameObject.Find("Span");
            Slider s = span.GetComponent<Slider>();

            for (int n = 0; n < Math.Floor(s.value); n++)
            {
                satellite.calc();
                GameObject sensor = g.transform.FindChild("Sensor").gameObject;

                citynum = 0;
                landnum = 0;
                seanum = 0;

                int x = (int)g.transform.position.x;
                int y = (int)g.transform.position.y;
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
                            if (string.Compare(this[i, j].City, null) != 0)
                            {
                                citynum++;
                            }
                            if (this[i, j].Land)
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
                score += citynum;
            }
            GameObject date = GameObject.Find("Score");
            Text t = date.GetComponent<Text>();
            t.text = citynum.ToString() + " " + score;
        }
    }
}


