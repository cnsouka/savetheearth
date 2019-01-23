using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchsomething : MonoBehaviour {

    public Transform fx;

    public void spawnit(Vector2 pos)
    {

        for (int i = 0; i < Random.Range(5,10); i++)
        {

            GameObject newrock = Instantiate(fx.gameObject);
            //newrock.transform.SetParent(transform);

            newrock.transform.position = pos + Random.insideUnitCircle * 0.1f;
            newrock.transform.localScale *= Random.Range(0.7f, 2f);
            newrock.transform.position = new Vector3(
                newrock.transform.position.x,
                newrock.transform.position.y,
                fx.position.z
            );
            //newrock.GetComponent<ani>().Play();

            Destroy(newrock.gameObject,2);

        }

        Camera.main.transform.GetComponent<cam_follow>().startshakecam();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (transform.tag == "card" && collision.transform.tag=="fami")
            return;

        spawnit(collision.transform.position);
        Destroy(transform.gameObject);
        if (collision.transform.tag == "rock")
        {
            Destroy(collision.gameObject);
        }
    }

}
