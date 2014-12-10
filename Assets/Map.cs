using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;

    /// <summary>
    /// 地図のセル情報を扱うクラス
    /// </summary>

       class Map : MonoBehaviour
    {

        /// <summary>
        /// セル情報を格納する配列
        /// </summary>
        private Cell_Data[,] cd;



        /// <summary>
        /// 衛星クラスのリスト管理
        /// </summary>
        private List<SatelliteComponent> satellite = new List<SatelliteComponent>();


       //衛星の削除を考えないと・・・
        /// <summary>
        /// 衛星クラスのリスト管理
        /// </summary>
        public SatelliteComponent Satellite
        {
             set { satellite.Add(value);}
        }
        
        
        /*
          コンストラクタ
           */
           
        public Map() 
        {
            
            this.cd = new Cell_Data[360,180];


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

                        this.cd[int.Parse(values[2]) + 180, int.Parse(values[3]) * (-1) + 90].Country = values[0];
                        this.cd[int.Parse(values[2]) + 180, int.Parse(values[3]) * (-1) + 90].City = values[1];
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

            for (int i = 0; i < 180; i++) {
                for (int j = 0; j < 360; j++) {
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

                    this.cd[int.Parse(values[2]) + 180, int.Parse(values[3]) * (-1) + 90].Country = values[0];
                    this.cd[int.Parse(values[2]) + 180, int.Parse(values[3]) * (-1) + 90].City = values[1];
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
                int j=0;
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


       //現在いるセル(一マス)しか見ていない

        /// <summary>
        /// 衛星のいる都市を取得
        /// </summary>
        /// <param name="sa">衛星クラス</param>
        /// <return>現在地の都市</return>
        /// 
           
        private String Current_City(SatelliteComponent sa) 
        {
            /*
             *  セルはx軸方向は経度‐180を0として、東方向に360マスある
             *        y軸方向は緯度-90？(北極)を0として、南方向に180マスある
             */

            if(string.Compare(cd[(int)sa.X + 180, (int)sa.Y + 90].City, null) != 0)
            {
                print(cd[(int)sa.X + 180, (int)sa.Y + 90].City);
                return cd[(int)sa.X + 180, (int)sa.Y + 90].City;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 衛星のいる国を取得
        /// </summary>
        /// <param name="sa">衛星クラス</param>
        /// <return>現在地の都市</return>
        /// 

        private String Current_Country(SatelliteComponent sa)
        {

            if (string.Compare(cd[(int)sa.X + 180, (int)sa.Y + 90].Country, null) != 0)
            {
                return cd[(int)sa.X + 180, (int)sa.Y + 90].Country;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 全衛星の位置更新
        /// </summary>
        /// 
        public void Satellite_Updata()
        {
            for (int i = 0; i < satellite.Count; i++)
            {
                satellite[i].update_locate(satellite[i].TIME);
                satellite[i].transform.position = new Vector3(satellite[i].X, satellite[i].Y, 0);
                satellite[i].TIME = satellite[i].TIME.AddMinutes(10);
           //     print(satellite[i].X + "," + satellite[i].Y);


                   String name = Current_Country(satellite[i]);
                   if (string.Compare(name, null) != 0 && i == 0) 
                   {
                       print(name);
                   }
            }
        }



    }


