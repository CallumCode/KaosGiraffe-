using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	float damageMassScaler = 20;
	float damageVelScaler = 10;

	public GameObject FirePrefab;

	public int maxFire = 100;
	public int fireCount = 0;

	public float fireSpawnRate = 25;
	float fireSpawmTimer = 0;

	bool spawnFire = true;

	public float maxMass = 30; // this is just a guess atm

	public const float maxTime = 25;
	public const float minTime = 5;
	public float lifeTime = minTime;
	public float startTimer = 0;


	enum FireBallStateType {charging , launched }
	FireBallStateType fireBallState = FireBallStateType.charging;

	void Start () 
	{
		// this means the fireball will fly through enmenies 
		Physics.IgnoreLayerCollision(12,11);
	}
	
	public void LaunchSetup()
	{
		startTimer = Time.time;
		fireBallState = FireBallStateType.launched;

		float t = Mathf.Clamp01(rigidbody.mass / maxMass);
		lifeTime = Mathf.Lerp(minTime, maxTime, t);

	}
		

	// Update is called once per frame
	void Update()
	{
		if (fireBallState == FireBallStateType.launched)
		{

			spawnFire = true;


			if (Time.time > (startTimer + lifeTime))
			{
				DestroySelf();
			}
		}
	}

	void DestroySelf()
	{
		Destroy(gameObject);
	}


	void OnCollisionStay(Collision collision)
	{	

		if(collision.collider.CompareTag("Fire"))
		{
			spawnFire = false;
		}

		if(spawnFire == true && collision.collider.CompareTag("Terrain"))
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
				GameObject fireObject = Instantiate(FirePrefab, pos, FirePrefab.transform.rotation) as GameObject;
				Fire fireScript = fireObject.GetComponent<Fire>();
				fireScript.fireBall = gameObject;

				float t = Mathf.Clamp01( rigidbody.mass / maxMass);

				fireScript.fireDamage = Mathf.Lerp(fireScript.minFireDamage, fireScript.maxFireDamage , t);
				fireScript.GetComponent<ParticleSystem>().startSize = Mathf.Lerp(fireScript.minPartSize , fireScript.maxPartSize , t );
				

				fireObject.transform.localScale =  Vector3.Lerp(fireScript.minScale, fireScript.maxScale , t ) ;
				
 			}
		}



	}




	void OnTriggerStay(Collider collider)
	{
		if (collider.CompareTag("Enemy")
		|| collider.CompareTag("TreeMan")
		|| collider.CompareTag("Swarm"))
		{

			float damage = damageMassScaler * rigidbody.mass;
			damage += damageVelScaler * rigidbody.velocity.magnitude;

			gameObject.GetComponent<Enemy>().TakeDamage(damage);
			//	Debug.Log("fire ball Collision " + collision.collider.tag + " " + damage);
		}



		if (collider.CompareTag("Fire"))
		{
			spawnFire = false; 

		}
		
	}
}

