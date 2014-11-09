using UnityEngine;
using System.Collections;

public class Swarm : Enemy {

 	SwarmHead swarmHeadScript;


	float avoidTimeTotal;
	float avoidTimeStep;
	float aovidSpeedMax;


	float avgPosSpeedMax;

	float avgDirSpeedMax;

	float maxDist;

	float radiusScaler;

	// Use this for initialization
	void Start () 
	{
		swarmHeadScript = transform.parent.GetComponent<SwarmHead>();

		turnSpeed = 0.8f;
		moveSpeed = 10;
		damage = 0.1f;
		attacksRate = 3;

		avoidTimeTotal = 1.0f;
		avoidTimeStep = 0.1f;
		
		aovidSpeedMax = 15.0f;				
		avgPosSpeedMax = 5;
		avgDirSpeedMax = 1;

		maxDist = 500;

		radiusScaler = 2.0f;

		armourScaler = 0.5f;

	}
	
	// Update is called once per frame
	void Update ()
	{
		Movement();

		Flock(); 
	}

  

	void Flock()
	{
		if (swarmHeadScript == null) return;
		if (swarmHeadScript.swarmList == null) return;
		if (swarmHeadScript.swarmList.Count == 0) return;


		Vector3 totalDir = Vector3.zero;
		Vector3 totalPos = Vector3.zero;

		// for each swarm in sight
		for (int i = 0; i < swarmHeadScript.swarmList.Count; i++)
		{

			GameObject obj = (GameObject)swarmHeadScript.swarmList[i];
			if (obj != null)
			{
				totalDir += obj.transform.forward;
				totalPos += obj.transform.position;

				HandleAvoid(obj);
			}
			else
			{
				swarmHeadScript.swarmList.RemoveAt(i);
			}
		}

		Vector3 avgPos =  totalPos * (1 / (float) (swarmHeadScript.swarmList.Count));

		Vector3 avgPosDir = avgPos - theTransform.position;
		avgPosDir.Normalize();

		Debug.DrawLine(theTransform.position, avgPos, Color.magenta);

		
		float t = Vector3.Distance(theTransform.position , avgPos ) / maxDist;
		t = Mathf.Clamp01(t);

		float avgPosSpeed = Mathf.Lerp(0, avgPosSpeedMax,t);
		characterCont.Move(avgPosDir * avgPosSpeed * Time.deltaTime);


		Vector3 avgDir = totalDir * (1 / (float) (swarmHeadScript.swarmList.Count));
		t = Vector3.Dot(avgDir, transform.forward);
		t = Mathf.Abs(t);
		t = Mathf.Clamp01(t);
		 
		float avgDirSpeed = Mathf.Lerp(0,avgDirSpeedMax,t);

		Debug.DrawRay(theTransform.position, avgDir * avgDirSpeed, Color.green);
		characterCont.Move(avgDir * avgDirSpeed* Time.deltaTime);
	
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

			if ((theTransform.localScale.x*radiusScaler) > Vector3.Distance(thisPos, otherPos))
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


				// exit early as only want to add one avoid per collision 
				return;

			}

		}

	}
}
