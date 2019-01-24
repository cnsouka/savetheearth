using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gotoblack : MonoBehaviour {

    public bool start = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (start)
        {
            transform.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0,1,Time.time);
        }
        else
        {
            transform.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, Time.time);
        }
    }
}
