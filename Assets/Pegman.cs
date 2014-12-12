using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
    public float x;
    public float y;
	// Use this for initialization
	void Start () {
        x = gameObject.transform.position.x;
        y = gameObject.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        x = gameObject.transform.position.x;
        y = gameObject.transform.position.x;
	}
}
