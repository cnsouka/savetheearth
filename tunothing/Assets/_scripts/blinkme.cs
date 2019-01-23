using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blinkme : MonoBehaviour {

    public float flashdelay = 3;

	
	// Update is called once per frame
	void Update () {

        transform.GetComponent<CanvasGroup>().alpha = Mathf.PingPong(flashdelay * Time.time, 1);

    }
}
