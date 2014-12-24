using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Popup_Manager : MonoBehaviour {

    private Vector3 m_pos;
    private float move_y = 0.0f;
    private int Count = 0;
    private bool Down_flag = false;
    private bool paused = true;
    private bool finish = false;

    public float stayTime = 3.0f;
    public int gap = 11;
    private float timer;

    // Use this for initialization
    void Start()
    {
        m_pos = transform.localPosition;  // 形状位置を保持
        timer = stayTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (paused) return;

        //下に出てくる
        if (!Down_flag && Count > 0)
        {
            m_pos.y -= move_y;

            transform.localPosition = m_pos;  // 移動を更新

            Count--;
        }

        //上に戻る
        if (Down_flag && Count < gap)
        {
            m_pos.y += move_y;

            transform.localPosition = m_pos;  // 移動を更新

            Count++;
            if (Count == gap - 1)
            {
                finish = true;
            }
        }

        //タイマー処理
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            timer = 0.0f;

            Down_flag = true;
        }

        

        //終わり
        if (finish)
        {
            Destroy(gameObject);
        }
	}

    public void UP(String s)
    {
        Count = gap;
        move_y = 3f;
        Text t = gameObject.GetComponentInChildren<Text>();
        t.text = s;

        paused = false;
    }
}
