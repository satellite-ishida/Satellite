using UnityEngine;
using System.Collections;
using System;

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
        Node_Info si = gameObject.GetComponent<Node_Info>();
        Info_Panel_Manager.target_Change(si.ID);
        SatelliteComponent sc_new = GameMaster.GetSatelliteByID(ID).GetComponent<SatelliteComponent>();

        if (GUI_Manager.SensorON_ID != ID)
        {
            SatelliteComponent sc_old = GameMaster.GetSatelliteByID(GUI_Manager.SensorON_ID).GetComponent<SatelliteComponent>();

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
        else if (GUI_Manager.SensorON_ID == ID)
        {
            sc_new.Sensor_Switch();
        }

        GUI_Manager.SensorON_ID = ID;
    }
}
