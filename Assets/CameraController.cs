using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {
    void Update()
    {
        GameObject camera = GameObject.Find("Main Camera");
        Camera component = camera.GetComponent<Camera>();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        component.orthographicSize += Input.GetAxis("Mouse ScrollWheel")*20;
        {
                    transform.position = mousePos;
           
        }

        //transform.Translate(new Vector3(0, 0, -10));
        

    }
}
