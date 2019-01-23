using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destorymeafterawhile : MonoBehaviour {

    public int destroytime = 10;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroytime);
	}
	
}
