﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{

	public enum AttackStateType { IceWall, FireBall, Lightning }
	public AttackStateType attackState = AttackStateType.IceWall;
	//
	public GameObject IceWallPrefab;

	private Vector3 IceWallStart;
	private Vector3 IceWallEnd;
	private bool IceWallStarted = false;
	public float minWallLength = 1;

	public float wallInitialManaCost = 5;
	public float wallLengthManCost = 5;

	//
	public GameObject FireBallPrefab;

	private GameObject currentFireBall = null;
	private Vector3 FireBallStart;
	private Vector3 FireBallEnd;
	private bool chargeFireBall = false;

	public float fireBallGrowthPerTick= 1;

	public float fireBallTickRate = 1;
	private float fireBallGrowthTimer = 0;

	public float fireBallForceScaler = 1;
	
	public float fireBallMasPerTick = 1;
	private float fireBallMass = 1;

	public float fireBallManaCost = 5;
	public float fireBallInitalManaCost = 5;


	//

	public GameObject LightningPrefab;
	private GameObject currentLightning = null;

	private Vector3 lightningStart;
	private Vector3 lightningEnd;

	private bool growingLightning = false;

	public float lightningHeight = 15;

	public float lightningManaCost = 1;
	public float lightningInitalManaCost = 5;


	//

	const float maxMana = 100;
	const float minMana = 0;
	public float mana = maxMana;

	public float manaRegain = 5;

	public GameObject ManaStatBarObject;
	private StatBar ManaStatBarScript;

	private LineRenderer helpLine;
	public Material fireBallHelpMat	;
	public Material iceWallHelpMat	;

	public float maxHelpWidth = 15;
	public float minHelpWidth = 1;
	public float helpWidth = 1;

	// Use this for initialization
	void Start () 
	{
		helpLine = GetComponent<LineRenderer>();

		ManaStatBarScript = ManaStatBarObject.GetComponent<StatBar>();
		ManaStatBarScript.SetStat(mana / maxMana);
	}
	
	// Update is called once per frame
	void Update () 
	{
		AttackStateUpdate();

		ChangeMana(-manaRegain * Time.deltaTime);
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

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			attackState = AttackStateType.Lightning;
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

			case AttackStateType.Lightning:
				{
					LightningUpdate();
				}
				break;
		}		

	}



	void LightningUpdate()
	{


		if (growingLightning  == false && Input.GetMouseButtonDown(0))
		{
			if ((mana - lightningInitalManaCost) > minMana)
			{
				ChangeMana(lightningInitalManaCost);
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					lightningStart = hit.point;
					growingLightning = true;
					SpawnLightning();
				}
			}
			else
			{
				NotEnoughMana();
			}
		}


		if (growingLightning)
		{
			bool skipNoMana = false;

			if ((mana - lightningManaCost * Time.deltaTime) > minMana)
			{
				ChangeMana(lightningManaCost * Time.deltaTime);
			}
			else
			{
				skipNoMana = true;
				NotEnoughMana();
			}


			if (Input.GetMouseButtonUp(0) || skipNoMana == true)
			{
				Lightning lightning = currentLightning.GetComponent<Lightning>();

				lightning.FinishGrowing();

				currentLightning = null;
				growingLightning = false;
			}

		}
	}

	void SpawnLightning()
	{
		lightningStart = new Vector3(lightningStart.x, lightningStart.y +  lightningHeight, lightningStart.z);
		currentLightning = Instantiate(LightningPrefab , lightningStart, Quaternion.identity) as GameObject;
 	}


	void IceWallUpdate()
	{			
		RaycastHit hit;
		Ray ray ;
		if (Input.GetMouseButtonDown(0))
		{
			if ((mana - wallInitialManaCost) > minMana)
			{
				ChangeMana(wallInitialManaCost);
				  ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					IceWallStarted = true;
					IceWallStart = hit.point;
					ActivateHelp(iceWallHelpMat , hit.point);
				}

			}
			else
			{
				NotEnoughMana();
			}
 		}



		  ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
	
			if (Physics.Raycast(ray, out hit))
			{
				helpLine.SetPosition(1,hit.point);
			
				float dist = Vector3.Distance(FireBallStart, hit.point);
				float t = Mathf.Clamp01(dist / 500);
				helpWidth = Mathf.Lerp(minHelpWidth, maxHelpWidth , t ); 
				helpLine.SetWidth( helpWidth , helpWidth );
					
				if (Input.GetMouseButtonUp(0) && IceWallStarted == true)
				{
					IceWallEnd = hit.point;
					CreateIceWall();
					helpLine.enabled = false;	
					IceWallStarted = false;
				}	
		}

	}

	void CreateIceWall()
	{
		IceWallEnd = new Vector3(IceWallEnd.x, IceWallStart.y -  IceWallPrefab.transform.localScale.y *0.5f , IceWallEnd.z);
		IceWallStart = new Vector3(IceWallStart.x, IceWallStart.y -  IceWallPrefab.transform.localScale.y*0.5f, IceWallStart.z);

		Vector3 midPoint = IceWallStart + (IceWallEnd - IceWallStart) * 0.5f;

		Vector3 dir = (IceWallEnd - IceWallStart);
		dir.Normalize();

		float distance = Vector3.Distance(IceWallEnd, IceWallStart);

		float manaCost =   distance * wallLengthManCost;
		if ((mana - manaCost) > minWallLength)
		{
			ChangeMana(manaCost);

			GameObject wall = Instantiate(IceWallPrefab, midPoint, Quaternion.identity) as GameObject;

			wall.transform.right = dir;

			if (distance < minWallLength)
			{
				distance = minWallLength;
			}


			wall.transform.localScale = new Vector3(distance, IceWallPrefab.transform.localScale.y, IceWallPrefab.transform.localScale.z);
		}
		else
		{
			NotEnoughMana();
		}
	}

	void ActivateHelp(Material mat, Vector3 pos)
	{
		helpLine.enabled = true;
		helpLine.SetPosition(0,pos);
		helpLine.SetPosition(1,pos);
		helpLine.renderer.material = mat;
		helpLine.SetWidth( helpWidth , helpWidth );
		helpWidth = minHelpWidth;
	}
	void FireBallUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if((mana - fireBallInitalManaCost) > minMana)
			{
				ChangeMana(fireBallInitalManaCost);

				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					chargeFireBall = true;
					FireBallStart = hit.point;
					SpawnFireBall();

					ActivateHelp(fireBallHelpMat, hit.point) ;

 				}
			} 
			else
			{
				NotEnoughMana();
			}
		}

		if (chargeFireBall == true)
		{
			bool endNoMana = false;
			if ((mana - fireBallManaCost) > minMana)
			{
				ChangeMana(fireBallManaCost * Time.deltaTime);
			}
			else
			{
				endNoMana = true;
			}

 
				GrowFireBall();
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					FireBallEnd = hit.point;
					helpLine.SetPosition(1,hit.point);

					float dist = Vector3.Distance(FireBallStart, hit.point);
					float t = Mathf.Clamp01(dist / 500);
					helpWidth = Mathf.Lerp(minHelpWidth, maxHelpWidth , t ); 
					helpLine.SetWidth( helpWidth , helpWidth );

				}

				if (Input.GetMouseButtonUp(0) || endNoMana == true)
				{
					chargeFireBall = false;
					helpLine.enabled = false;
					LaunchFireBall();

				}
			 
		}

	
	}
	
	void SpawnFireBall()
	{
 		currentFireBall = Instantiate(FireBallPrefab, FireBallStart, Quaternion.identity) as GameObject;
		fireBallMass = 1;
	}

	void GrowFireBall()
	{
		if (currentFireBall != null)
		{

			if (Time.time > (fireBallGrowthTimer + 1 / fireBallTickRate) )
			{
				fireBallGrowthTimer = Time.time;

				currentFireBall.transform.localScale += new Vector3(1,1,1) * fireBallGrowthPerTick;
				fireBallMass += fireBallMasPerTick;

				currentFireBall.transform.position = new Vector3(FireBallStart.x, FireBallStart.y + currentFireBall.transform.localScale.y*0.5f, FireBallStart.z);

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

		currentFireBall.GetComponent<FireBall>().LaunchSetup();

		currentFireBall = null;
		
		fireBallMass = 1;
	}


	void ChangeMana(float amount )
	{
		mana -= amount;

		mana = Mathf.Clamp(mana, minMana, maxMana);

		ManaStatBarScript.SetStat(mana / maxMana);

	}

	void NotEnoughMana()
	{

		Debug.Log("not Enough Mana");

	}
}
