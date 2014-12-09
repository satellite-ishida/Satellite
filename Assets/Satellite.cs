using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

class Satellite
{

    private double M1;    //平均運動
    private double M2; //平均運動変化係数
    private double M0;   //平均近点角
    private double e;   //離心率
    private double i; //軌道傾斜角
    private double s_omg0;    //近地点引数
    private double L_omg0;   //昇交点赤経
    private double ET;    //元期

    private float locate_x;    //現在位置_x軸
    private float locate_y;    //現在位置_y軸

    public Satellite(float init_x,float init_y,double M0,double M1,double M2,double e,double i,double s_omg0,double L_omg0,double ET)
    {
        this.M0 = M0;
        this.M1 = M1;
        this.M2 = M2;
        this.e = e;
        this.i = i;
        this.s_omg0 = s_omg0;
        this.L_omg0 = L_omg0;
        this.ET = ET;

        this.locate_x = init_x;
        this.locate_y = init_y;
    }

    //位置の更新
    public void update_locate(DateTime ob_time)
    {
        float[] new_locate = calc_orbit(ob_time);
        this.locate_x = new_locate[0];
        this.locate_y = new_locate[1];
    }

    //xの値を返す
    public float get_locate_x()
    {
        return locate_x;
    }

    //yの値を返す
    public float get_locate_y()
    {
        return locate_y;
    }

    //衛星軌道の計算
    private float[] calc_orbit(DateTime ob_time)
    {

        //元期からの経過日数の計算
        double time_diff = calc_time_diff(ob_time);
        //平均運動の計算
        double Mm = M1 + M2 * time_diff;
        //軌道長半径の計算
        double a = Math.Pow(((2.975537 * Math.Pow(10.0, 15)) / (4.0 * Math.PI * Math.PI * Mm * Mm)), (1.0 / 3.0));
        //平均近点角の計算
        double M = M0 / 360 + M1 * time_diff + 0.5 * (M2 * Math.Pow(time_diff, 2));
        M = 360 * (M - Math.Floor(M));
        //離心近点角の計算
        double E = calc_eccentric_anomaly(M);
        //軌道面座標の計算
        double U = a * Math.Cos(E / 180 * Math.PI) - a * e;
        double V = a * Math.Sqrt(1 - e * e) * Math.Sin(E / 180 * Math.PI);
        //近地点引数と昇交点赤経の計算(若干誤差ある)(これ以降誤差が大きくなる)
        double s_omg = s_omg0 + ((180.0 * 0.174 * (2.0 - 2.5 * Math.Pow(Math.Sin(i / 180 * Math.PI), 2))) / (Math.PI * Math.Pow(a / 6378.137, 3.5))) * time_diff;
        double L_omg = L_omg0 - ((180.0 * 0.174 * Math.Cos(i / 180 * Math.PI)) / (Math.PI * Math.Pow(a / 6378.137, 3.5))) * time_diff;
        //軌道面上の位置(U,V)を地球中心の三次元直角座標(x,y,z)に変換する
        double x = U * (Math.Cos(L_omg / 180 * Math.PI) * Math.Cos(s_omg / 180 * Math.PI) - Math.Sin(L_omg / 180 * Math.PI) * Math.Cos(i / 180 * Math.PI) * Math.Sin(s_omg / 180 * Math.PI))
            - V * (Math.Cos(L_omg / 180 * Math.PI) * Math.Sin(s_omg / 180 * Math.PI) + Math.Sin(L_omg / 180 * Math.PI) * Math.Cos(i / 180 * Math.PI) * Math.Cos(s_omg / 180 * Math.PI));
        double y = U * (Math.Sin(L_omg / 180 * Math.PI) * Math.Cos(s_omg / 180 * Math.PI) + Math.Cos(L_omg / 180 * Math.PI) * Math.Cos(i / 180 * Math.PI) * Math.Sin(s_omg / 180 * Math.PI))
            - V * (Math.Sin(L_omg / 180 * Math.PI) * Math.Sin(s_omg / 180 * Math.PI) - Math.Cos(L_omg / 180 * Math.PI) * Math.Cos(i / 180 * Math.PI) * Math.Cos(s_omg / 180 * Math.PI));
        double z = U * Math.Sin(i / 180 * Math.PI) * Math.Sin(s_omg / 180 * Math.PI) + V * Math.Sin(i / 180 * Math.PI) * Math.Cos(s_omg / 180 * Math.PI);
        //グリニッジ子午線の赤経計算(ちょっと誤差出てるかも)
        double st0 = calc_greenwich_roll(new DateTime(ob_time.Year, 1, 1, 0, 0, 0));
        double stg = st0 + 1.002737909 * calc_num_day(ob_time);
        stg = 360 * (stg - Math.Floor(stg));
        //緯度・経度の計算(結構誤差出てる)
        double X = x * Math.Cos((-1) * stg / 180 * Math.PI) - y * Math.Sin((-1) * stg / 180 * Math.PI);
        double Y = x * Math.Sin((-1) * stg / 180 * Math.PI) + y * Math.Cos((-1) * stg / 180 * Math.PI);
        double Z = z;
        double latitude = Math.Asin((Z / Math.Sqrt(X * X + Y * Y + Z * Z))) * 180 / Math.PI;
        double longitude = Math.Atan2(Y, X) * 180 / Math.PI;

        return (new float[2] { (float)longitude, (float)latitude});
    }

