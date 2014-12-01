using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour 
{

	public Transform target = null;

	public Vector3 offset = Vector3.zero;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(target != null)
		{
			transform.LookAt(target.position + offset);
		}
	}
}
