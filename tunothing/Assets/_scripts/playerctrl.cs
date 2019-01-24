using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerctrl : MonoBehaviour
{

    public Transform mouse_curser;

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    public Transform getathing;

    bool startattract = false;
    int attract = 1;
    int attract_max = 50;

    public int myhealth = 3;
    public Transform healthbar;
    bool getdamaging = false;

    public Transform firstaid;

    public int destroyfamicoms = 0;

    public bool stopmove = false;

    // Use this for initialization
    void Start()
    {

        Cursor.visible = false;

        for (int i = 0; i < Random.Range(5, 10); i++)
        {

            GameObject newrock = Instantiate(firstaid.gameObject);
            //newrock.transform.SetParent(transform);

            newrock.transform.position = Random.insideUnitCircle * 10;
            newrock.transform.position = new Vector3(
                newrock.transform.position.x,
                newrock.transform.position.y,
                firstaid.position.z
            );
            //newrock.GetComponent<ani>().Play();

        }

    }

    public void idestroyafamicom()
    {
        destroyfamicoms++;

        if (destroyfamicoms >=2)
        {
            StartCoroutine(_changescene("end"));
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!stopmove)
        {

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

                //se
                if (!GameObject.Find("se").transform.GetChild(3).GetComponent<AudioSource>().isPlaying)
                {
                    GameObject.Find("se").transform.GetChild(3).GetComponent<AudioSource>().Play();
                }

            }
            if (Input.GetMouseButtonUp(0))
            {

                GameObject.Find("se").transform.GetChild(3).GetComponent<AudioSource>().Stop();

                if (getathing)
                {

                    var playerDir = transform.position - getathing.position;
                    playerDir.z = 0.0f;
                    playerDir = playerDir.normalized;

                    getathing.GetComponent<Rigidbody2D>().AddForce(-playerDir * attract * 0.015f, ForceMode2D.Impulse);
                    getathing.tag = "boom";

                    //se
                    GameObject.Find("se").transform.GetChild(5).GetComponent<AudioSource>().Play();

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("intro", LoadSceneMode.Single);
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
            if (collision.tag == "rock" || collision.tag == "card")
                getathing = collision.transform;

        //transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //remove force
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        transform.GetComponent<Rigidbody2D>().angularVelocity = 0;

        if (collision.transform.tag == "fami" || collision.transform.tag == "rock" || collision.transform.tag == "card")
        {
            getdamage();
            transform.GetComponent<flashme>().flashit();

        }

        if (collision.transform.tag == "aid")
        {

            //se
            GameObject.Find("se").transform.GetChild(2).GetComponent<AudioSource>().Play();

            Destroy(collision.gameObject);

            if (myhealth < 3)
            {
                myhealth++;

                GameObject newbar = Instantiate(healthbar.GetChild(healthbar.childCount - 1).gameObject);
                newbar.transform.position = new Vector3(
                    healthbar.GetChild(healthbar.childCount - 1).transform.position.x + 0.04f * healthbar.GetChild(healthbar.childCount - 1).parent.localScale.x,
                    healthbar.GetChild(healthbar.childCount - 1).transform.position.y,
                    0
                    );
                newbar.transform.localScale = healthbar.GetChild(healthbar.childCount - 1).parent.localScale;
                newbar.transform.parent = healthbar.GetChild(healthbar.childCount - 1).parent;

                healthbar.GetChild(healthbar.childCount - 1).GetComponent<flashme>().flashit();
            }
        }

    }

    void getdamage()
    {
        if (!getdamaging) StartCoroutine(_getdamage());
    }
    IEnumerator _getdamage()
    {

        if (myhealth > 0)
        {

            getdamaging = true;

            myhealth--;

            healthbar.GetChild(healthbar.childCount - 1).GetComponent<flashme>().flashit();

            Destroy(healthbar.GetChild(healthbar.childCount - 1).gameObject, healthbar.GetChild(healthbar.childCount - 1).GetComponent<flashme>().flashtime);

            yield return new WaitForSeconds(healthbar.GetChild(healthbar.childCount - 1).GetComponent<flashme>().flashtime + 0.01f);

            getdamaging = false;

        }
        else if (myhealth <=0)
        {
            stopmove = true;

            StartCoroutine(_changescene("intro"));

            //se
            GameObject.Find("se").transform.GetChild(1).GetComponent<AudioSource>().Play();

        }

    }

    IEnumerator _changescene(string scene)
    {

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(scene, LoadSceneMode.Single);

    }

}

