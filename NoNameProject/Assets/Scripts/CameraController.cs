using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject followTarget;

	void Start ()
	{
	    if (followTarget == null)
	    {
            followTarget = GameObject.FindWithTag("Player");
	    }
	}
	
	void Update () {
        transform.position = new Vector3(followTarget.transform.position.x, transform.position.y, transform.position.z);
	}

}
