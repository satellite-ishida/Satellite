using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

//スプライトをマウスドラッグで動かすためのスクリプト
public class Sprite_Mouse_Controller : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;


    void OnMouseDown()
    {
        //カメラから見たオブジェクトの現在位置を画面位置座標に変換
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        //取得したscreenPointの値を変数に格納
        float _x = Input.mousePosition.x;
        float _y = Input.mousePosition.y;

        //オブジェクトの座標からマウス位置(つまりクリックした位置)を引いている。
        //これでオブジェクトの位置とマウスクリックの位置の差が取得できる。
        //ドラッグで移動したときのずれを補正するための計算だと考えれば分かりやすい
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(_x, _y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        //ドラッグ時のマウス位置を変数に格納
        float _x = Input.mousePosition.x;
        float _y = Input.mousePosition.y;

        //ドラッグ時のマウス位置をシーン上の3D空間の座標に変換する
        Vector3 currentScreenPoint = new Vector3(_x, _y, screenPoint.z);

        //上記にクリックした場所の差を足すことによって、オブジェクトを移動する座標位置を求める
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;

        //オブジェクトの位置を変更する
        transform.position = currentPosition;

        //オブジェクトの移動をGUIに知らせる
        if (currentPosition.x > 180 || currentPosition.x < -180 || currentPosition.y > 90 || currentPosition.y < -90) { }
        else
        {
            GameObject text_lat = GameObject.Find("Put_lattitude");
            GameObject text_log = GameObject.Find("Put_longitude");
            Text t1 = text_lat.GetComponent<Text>();
            t1.text = transform.position.y.ToString();
            Text t2 = text_log.GetComponent<Text>();
            t2.text = transform.position.x.ToString();
        }
    }
}
