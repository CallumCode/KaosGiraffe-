using UnityEngine;
using System.Collections;

public class SwarmVision : MonoBehaviour 
{

	public ArrayList swarmInSight;

	public float count = 0;
	// Use this for initialization
	void Start () 
	{
		swarmInSight = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () 
	{
		count = swarmInSight.Count;
		swarmInSight.Clear();
	}

	void OnTriggerStay(Collider other)
	{

		if (other.CompareTag("Swarm") )
		{
			swarmInSight.Add(other.gameObject);
		}
	}

}
