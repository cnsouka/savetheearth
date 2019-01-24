using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class famictrl : MonoBehaviour {

    bool chasingplayer = false;

    public Transform player;

    private Vector3 velocity = Vector3.zero;
    public float movespeed = 10;

    bool iamthinking = false;
    int maxthinkingtime = 2;

    public Vector3 tagretpoint;

    public Transform fx;

    public int myhealth = 10;
    public Transform healthbar;
    bool getdamaging = false;

    public Transform shootthing;
    public float shootdelay = 1;

    public bool stopmove = false;

    enum _famistate
    {
        idle,
        chaseing,
        lookaround,
        shoot,
    }

    _famistate famistate;

    Coroutine iwanttodo;

    // Use this for initialization
    void Start () {

        transform.position = new Vector3(
            player.position.x + Random.insideUnitCircle.x * 10,
            -(player.position.y + Random.insideUnitCircle.y * 10),
            0
            );

        famistate = _famistate.idle;
        chooseapointtogo();

        //shoot!!!
        InvokeRepeating("shootthings", shootdelay, shootdelay);

    }
	
	// Update is called once per frame
	void Update () {

        if (!stopmove)
        {
            if (famistate == _famistate.chaseing)
            {
                float step = movespeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            }
            if (famistate == _famistate.lookaround)
            {
                float step = movespeed * Time.deltaTime * 0.8f;
                transform.position = Vector3.MoveTowards(transform.position, tagretpoint, step);
            }
        }
	
	}

    IEnumerator _choosewhatiwanttodo()
    {

        iamthinking = true;

        //choose one
        int randomchooseone = Random.Range(0, 4);
        switch (randomchooseone)
        {
            case 0:
                famistate = _famistate.idle;

                break;

            case 1:
            case 2:
            case 3:
                famistate = _famistate.lookaround;

                tagretpoint = new Vector3(0,0,0) + Random.insideUnitSphere * 15;
                tagretpoint.z = 0;

                //transform.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitSphere * 100, ForceMode2D.Force);

                break;

            default:
                famistate = _famistate.idle;

                break;
        }

        yield return new WaitForSeconds(Random.Range(1f, maxthinkingtime));

        iamthinking = false;

        //run again!
        chooseapointtogo();

    }

    void chooseapointtogo()
    {
        if (famistate != _famistate.chaseing)
            iwanttodo = StartCoroutine(_choosewhatiwanttodo());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            StopCoroutine(iwanttodo);
            famistate = _famistate.chaseing;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            famistate = _famistate.idle;
            chooseapointtogo();
        }

    }

    public void spawnit(Vector2 pos)
    {

        for (int i = 0; i < Random.Range(5, 10); i++)
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

            Destroy(newrock.gameObject, 2);

        }

        Camera.main.transform.GetComponent<cam_follow>().startshakecam();

        //se
        GameObject.Find("se").transform.GetChild(0).GetComponent<AudioSource>().Play();

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

        }else if (myhealth <= 1)
        {
            destroyme();

            //se
            GameObject.Find("se").transform.GetChild(1).GetComponent<AudioSource>().Play();
        }

    }

    void destroyme()
    {

        transform.GetComponent<flashme>().flashit();

        Camera.main.transform.GetComponent<cam_follow>().startshakecam();

        spawnit(transform.position);
        spawnit(transform.position);
        spawnit(transform.position);

        player.GetComponent<playerctrl>().idestroyafamicom();

        stopmove = true;

        Destroy(gameObject, 2);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //remove force
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        transform.GetComponent<Rigidbody2D>().angularVelocity = 0;

        if (collision.transform.tag == "Player")
        {
            spawnit(collision.transform.position);
        }
        if (collision.transform.tag == "boom")
        {

            spawnit(collision.transform.position);
            getdamage();
            transform.GetComponent<flashme>().flashit();
        }
    }

    void shootthings()
    {

        if (!stopmove)
        {

            if (famistate == _famistate.chaseing)
            {

                GameObject shoot = Instantiate(shootthing.gameObject);
                //newrock.transform.SetParent(transform);

                shoot.transform.position = transform.position + Random.insideUnitSphere * .3f;
                shoot.transform.position = new Vector3(
                    shoot.transform.position.x,
                    shoot.transform.position.y,
                    shootthing.position.z
                );

                var playerDir = transform.position - player.position;
                playerDir.z = 0.0f;
                playerDir = playerDir.normalized;

                shoot.GetComponent<Rigidbody2D>().AddForce(-playerDir * 1.5f, ForceMode2D.Impulse);
                //newrock.GetComponent<ani>().Play();

                Destroy(shoot.gameObject, 120);

                //se
                GameObject.Find("se").transform.GetChild(5).GetComponent<AudioSource>().Play();

            }

        }

    }

}
