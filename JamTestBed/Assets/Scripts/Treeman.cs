﻿using UnityEngine;
using System.Collections;

public class Treeman : Enemy 
{

	// Use this for initialization
	void Start () 
	{
		damage = 5;
		attacksRate = 1;
		moveSpeed = 5; 
		turnSpeed = 0.75f;
		armourScaler = 0.1f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Movement();
	}
}
	