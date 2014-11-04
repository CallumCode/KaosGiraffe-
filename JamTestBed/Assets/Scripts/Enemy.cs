using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{

	private GameObject TowerObject;
	private Tower TowerScript;

	float health = 100;
	protected float damage;
	protected float attacksRate;
	protected float moveSpeed;
	protected float turnSpeed;
	private float attackTimer = 0;

	protected CharacterController characterCont;
	protected Vector3 target;

	protected Transform theTransform;

	// Use this for initialization
	void Start () 
	{
		damage = 1;
		attacksRate = 2;
		moveSpeed = 10;
		turnSpeed = 1;
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
		Debug.DrawRay(transform.position, newDir, Color.blue);
		theTransform.rotation = Quaternion.LookRotation(newDir);

		characterCont.Move(transform.forward*moveSpeed*Time.deltaTime);

	}


	public void SetUp(GameObject towerObj)
	{
		TowerObject = towerObj;
		TowerScript = TowerObject	.GetComponent<Tower>();

		target = towerObj.transform.position;
		target = new Vector3(target.x, transform.position.y , target.z);

		characterCont = GetComponent<CharacterController>();


		// cache it as transform acutaly is getComponet and will be called a lot
		theTransform = transform;
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
