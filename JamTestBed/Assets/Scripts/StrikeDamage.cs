using UnityEngine;
using System.Collections;

public class StrikeDamage : MonoBehaviour 
{
	public float damage = 50;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	 
	}

	void OnTriggerStay(Collider collider)
	{

		if (collider.CompareTag("Enemy")
			|| collider.CompareTag("TreeMan")
			|| collider.CompareTag("Swarm"))
		{


			collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
			Debug.Log("Strike hit" + collider.tag);
		}
	}
}
