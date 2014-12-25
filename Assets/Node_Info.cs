using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Node_Info : MonoBehaviour {

    /// <summary>
    /// 衛星のID
    /// </summary>
    public int ID;

    public void set_Info(SatelliteComponent sc)
    {
        ID = sc.ID;
    }

    //リスト中のノードをクリックしたとき
    public void Click_Sat_Node()
    {
        //クリックした衛星を光らせる
        Node_Info si = gameObject.GetComponent<Node_Info>();
        Info_Panel_Manager.target_Change(si.ID);
        SatelliteComponent sc_new = GameMaster.GetSatelliteByID(ID).GetComponent<SatelliteComponent>();

        if (GUI_Manager.SensorON_ID != ID)
        {
            GameObject g_old = GameMaster.GetSatelliteByID(GUI_Manager.SensorON_ID);
            SatelliteComponent sc_old = null;
            //g_oldがデストロイされているか判断
            if (g_old != null)
            {
                sc_old = g_old.GetComponent<SatelliteComponent>();

                if (sc_old.sensorOn)
                {
                    sc_old.Sensor_Switch();
                    sc_new.Sensor_Switch();
                }
                else if (!sc_old.sensorOn)
                {
                    sc_new.Sensor_Switch();
                }
            }
            else
            {
                sc_new.Sensor_Switch();
            }
        }
        else if (GUI_Manager.SensorON_ID == ID)
        {
            sc_new.Sensor_Switch();
        }

        GUI_Manager.SensorON_ID = ID;

        //Infoパネルに情報表示
        GameObject info = GameObject.Find("Sat_Info");
        Text type = GameObject.Find("Sat_type").GetComponent<Text>();
        Text launch = GameObject.Find("launch_time").GetComponent<Text>();
        Text body = GameObject.Find("Body_param").GetComponent<Text>();
        Text sensor = GameObject.Find("Sensor_range").GetComponent<Text>();

        if(GameMaster.GetSatelliteByID(ID).GetComponent<GPS_Satellite>())
        {
            type.text = "type : GPS";
        }
        else if(GameMaster.GetSatelliteByID(ID).GetComponent<Weather_Satellite>())
        {
            type.text = "type : Weather";
        }
        else if(GameMaster.GetSatelliteByID(ID).GetComponent<Broadcasting_Satellite>())
        {
            type.text = "type : BS";
        }

        launch.text = "launch : "+sc_new.CreateTime.ToString();
        body.text = "body : "+sc_new.Body_Performance.ToString();
        sensor.text = "sensor : "+sc_new.Sensor_Performance.ToString();
    }
}
