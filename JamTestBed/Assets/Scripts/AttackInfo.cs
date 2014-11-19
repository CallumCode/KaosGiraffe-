using UnityEngine;
using System.Collections;

public class AttackInfo
{
	public float time;
	public enum AttackType { Swarm, Treeman };
	public AttackType attackType;


	public float minTime = 0.1f;
	public float maxTime = 1.0f;


	public float startAngle;

	public AttackInfo()
	{

		RandomIinit();
	}


	void RandomIinit()
	{
		time = Random.Range(minTime, maxTime);

		float r = Random.Range(1, 100);

		if (r > 40)
		{
			attackType = AttackType.Swarm;
		}
		else
		{
			attackType = AttackType.Treeman;
		}


		startAngle = Random.Range(0.0f, 360.0f);

	}


}
