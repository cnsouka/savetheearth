using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerctrl : MonoBehaviour {

    public Transform mouse_curser;

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    public Transform getathing;

    bool startattract = false;
    int attract = 1;
    int attract_max = 100;

    // Use this for initialization
    void Start () {

        Cursor.visible = false;

    }
	
	// Update is called once per frame
	void Update () {

        //move mouse
        Vector3 mouseP = Input.mousePosition;
        mouseP.z = 2.0f;
        mouse_curser.position = Camera.main.ScreenToWorldPoint(mouseP);

        //gogogo
        moveme();

        //thing around me
        if (getathing)
        {
            getathing.RotateAround(transform.position, Vector3.forward, attract * 0.04f * 200 * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            startattract = true;

        }
        if (Input.GetMouseButtonUp(0))
        {

            if (getathing)
            {

                var mouseDir = mouse_curser.position - getathing.position;
                mouseDir.z = 0.0f;
                mouseDir = mouseDir.normalized;

                getathing.GetComponent<Rigidbody2D>().AddForce(-mouseDir, ForceMode2D.Impulse);
            }
            startattract = false;
            getathing = null;
        }

        if (startattract)
        {
            attract += 1;
            if (attract >= attract_max)
                attract = attract_max;
        }
        else
        {
            attract -= 8;
            if (attract <= 1)
                attract = 1;
        }

    }

    void moveme()
    {

        //move me
        Vector3 point = Camera.main.WorldToViewportPoint(mouse_curser.position);
        Vector3 delta =
            mouse_curser.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        Vector3 destination = transform.position + delta;
        destination.z = 0;

        transform.position =
            Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, 0), destination, ref velocity, dampTime);

    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!getathing && startattract)
            if (collision.tag == "rock")
                getathing = collision.transform;

        //transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);

    }


}
