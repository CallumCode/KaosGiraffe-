using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject EnemyPrefab;
	public GameObject TreeManPrefab;

	public GameObject SwarmPrefab;

	public GameObject TowerObject;

	public float spawnRate = 10;
	public float radius = 10;
	private float spawnTimer = 0;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Time.time > (spawnTimer + 1 / spawnRate))
		{
			spawnTimer = Time.time;
			SpawnEnemy();		
		}
	}

	void SpawnEnemy()
	{
		if (EnemyPrefab != null && TowerObject != null)
		{




				GameObject type = null;
		float rand = Random.value;

			if (rand > 0.5)
			{
				SpawnEnenmy();
			}
			else if (rand > 0.2)
			{
				SpawnSwarm();
			}

			else 
			{
				SpawnTreeMan();
			}
 		
		}
	}

	void SpawnEnenmy()
	{
		Vector3 pos = PointOnCircle(radius, Random.Range(0, 360), TowerObject.transform.position);
		pos = new Vector3(pos.x,EnemyPrefab.transform.localScale.y, pos.z);

		GameObject enemy = Instantiate(EnemyPrefab, pos, Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy>().SetUp(TowerObject);
		enemy.transform.parent = transform;
	}

	void SpawnTreeMan()
	{
		Vector3 pos = PointOnCircle(radius, Random.Range(0, 360), TowerObject.transform.position);
		pos = new Vector3(pos.x, TreeManPrefab.transform.localScale.y, pos.z);

		GameObject enemy = Instantiate(TreeManPrefab, pos, Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy>().SetUp(TowerObject);
		enemy.transform.parent = transform;
	}


	void  SpawnSwarm()
	{ 
			Vector3 pos = PointOnCircle(radius, Random.Range(0,360), TowerObject.transform.position);

			pos = new Vector3(pos.x, SwarmPrefab.transform.localScale.y , pos.z);

		const int num = 5;
		for (int i = 0; i < num; i ++ )
		{
			GameObject enemy = Instantiate(SwarmPrefab, pos  + transform.right*i , Quaternion.identity) as GameObject;
			enemy.GetComponent<Enemy>().SetUp(TowerObject);
			enemy.transform.parent = transform;

		}
		
	}
	 
	Vector3 PointOnCircle(float radius, float angleInDegrees, Vector3 origin)
	{
 		float x = (float)(radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180F)) + origin.x;
		float z = (float)(radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180F)) + origin.z;

		return new Vector3(x,0,z);
	}
}
