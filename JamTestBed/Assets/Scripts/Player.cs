using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{

	public enum AttackStateType { IceWall, FireBall }
	public AttackStateType attackState = AttackStateType.IceWall;
	//
	public GameObject IceWallPrefab;

	private Vector3 IceWallStart;
	private Vector3 IceWallEnd;
	private bool IceWallStarted = false;
	public float minWallLength = 1;


	//
	public GameObject FireBallPrefab;

	private GameObject currentFireBall = null;
	private Vector3 FireBallStart;
	private Vector3 FireBallEnd;
	private bool chargeFireBall = false;

	public float fireBallGrowthPerTick= 1;

	public float fireBallTickRate = 1;
	private float firBallGrowthTimer = 0;

	public float fireBallForceScaler = 1;
	
	public float fireBallMasPerTick = 1;
	private float fireBallMass = 0;



	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		AttackStateUpdate();
	}


	void AttackStateUpdate()
	{

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			attackState = AttackStateType.FireBall;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			attackState = AttackStateType.IceWall;
		}

		///
		switch (attackState)
		{
			case AttackStateType.IceWall:
				{
					IceWallUpdate();
				}
				break;

			case AttackStateType.FireBall:
				{
					FireBallUpdate();
				}
				break;
		}		

	}


	void IceWallUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
  			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				IceWallStarted = true;
				IceWallStart = hit.point;
				//Debug.Log("Start POint "  + startPos);
			}
 		}


		if (Input.GetMouseButtonUp(0) && IceWallStarted == true)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				IceWallEnd = hit.point;
			//	Debug.Log("endPos " + endPos);
				CreateIceWall();
			}


			IceWallStarted = false;

		}

	}

	void CreateIceWall()
	{
		IceWallEnd  = new Vector3(IceWallEnd.x, -0.9f, IceWallEnd.z);
		IceWallStart = new Vector3(IceWallStart.x, -0.9f, IceWallStart.z);



		Vector3 midPoint = IceWallStart + (IceWallEnd - IceWallStart) * 0.5f;

		Vector3 dir = (IceWallEnd - IceWallStart);
		dir.Normalize();

		float distance = Vector3.Distance(IceWallEnd, IceWallStart);


		GameObject wall = Instantiate(IceWallPrefab, midPoint, Quaternion.identity) as GameObject;

		wall.transform.right = dir;

		if (distance < minWallLength)
		{
			distance = minWallLength;
		}

		wall.transform.localScale = new Vector3(distance, 1, 1);

	}

	void FireBallUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				chargeFireBall = true;
				FireBallStart = hit.point;
				SpawnFireBall();
 			}
		}

		if (chargeFireBall == true)
		{
			GrowFireBall();
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				FireBallEnd = hit.point;
			}

			if (Input.GetMouseButtonUp(0))
			{		
				chargeFireBall = false;

				LaunchFireBall();

			}

		}

	
	}

	void SpawnFireBall()
	{
		FireBallStart = new Vector3(FireBallStart.x,1, FireBallStart.z);
		currentFireBall = Instantiate(FireBallPrefab, FireBallStart, Quaternion.identity) as GameObject;
		fireBallMass = 0;
	}


	void GrowFireBall()
	{
		if (currentFireBall != null)
		{

			if (Time.time > (firBallGrowthTimer + 1 / fireBallTickRate) )
			{
				firBallGrowthTimer = Time.time;

				currentFireBall.transform.localScale += new Vector3(1,1,1) * fireBallGrowthPerTick;
				fireBallMass += fireBallMasPerTick;

				currentFireBall.transform.position = new Vector3(FireBallStart.x, currentFireBall.transform.localScale.y*0.5f, FireBallStart.z);

			}


		}
		else
		{
			Debug.LogError("trying to grow null fireball");
		}
	}

	void LaunchFireBall()
	{
		Vector3 dir = (FireBallEnd - FireBallStart);
		dir.Normalize();
		float distance = Vector3.Distance(FireBallEnd , FireBallStart);
		currentFireBall.AddComponent<Rigidbody>();
		currentFireBall.rigidbody.mass = fireBallMass;
		currentFireBall.rigidbody.AddForce(dir * distance * fireBallForceScaler);

		currentFireBall = null;
		fireBallMass = 0;

	}
}
