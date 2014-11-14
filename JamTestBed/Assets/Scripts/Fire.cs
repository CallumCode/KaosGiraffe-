using UnityEngine;
using System.Collections;

public class Fire: MonoBehaviour 
{

	public float lifeTime = 10;
	private float timer =0;

	public GameObject fireBall = null;
	
	public float fireDamage = 50;
	public float minFireDamage = 50;
	public float maxFireDamage = 200;

	public readonly Vector3 minScale = new Vector3(1, 1, 1);
	public readonly Vector3 maxScale = new Vector3(10, 10, 10);

	public readonly float minPartSize = 1;
	public readonly float maxPartSize = 10;

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


			collider.gameObject.GetComponent<Enemy>().TakeDamage(fireDamage * Time.deltaTime);
			//Debug.Log("fire  burn " + collider.tag);
		}
	}


}
