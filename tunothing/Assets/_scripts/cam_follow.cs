using UnityEngine;
using System.Collections;

public class cam_follow : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

    public float bumpThreshold = 1;

    public Camera cam;

	public Vector3 cam_offect;

	void Start(){



	}

	// Update is called once per frame
	void Update () 
	{
		if (target)
		{
			Vector3 point = cam.WorldToViewportPoint(target.position);
			Vector3 delta = 
				target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
			
			Vector3 destination = transform.position + delta + cam_offect;

			transform.position =
				Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

	}
}
