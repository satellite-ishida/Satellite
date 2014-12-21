﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


/// <summary>
/// セルの情報を保持
/// </summary>
public class Cell_Data
{

    private Boolean land;
    /// <summary>
    /// 陸の判定用(TRUE:陸,FALSE:海)。
    /// </summary>
    public Boolean Land
    {
        get { return land; }
        set { land = value; }

    }


    private string country;
    /// <summary>
    /// 国名
    /// </summary>
    public string Country
    {
        get { return country; }
        set { country = value; }

    }


    private string city;
    /// <summary>
    /// 都市名
    /// </summary>
    public string City
    {
        get { return city; }
        set { city = value; }

    }

    private bool ground_station = false;
    /// <summary>
    /// 基地局
    /// </summary>
    public bool GS
    {
        get { return ground_station; }
        set { ground_station = value; }

    }

    private bool observe_status;
    private DateTime observe_time;
    /// <summary>
    /// セルの観測状況
    /// </summary>
    public bool Observe_Status
    {
        get { return observe_status; }
        set 
        { 
            observe_status = value;
            observe_time = DateTime.Now;
        }
    }

    public DateTime ObserveTime
    {
        get { return observe_time; }
    }

}

