using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introctrl : MonoBehaviour {

	public void gotomain()
    {
        SceneManager.LoadScene("main", LoadSceneMode.Single);
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gotomain();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
