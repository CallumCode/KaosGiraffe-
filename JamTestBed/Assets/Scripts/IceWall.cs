using UnityEngine;
using System.Collections;

public class IceWall : MonoBehaviour
{
	public float raiseSpeed = 1;
	public float idleTime = 5;
	private float idleStartTime = 0;

	// Use this for initialization

	public enum IceWallStageType {growing,idle, shrinking, dead};
	public IceWallStageType iceWallStage = IceWallStageType.growing;
	void Start ()
	{

 	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateStages();
	}

	void UpdateStages()
	{

		switch (iceWallStage)
		{
			case IceWallStageType.growing:
				{
					Growing();
				}
				break;
			case IceWallStageType.idle:
				{
					Idle();
				}
				break;
			case IceWallStageType.shrinking:
				{
					Shrinking();
				}
				break;
			case IceWallStageType.dead:
				{
					Dead();
				}
				break;

		}
	
	}

	void Idle()
	{
		if (Time.time > (idleTime + idleStartTime))
		{
			iceWallStage = IceWallStageType.shrinking;	
		}
	}

	void Growing()
	{
		transform.Translate(Vector3.up * raiseSpeed * Time.deltaTime);
	
		if (transform.position.y > (transform.localScale.y*0.5))
		{
			iceWallStage = IceWallStageType.idle;
			idleStartTime = Time.time;
		}
	}

	void Shrinking()
	{
		transform.Translate(-Vector3.up * raiseSpeed * Time.deltaTime);

		if (transform.position.y < (-transform.localScale.y * 0.5))
		{
			iceWallStage = IceWallStageType.dead;			
		}
	}


	void Dead()
	{
		Destroy(gameObject);
	}
}
