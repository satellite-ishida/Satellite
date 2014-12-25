using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SatelliteComponent : MonoBehaviour {
    /// <summary>
    /// 平均近点角
    /// </summary>
    public double M0;

    /// <summary>
    /// 平均運動
    /// </summary>
    public double M1;

    /// <summary>
    /// 平均運動変化係数
    /// </summary>
    public double M2;

    /// <summary>
    /// 離心率
    /// </summary>
    public double e;

    /// <summary>
    /// 軌道傾斜角
    /// </summary>
    public double i;

    /// <summary>
    /// 近地点引数
    /// </summary>
    public double s_omg0;

    /// <summary>
    /// 昇交点赤経
    /// </summary>
    public double L_omg0;

    /// <summary>
    /// 現在時刻
    /// </summary>
    protected DateTime observe_time = new DateTime(2000,1,1,0,0,0);


    /// <summary>
    /// 衛星のID
    /// </summary>
    public int ID;

    /// <summary>
    /// 元期
    /// </summary>
    public double double_et;
    public  DateTime date_et;
    public double ET
    {
        get { return double_et; }
        set {
            double_et = value;
            int year = (int)(double_et / 1000);
            date_et = new DateTime(year+2000, 1, 1);
            double diff_day = (double)(double_et - year * 1000) - 1;
            date_et = date_et.AddDays(diff_day);
            observe_time = date_et;
        }
    }

    /// <summary>
    /// 衛星の名前
    /// </summary>
    private String name;
    public String NAME
    {
        get { return name; }
        set { name = value; }
    }

    /// <summary>
    /// 現在位置_x軸
    /// </summary>
    private  float locate_x;

    /// <summary>
    /// 現在位置_y軸
    /// </summary>
    private float locate_y;



    /// <summary>
    /// 現在時刻
    /// </summary>
    public DateTime TIME
    {
        get { return observe_time; }
        set { observe_time = value; }
    }

    /// <summary>
    /// 現在位置X
    /// </summary>
    public float X
    {
        get { return locate_x; }
    }

    /// <summary>
    /// 現在位置Y
    /// </summary>
    public float Y
    {
        get { return locate_y; }
    }


    /// <summary>
    /// 位置の更新
    /// </summary>
    /// <param name="ob_time"></param>
    public void update_locate(DateTime ob_time)
    {
        float[] new_locate = calc_orbit(ob_time);
        this.locate_x = new_locate[0];
        this.locate_y = new_locate[1];
    }

    /// <summary>
    /// 乱数
    /// </summary>
    private System.Random rnd;

    public int start = 0;
    
    /// <summary>
    /// 故障率
    /// </summary>
    public double fail = 1 - 0.99920;

    /// <summary>
    /// 故障判定
    /// </summary>
    public Boolean Fail
    {
        get {
            if (rnd.Next(0, 10000) < fail * start * 10000)
            {
                //Destroy(gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// センサの観測範囲
    /// </summary>
    private int sensor_performance = 5;

    public int Sensor_Performance 
    {
        get { return sensor_performance;  }
        set { sensor_performance = value; }
    }

    /// <summary>
    /// Bodyの性能
    /// </summary>
    private double body_performance = 0;

    public double Body_Performance
    {
        get { return body_performance; }
        set { body_performance = value; }
    }


    /// <summary>
    /// 衛星軌道の計算
    /// </summary>
    /// <param name="ob_time"></param>
    /// <returns></returns>
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
        //グリニッジ子午線の赤経計算
        double st0 = calc_greenwich_roll(new DateTime(ob_time.Year, 1, 1, 0, 0, 0));
        double stg = st0 + 1.002737909 * calc_num_day(ob_time);
        stg = 360 * (stg - Math.Floor(stg));
        //緯度・経度の計算
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
        double year_diff = ((double)ob_time.Year - 2000) - Math.Floor(double_et/1000);
        double day_diff = calc_num_day(ob_time) + 1 - (double_et -  Math.Floor(double_et/1000)*1000);
        //double time_diff = year_diff * 365 + day_diff;

        // 軌道修正
        double time_diff = day_diff;
        if (day_diff < 0)
        {
            day_diff += 365;
        }

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

    // その年の1/1 00:00からどれだけの日数が経っているか計算する
    private double calc_num_day(DateTime observe_time)
    {
        //もしJST(日本時刻)ならUSTに直す。(時間から９時間引く)
        DateTime greenwich = new DateTime(observe_time.Year, 1, 1, 0, 0, 0);
        TimeSpan span = observe_time - greenwich;
        //double day_decimal = ((double)observe_time.Hour + (double)observe_time.Minute / 60 + (double)observe_time.Second / 60 / 10) / 24;
        double second = (double)observe_time.Second + (double)observe_time.Millisecond / 1000;
        double minute = (double)observe_time.Minute + second / 60;
        double hour   = (double)observe_time.Hour + minute / 60;    
        double day_decimal = hour / 24;
        double day = double.Parse(span.Days.ToString());
        double num_day = day + day_decimal;
        
        return num_day;
    }

    public virtual void Awake()
    {
        ID = GameMaster.Get_Satellite_ID();
    }

    protected DateTime createtime = GameMaster.GlobalTime;
    public DateTime CreateTime {
        get { return createtime; }
    }
    protected Boolean launch = false;
    public Boolean Launch {
        get { return launch; }
    }

    //コルーチンのスタートとIDの取得
    public virtual void Start()
    {
        rnd = new System.Random(ID + Environment.TickCount);

        SpriteRenderer MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //MainSpriteRenderer.color = Color.black;

        update_locate(observe_time);
        observe_time = observe_time.AddMinutes(15);
        transform.position = new Vector3(locate_x, locate_y, 0);

        //正距円筒による歪みを考慮
        GameObject sensor = gameObject.transform.FindChild("Sensor").gameObject;
        SpriteRenderer sr = sensor.GetComponent<SpriteRenderer>();
        float a = sr.transform.lossyScale.x;

        double h = Math.Cos(locate_y * (2 * (Math.PI / 360)));
        //0割り回避
        float xscale = (h < 0.001) ? 360 : (float)(1.0 / h);
        sr.transform.localScale = new Vector3(xscale * sensor_performance, sensor_performance, 1);

        StartCoroutine("SatObject");
    }




    //コルーチン
    private IEnumerator SatObject()
    {

        CalcNecessaryTime();
        while (true)
        {
            if (launch)
            {
                update_locate(observe_time);
                observe_time = observe_time.AddMinutes(15);

                transform.position = new Vector3(locate_x, locate_y, 0);
                //  GameManager.CalcScore(gameObject);

                //正距円筒による歪みを考慮
                GameObject sensor = gameObject.transform.FindChild("Sensor").gameObject;
                SpriteRenderer sr = sensor.GetComponent<SpriteRenderer>();
                float a = sr.transform.lossyScale.x;

                double h = Math.Cos(locate_y * (2 * (Math.PI / 360)));
                //0割り回避
                float xscale = (h < 0.001) ? 360 : (float)(1.0 / h);
                sr.transform.localScale = new Vector3(xscale * sensor_performance, sensor_performance, 1);

                CalcScore();
            }
            yield return new WaitForSeconds(0.03f);//0.03fで30fpsぐらい
            
        }
    }

    /// <summary>
    /// スコア計算関数
    /// </summary>
    protected virtual void CalcScore() { }


    // Update is called once per frame
    public virtual void Update()
    {
        //スパンの値でタイムスケールの調整
        Time.timeScale = 1.0f * GameMaster.SpanValue;

        if (!launch) 
        {
            LaunchSat();      
        }

        if (observe_time.Year > 1900)
        {
            //update_locate(observe_time);
            //observe_time = observe_time.AddMinutes(10);

            //transform.position = new Vector3(locate_x, locate_y, 0);
            //GameManager.CalcScore(gameObject);

            ////正距円筒による歪みを考慮
            //GameObject sensor = gameObject.transform.FindChild("Sensor").gameObject;
            //SpriteRenderer sr = sensor.GetComponent<SpriteRenderer>();
            //float a = sr.transform.lossyScale.x;
            //double h = Math.Cos(locate_y * (2 * (Math.PI / 360)));
            //float xscale = (float)(1.0 / h);
            //sr.transform.localScale = new Vector3(xscale * 5, 5, 1);

        }

    }

    protected virtual void CalcNecessaryTime() { }
    protected virtual void LaunchSat() { }

    public Boolean sensorOn = false;

    public void Sensor_Switch()
    {
        GameObject sensor = gameObject.transform.FindChild("Sensor").gameObject;
        SpriteRenderer sr = sensor.GetComponent<SpriteRenderer>();

                                                                                                                                                                                                                                                                                                                                                     print(sr.color);

        Color c;
        if (sensorOn)
        {
            c = new Color(1f, 1f, 0f, 0f);
            sensorOn = false;
        }
        else
        {
            c = new Color(1f, 1f, 0f, 0.2f);
            sensorOn = true;
        }

    sr.color = c;

    }


}
