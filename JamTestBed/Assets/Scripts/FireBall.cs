using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	float damageMassScaler = 20;
	float damageVelScaler = 10;

	public GameObject FirePrefab;

	public int maxFire = 100;
	public int fireCount = 0;

	float fireSpawnRate = 10;
	float fireSpawmTimer = 0;

	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}




	void OnCollisionStay(Collision collision)
	{

		if (collision.collider.CompareTag("Enemy")
			|| collision.collider.CompareTag("TreeMan")
			|| collision.collider.CompareTag("Swarm"))
		{

			float damage = damageMassScaler * rigidbody.mass;
			damage += damageVelScaler * rigidbody.velocity.magnitude;

			collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
		//	Debug.Log("fire ball Collision " + collision.collider.tag + " " + damage);
		}


		if (collision.collider.CompareTag("Terrain"))
		{
			foreach (ContactPoint contactPoint in collision.contacts)
			{
				SpawnFire(contactPoint.point);
			}
		}


	}

	void SpawnFire(Vector3 pos)
	{
		if(fireCount < maxFire)
		{
			if (Time.time > (fireSpawmTimer + 1 / fireSpawnRate))
			{
				fireSpawmTimer = Time.time;
				fireCount++;
				GameObject fire = Instantiate(FirePrefab, pos, FirePrefab.transform.rotation) as GameObject;
				fire.GetComponent<Fire>().fireBall = gameObject;
			}
		}

	}

}
