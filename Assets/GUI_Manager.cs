using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GUI_Manager : MonoBehaviour
{
    static public int SensorON_ID = 0; 

    //スライダーの値が変更されたとき
    public void Set_Slider_Event()
    {
        GameObject span = GameObject.Find("Span");
        Slider s = span.GetComponent<Slider>();
        GameMaster.SpanValue = s.value;

        GameObject spanText = GameObject.Find("SpanText");
        Text st = spanText.GetComponent<Text>();
        st.text = "Time Scale : " + s.value.ToString();
    }
    //アドバンスパネルを前面に
    public void Set_Advanced_Panel()
    {
        GameObject g = GameObject.Find("Input_SatComp_Panel");
        g.transform.SetAsLastSibling();
        //ペグマンをですとろい
        GameObject Pegman = GameObject.Find("Pegman(Clone)");
        Destroy(Pegman);
    }
    //イージーパネルを前面に
    public void Set_Easy_Panel()
    {
        GameObject g = GameObject.Find("Input_Easy_Sat_Panel");
        g.transform.SetAsLastSibling();
        GameObject g2 = GameObject.Find("CreateSat");
        g2.transform.SetAsLastSibling();
    }
    //Create_Satパネルにチェンジ
    public void Set_Create_Sat_Panel()
    {
        GameObject g = GameObject.Find("Create_Sat_Panel");
        g.transform.SetAsLastSibling();
    }
    //Main_GUIパネルにチェンジ
    public void Set_Main_GUI_Panel()
    {
        GameObject g = GameObject.Find("Main_GUI_Panel");
        g.transform.SetAsLastSibling();
    }
    //Eventパネルを前面に
    public void Set_Event_Panel()
    {
        GameObject g = GameObject.Find("Event_Panel");
        g.transform.SetAsLastSibling();

        GameMaster.POPUP("test");
    }
    //Sat_Listパネルを前面に
    public void Set_Sat_List_Panel()
    {
        GameObject g = GameObject.Find("Sat_List_Panel");
        g.transform.SetAsLastSibling();
    }
    //CreateSatを前面に
    public void Set_CreateSat()
    {
        GameObject g = GameObject.Find("CreateSat");
        g.transform.SetAsLastSibling();
    }
    //CreateBaseを前面に
    public void Set_CreateBase()
    {
        print("createBase");
        GameObject g = GameObject.Find("CreateBase");
        g.transform.SetAsLastSibling();
    }

    //Sat_Listにノード追加
    static public void Add_Sat_Node(SatelliteComponent sc)
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Sat_Node");
        //GameObject node = Instantiate(prefab) as GameObject;
        var item = GameObject.Instantiate(prefab.transform) as RectTransform;
        Node_Info si = item.GetComponent<Node_Info>();
        si.set_Info(sc);


        GameObject self = GameObject.Find("Sat_List");
        item.SetParent(self.transform, false);

        var text = item.GetComponentInChildren<Text>();
        text.text = "ID:" + sc.ID.ToString();
    }
    //Sat_Listの衛星ノードを削除（衛星IDで判別）
    public static void Destroy_Sat_Node(int ID)
    {
        GameObject g = GameObject.Find("Sat_List");
        var item = g.transform as RectTransform;
        foreach (RectTransform child in item)
        {
            if (child.GetComponent<Node_Info>().ID == ID) Destroy(child.gameObject);
            break;
        }     
    }
    //Sat_Listの衛星ノードを削除（List<GameObjevt>で判別）
    public static void Destroy_Sat_Node(List<GameObject> gl)
    {
        GameObject g = GameObject.Find("Sat_List");
        var item = g.transform as RectTransform;


        foreach (GameObject gl_node in gl) 
        {
            foreach (RectTransform item_node in item)
            {
                if (gl_node.GetComponent<SatelliteComponent>().ID
                    == item_node.GetComponent<Node_Info>().ID)
                {
                    Destroy(item_node.gameObject);
                    break;
                }
            }

        }     
    }
    //Sat_Listの衛星ノードを削除（GameObjevtで判別）
    public static void Destroy_Sat_Node(GameObject gl)
    {
        GameObject g = GameObject.Find("Sat_List");
        var item = g.transform as RectTransform;

        int ID = gl.GetComponent<SatelliteComponent>().ID;

        foreach (RectTransform child in item)
        {
            if (child.GetComponent<Node_Info>().ID == ID) Destroy(child.gameObject);
            break;
        }    
    }


    public static void Set_Log(String s)
    {
        GameMaster.Add_Log(s);
        Queue<String> qs = GameMaster.Get_Log();
        GameObject g = GameObject.Find("Log_Text");
        Text t = g.GetComponent<Text>();
        String sb = "";
        foreach (String log in qs)
        {
            sb = sb + log + "\n";
        }
        t.text = sb;
    }
}
    
