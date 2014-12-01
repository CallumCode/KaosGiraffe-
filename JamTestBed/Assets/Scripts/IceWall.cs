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


	private Vector3 startPos = Vector3.zero;
	void Start ()
	{
		startPos = transform.position;
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
	
		if (transform.position.y > (startPos.y + transform.localScale.y  ))
		{
			iceWallStage = IceWallStageType.idle;
			idleStartTime = Time.time;
		}
	}

	void Shrinking()
	{
		transform.Translate(-Vector3.up * raiseSpeed * Time.deltaTime);

		if (transform.position.y > (startPos.y  + transform.localScale.y))
		{
			iceWallStage = IceWallStageType.dead;			
		}
	}


	void Dead()
	{
		Destroy(gameObject);
	}
}
