using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	//public GameObject FirePrefab;

	//public float maxFireRate = 5;
//	private float fireTimer = 0;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
/*
	void OnCollisionStay(Collision collision)
	{
		if (Time.time > (fireTimer + 1 / maxFireRate) && FirePrefab != null)
		{
			fireTimer = Time.time;
			foreach (ContactPoint contact in collision.contacts)
			{
				Vector3 pos = contact.point;
				Quaternion rot = Quaternion.FromToRotation(Vector3.right, contact.normal);
				GameObject fire = Instantiate(FirePrefab, pos, rot) as GameObject;

				ParticleSystem pSystem = fire.GetComponent<ParticleSystem>();
				pSystem.startSize = transform.localScale.x * 0.5f;
				pSystem.startLifetime = transform.localScale.x * 2;
				pSystem.startSpeed = transform.localScale.x * 2;
				SerializedObject so = new SerializedObject(pSystem);
				so.FindProperty("ShapeModule.radius").floatValue = transform.localScale.x*0.5f;
				

				so.ApplyModifiedProperties();

 			}
		}
	}*/
}
