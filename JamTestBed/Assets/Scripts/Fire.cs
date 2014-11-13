using UnityEngine;
using System.Collections;

public class Fire: MonoBehaviour 
{

	public float lifeTime = 5;
	private float timer;

	public GameObject fireBall = null;
	
	public float fireDamge = 50;

	// Use this for initialization
	void Start ()
	{
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{


		if( Time.time > (timer + lifeTime) )
		{


			DesotrySelf();
		}
	
	}

	void DesotrySelf()
	{
		if (fireBall != null)
		{
			fireBall.GetComponent<FireBall>().fireCount--;
		}


		Destroy(gameObject);
	}

	void OnTriggerStay(Collider collider)
	{

		if (collider.CompareTag("Enemy")
			|| collider.CompareTag("TreeMan")
			|| collider.CompareTag("Swarm"))
		{


			collider.gameObject.GetComponent<Enemy>().TakeDamage(fireDamge * Time.deltaTime);
			Debug.Log("fire  burn " + collider.tag);
		}
	}


}
