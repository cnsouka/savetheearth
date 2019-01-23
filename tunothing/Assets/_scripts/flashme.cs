using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashme : MonoBehaviour {

    public float flashdelay = 20;
    public float flashtime = 1;
    bool flashing = false;

    public bool autostart = false;

    public void flashit()
    {
        if (!flashing) StartCoroutine(_flashit());
    }

    private void Start()
    {
        if (autostart)
            flashit();
    }

    private void Update()
    {
        if (flashing)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(
                transform.GetComponent<SpriteRenderer>().color.r,
                transform.GetComponent<SpriteRenderer>().color.g,
                transform.GetComponent<SpriteRenderer>().color.b,
                Mathf.PingPong(flashdelay * Time.time, 1)
            );
        }

    }

    IEnumerator _flashit()
    {

        flashing = true;

        yield return new WaitForSeconds(flashtime);

        flashing = false;

        transform.GetComponent<SpriteRenderer>().color = new Color(
            transform.GetComponent<SpriteRenderer>().color.r,
            transform.GetComponent<SpriteRenderer>().color.g,
            transform.GetComponent<SpriteRenderer>().color.b,
            1
        );

    }

}
