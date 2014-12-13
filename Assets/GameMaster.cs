using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    private static int Satellite_ID = 0;

    public static int Get_Satellite_ID()
    {
        Satellite_ID++;

        return Satellite_ID - 1;
    }
}
