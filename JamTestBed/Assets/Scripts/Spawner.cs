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


			Vector3 pos = PointOnCircle(radius, Random.Range(0,360), TowerObject.transform.position);

			GameObject type = GetEnemyType();
			pos = new Vector3(pos.x, type.transform.localScale.y , pos.z);

			GameObject enemy = Instantiate(type, pos, Quaternion.identity) as GameObject;
			enemy.GetComponent<Enemy>().SetUp(TowerObject);
			enemy.transform.parent = transform;
		}
	}



	GameObject GetEnemyType()
	{
		GameObject type = null;
		float rand = Random.value;

		//if (rand > 0.2) type = EnemyPrefab;
		//else
			type = SwarmPrefab;

		return type;
	}

	Vector3 PointOnCircle(float radius, float angleInDegrees, Vector3 origin)
	{
 		float x = (float)(radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180F)) + origin.x;
		float z = (float)(radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180F)) + origin.z;

		return new Vector3(x,0,z);
	}
}
