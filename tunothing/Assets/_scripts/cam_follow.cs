using UnityEngine;
using System.Collections;

public class cam_follow : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

    public float bumpThreshold = 1;

    public Camera cam;

	public Vector3 cam_offect;

    public float shakeduration = 1;
    public float shakeamount = 10;
    bool shaking = false;

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

            if (shaking)
            {
                for (int i = 0; i < Random.Range(1,4); i++)
                {
                    Vector3 shakedestination = new Vector3(
                        destination.x + Random.insideUnitSphere.x * shakeamount,
                        destination.y + Random.insideUnitSphere.y * shakeamount,
                        destination.z
                    ) + delta + cam_offect;

                    transform.position = Vector3.SmoothDamp(transform.position, shakedestination, ref velocity, 0.1f);
                }
            }

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);


		}

	}

    public void startshakecam()
    {
        if (!shaking)
        {
            StartCoroutine(_shakingcam());
        }
    }

    IEnumerator _shakingcam()
    {

        shaking = true;

        yield return new WaitForSeconds(shakeduration);

        shaking = false;

    }

}
