using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	float damageMassScaler = 20;
	float damageVelScaler = 10;

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
			Debug.Log("fire ball  " + collision.collider.tag + " " + damage );
		}
	}
}
