using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{

	private GameObject TowerObject;
	private Tower TowerScript;

	float health = 100;
	float damage = 1;
	float attacksRate = 1;
	public	float moveSpeed = 10;
	float turnSpeed = 1;
	private float attackTimer = 0;

	CharacterController characterCont;
	Vector3 target;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Movement();
	}

public 	void Movement()
	{

		Vector3 targetDir = target - transform.position;
		float step = turnSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);

		characterCont.Move(transform.forward*moveSpeed*Time.deltaTime);

	}


	public void SetUp(GameObject towerObj)
	{
		TowerObject = towerObj;
		TowerScript = TowerObject	.GetComponent<Tower>();

		target = towerObj.transform.position;
		target = new Vector3(target.x, transform.position.y , target.z);

		characterCont = GetComponent<CharacterController>();
	}


	void OnControllerColliderHit(ControllerColliderHit collisionInfo)
	{
 

		if(collisionInfo.collider.CompareTag("Tower"))
		{
 
			if(Time.time > (attackTimer + 1 / attacksRate) )
			{
				attackTimer = Time.time;
				TowerScript.TakeDamage(damage);
			}
		} 
	}

}
