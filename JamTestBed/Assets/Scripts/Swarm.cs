using UnityEngine;
using System.Collections;

public class Swarm : Enemy {

	public	GameObject visionGameObject;
	SwarmVision swarmVisionScript;


	float avoidTimeTotal;
	float avoidTimeStep;
	float aovidSpeedMax;


	float avgPosSpeed;

	float avgDirSpeed;

	// Use this for initialization
	void Start () 
	{
		swarmVisionScript = visionGameObject.GetComponent<SwarmVision>();

		turnSpeed = 0.8f;
		moveSpeed = 10;
		damage = 0.1f;
		attacksRate = 3;

		avoidTimeTotal = 0.5f;
		avoidTimeStep = 0.1f;
		aovidSpeedMax = 0.2f;	
	
		
		avgPosSpeed = 1;
		avgDirSpeed = 1;

	}
	
	// Update is called once per frame
	void Update ()
	{
		Movement();

		Flock(); 
	}

  

	void Flock()
	{
		if (swarmVisionScript == null) return;
		if (swarmVisionScript.swarmInSight == null) return;
		if (swarmVisionScript.swarmInSight.Count == 0) return;


		Vector3 totalDir = Vector3.zero;
		Vector3 totalPos = Vector3.zero;

		// for each swarm in sight
		for (int i = 0; i < swarmVisionScript.swarmInSight.Count; i++)
		{
			GameObject obj = (GameObject)swarmVisionScript.swarmInSight[i];

			totalDir += obj.transform.forward;
			totalPos += obj.transform.position;

			HandleAvoid(obj);
		}

		Vector3 avgPos =  totalPos * (1 / (float) (swarmVisionScript.swarmInSight.Count));

		Vector3 avgPosDir = theTransform.position-avgPos;
		avgPosDir.Normalize();

		Debug.DrawLine(theTransform.position, avgPos, Color.magenta);
		characterCont.Move(avgPosDir * avgPosSpeed * Time.deltaTime);



		Vector3 avgDir = totalDir * (1 / (float) (swarmVisionScript.swarmInSight.Count));

		Debug.DrawRay(theTransform.position, avgDir * avgDirSpeed, Color.green);
		characterCont.Move(avgDir * avgDirSpeed * Time.deltaTime);
	
	}

	void HandleAvoid(GameObject obj)
	{

		Vector3 thisPos = Vector3.zero;
		Vector3 otherPos = Vector3.zero;

		// predict collision
		for (float k = 0; k < avoidTimeTotal; k += avoidTimeStep)
		{
			// we use same moveSpeed as they are same type of enemy;
			thisPos = theTransform.position + theTransform.forward * k * moveSpeed;
			otherPos = obj.transform.position + obj.transform.forward * k * moveSpeed;

			if ((theTransform.localScale.x) > Vector3.Distance(thisPos, otherPos))
			{
				// colision is predicted

				Vector3 dir = thisPos - otherPos;
				dir.Normalize();

				// lerp means will have a max avoid speed when the colission is least num of steps
				float t = k / avoidTimeTotal;
				t = Mathf.Clamp01(t);

				float speed = Mathf.Lerp(aovidSpeedMax, 0, t);

				Debug.DrawRay(theTransform.position, dir * speed, Color.red);

				characterCont.Move(dir * speed * Time.deltaTime);

			}

		}

	}
}
