using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Info_Panel_Manager : MonoBehaviour {


    static private int target_ID = 0;
    private static GameObject target_Sat = null;
    public static void target_Change(int ID)
    {
        target_ID = ID;
        GameObject g = GameMaster.GetSatelliteByID(ID);
        if (g != null)
        {
            target_Sat = g;
        }
        else
        {
            target_Sat = null;
        }
    }

    /*
     * mode 0 : Satellite info
     *      1 : 
     */
    private int info_mode = 0;
    public void mode_change_SatInfo()
    {
        info_mode = 0;
    }

	void Start () {
        target_Change(0);
	}
	
	// Update is called once per frame
	void Update () {

        if (info_mode == 0)
        {
            Sat_Info();
        }
	}

    //衛星の情報を表示
    private void Sat_Info()
    {
        GameObject g = GameObject.Find("Info_Panel/Sat_Info");
        g.transform.SetAsLastSibling();
        if (target_Sat != null)
        {
            SatelliteComponent sc = target_Sat.GetComponent<SatelliteComponent>();
            //Text t = g.GetComponent<Text>();
            Text t = g.GetComponentInChildren<Text>();
            t.text = sc.X.ToString()+" , "+sc.Y.ToString();
        }
    }
}
