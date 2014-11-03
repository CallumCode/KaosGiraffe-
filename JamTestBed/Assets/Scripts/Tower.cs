using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	public float health = 100;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void TakeDamage(float amount)
	{
		health -= amount;

		health = Mathf.Clamp(health, 0, 100);

		if(health <= 0)
		{
			LoseGame();
		}

	}


	void LoseGame()
	{
		Debug.Log("GameLost");
		Debug.Break();
	}
}
