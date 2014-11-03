using UnityEngine;
using System.Collections;

public class Treeman : Enemy 
{

	// Use this for initialization
	void Start () 
	{
		moveSpeed = 1;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Movement();
	}
}
