using UnityEngine;
using System.Collections;

public class Treeman : Enemy 
{

	// Use this for initialization
	void Start () 
	{
		damage = 10;
		attacksRate = 2;
		moveSpeed = 2; 
		turnSpeed = 0.5f;
		armourScaler = 0.1f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Movement();
	}
}
