using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Popup_Script : MonoBehaviour {

    private Vector3 m_pos;
    // Use this for initialization
    void Start()
    {
        m_pos = transform.localPosition;  // 形状位置を保持
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UP(String s)
    {
        Text t = gameObject.GetComponentInChildren<Text>();
        t.text = s;

        m_pos.x += 5f;
        transform.localPosition = m_pos;  // 移動を更新
    }
}
