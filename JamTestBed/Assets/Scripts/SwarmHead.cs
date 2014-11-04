using UnityEngine;
using System.Collections;

public class SwarmHead : MonoBehaviour 
{

	public GameObject SwarmPrefab;


	public ArrayList swarmList;

	GameObject TowerObject;
	public float count = 5;
	// Use this for initialization
	void Start () 
	{
		swarmList = new ArrayList();
		SpawnSwarm();

	}
	
	// Update is called once per frame
	void Update () 
	{

 	}


	public void SetUp(GameObject inTowerObject)
	{
		TowerObject = inTowerObject;
	}

	void  SpawnSwarm()
	{ 
 
 
 		for (int i = 0; i < count; i ++ )
		{
			GameObject enemy = Instantiate(SwarmPrefab, transform.position  + transform.right*i , Quaternion.identity) as GameObject;
			enemy.GetComponent<Enemy>().SetUp(TowerObject);
			enemy.transform.parent = transform;

			swarmList.Add(enemy);
		}
		
	}
}
