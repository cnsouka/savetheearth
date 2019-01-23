using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endctrl : MonoBehaviour {


    public void gotointro()
    {
        SceneManager.LoadScene("intro", LoadSceneMode.Single);
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gotointro();
        }
    }
}
