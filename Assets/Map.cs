using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

    /// <summary>
    /// 地図のセル情報を扱うクラス
    /// </summary>

    class Map
    {

        /// <summary>
        /// セル情報を格納する配列
        /// </summary>
        private Cell_Data[,] cd;



        /// <summary>
        /// 衛星クラスのリスト管理
        /// </summary>
        private List<Satellite> satellite = new List<Satellite>();
        

        

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

                using (System.IO.StreamReader sr = new System.IO.StreamReader("../../city.csv", System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        string[] values = line.Split(',');

                        this.cd[int.Parse(values[2]) + 180, int.Parse(values[3]) * (-1) + 90].Country = values[0];
                        this.cd[int.Parse(values[2]) + 180, int.Parse(values[3]) * (-1) + 90].City = values[1];
                    }

                }

             /*
                csvファイルから陸か海を埋め込む
             */

            using (System.IO.StreamReader sr = new System.IO.StreamReader("../../map.csv", System.Text.Encoding.GetEncoding("shift_jis")))
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


        /// <summary>
        /// 新しい衛星を追加する
        /// </summary>

        public void Satellite_Add() 
        {

//            Satellite st = new Satellite();

 //           this.satellite.Add(st);

        }




    }


