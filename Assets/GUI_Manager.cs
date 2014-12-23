using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GUI_Manager : MonoBehaviour
{

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

    private int target_ID = 0;
    public void Click_Sat_Node()
    {
        print(gameObject.name);
    }

    //Sat_Listにノード追加
    static public void Add_Sat_Node(SatelliteComponent sc)
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Sat_Node");
        var item = GameObject.Instantiate(prefab.transform) as RectTransform;
        Sat_Info si = prefab.GetComponent<Sat_Info>();
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
            if (child.GetComponent<Sat_Info>().ID == ID) Destroy(child.gameObject);
            break;
        }     
    }
    //Sat_Listの衛星ノードを削除（GameObjevtで判別）
    public static void Destroy_Sat_Node(List<GameObject> gl)
    {
        GameObject g = GameObject.Find("Sat_List");
        var item = g.transform as RectTransform;
        foreach (GameObject child in gl)
        {
            if (child.GetComponent<SatelliteComponent>().Fail) Destroy(child.gameObject);
            break;
        }
    }
}
    