    //1900年代は無いものとする
    private double calc_time_diff(DateTime ob_time)
    {
        double diff = ((double)ob_time.Year - 2000) * 1000 + calc_num_day(ob_time) + 1;
        double time_diff = Math.Abs(ET - diff);

        return time_diff;
    }

    private double calc_eccentric_anomaly(double M)
    {
        double e_r = e;
        double e_d = e_r * 180.0 / Math.PI;
        double Ei;
        Ei = M + e_d * Math.Sin(M / 180 * Math.PI);

        while (true)
        {
            double Mi = Ei - e_d * Math.Sin(Ei / 180 * Math.PI);
            if (Math.Abs(Mi - M) < 0.000000001)
            {
                break;
            }
            else
            {
                Ei = Ei + (M - Mi) / (1 - e_r * Math.Cos(Ei / 180 * Math.PI));
            }
        }

        return Ei;
    }

    private double calc_greenwich_roll(DateTime greenwichTime)
    {
        double Y = greenwichTime.Year;
        double M = greenwichTime.Month;
        double D = greenwichTime.Day;
        double h = greenwichTime.Hour;
        double m = greenwichTime.Minute;
        double s = greenwichTime.Second;

        if (M == 1 || M == 2)
        {
            M = M + 12;
            Y = Y - 1;
        }

        double JD = Math.Floor(365.25 * Y) + Math.Floor(Y / 400) - Math.Floor(Y / 100) + Math.Floor(30.59 * (M - 2)) + D + 1721088.5 + h / 24 + m / 1440 + s / 86400;
        double TJD = JD - 2440000.5;
        double st = (0.671262 + 1.0027379094 * TJD);
        double st_decimal = st - Math.Floor(st);

        return st_decimal;
    }

    //その年の1/1 00:00からどれだけの日数が経っているか計算する
    private double calc_num_day(DateTime observe_time)
    {
        //もしJST(日本時刻)ならUSTに直す。(時間から９時間引く)
        DateTime greenwich = new DateTime(observe_time.Year, 1, 1, 0, 0, 0);
        TimeSpan span = observe_time - greenwich;
        double day_decimal = ((double)observe_time.Hour + (double)observe_time.Minute / 60 + (double)observe_time.Second / 60 / 10) / 24;
        double day = double.Parse(span.Days.ToString());
        double num_day = day + day_decimal;

        return num_day;
    }
}