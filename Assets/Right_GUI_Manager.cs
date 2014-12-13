using UnityEngine;
using System.Collections;

public class Right_GUI_Manager: MonoBehaviour {

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

    }
}
