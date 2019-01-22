using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwansomerock : MonoBehaviour {

    public int rock_max = 20;
    public int spawndelay = 2;

    public Transform rocklist;

	// Use this for initialization
	void Start () {

        spawnit();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spawnit()
    {
        //rock
        for (int i = 0; i < rock_max; i++)
        {

            GameObject newrock = Instantiate(rocklist.GetChild( Random.Range(0,rocklist.childCount) ).gameObject);
            newrock.transform.SetParent(transform);

            newrock.transform.position = Random.insideUnitCircle * transform.GetComponent<CircleCollider2D>().radius;
            newrock.transform.position = new Vector3(
                newrock.transform.position.x,
                newrock.transform.position.y,
                rocklist.GetChild(Random.Range(0, rocklist.childCount)).position.z
            );

        }


    }

}
