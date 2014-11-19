using UnityEngine;
using System.Collections;

public class WaveMechanic : MonoBehaviour 
{
 	public GameObject TreeManPrefab;
	public GameObject SwarmHeadPrefab;

	public int numOfWaves = 10;
	ArrayList waveList;

	public float timeBetweenWaves = 5;

	public float waveEnemyScaler = 2; // will be linear atm

	public GameObject TowerObject;

	public float radius = 10;


	ArrayList currentWave = null;
	AttackInfo currentAttack = null;
	private float lastAttackTime = 0;
	
	private float lastWaveTime = 0;

	private int waveCount = 0;
	// Use this for initialization
	void Start ()
	{
		CreateAllWaves();

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (currentAttack == null)
		{
			currentAttack = GetNextAttack();
		}
		else
		{
			if (Time.time > (lastAttackTime + currentAttack.time))
			{
				SpawnAttack(currentAttack);
			}
		}
	}

	void CreateAllWaves()
	{
		waveList = new ArrayList();

		for(int i = 0; i < numOfWaves; i++)
		{

			waveList.Add(CreateAWave(i));
		}
	}


	ArrayList CreateAWave(int num)
	{
		int numInThisWave = Mathf.RoundToInt(  (1 + num) * waveEnemyScaler);

		ArrayList wave = new ArrayList();

		for(int i = 0; i  < numInThisWave; i++)
		{
			AttackInfo attack = new AttackInfo();

			wave.Add(attack);
		}

		return wave;
	}

	void SpawnAttack(AttackInfo attack)
	{
		if (TowerObject != null && attack != null && TreeManPrefab != null && SwarmHeadPrefab != null && attack != null)
		{


			switch(attack.attackType)
			{
				case AttackInfo.AttackType.Swarm:
				{
					SpawnSwarmHead(attack);
				}
				break;

				case AttackInfo.AttackType.Treeman:
				{
					SpawnTreeMan(attack);
				}
				break;
			}

			currentAttack = null;
			lastAttackTime = Time.time;

 		}
		else
		{
			Debug.Log(" wave SpawnAttack null");
			Debug.Break();
		}
		
	}

	void SpawnSwarmHead(AttackInfo attack)
	{
 
		float x = Mathf.Cos(attack.startAngle*Mathf.Deg2Rad) * radius;
		float y = SwarmHeadPrefab.transform.position.y;
		float z = Mathf.Sin(attack.startAngle * Mathf.Deg2Rad) * radius;
		Vector3 pos = new Vector3(x, y ,z );

 
		GameObject head = Instantiate(SwarmHeadPrefab, pos, Quaternion.identity) as GameObject;
		head.GetComponent<SwarmHead>().SetUp(TowerObject);
		head.transform.parent = transform;

	}

	void SpawnTreeMan(AttackInfo attack)
	{
		float x = Mathf.Cos(attack.startAngle * Mathf.Deg2Rad) * radius;
		float y = SwarmHeadPrefab.transform.position.y;
		float z = Mathf.Sin(attack.startAngle * Mathf.Deg2Rad) * radius;
		Vector3 pos = new Vector3(x, y, z);


		GameObject TreeMan = Instantiate(TreeManPrefab, pos, Quaternion.identity) as GameObject;
		TreeMan.GetComponent<Enemy>().SetUp(TowerObject);
		TreeMan.transform.parent = transform;
	}


	AttackInfo GetNextAttack()
	{
		AttackInfo attackInfo = null;


		if (currentWave == null || currentWave.Count == 0)
		{
			if (currentWave != null  && currentWave.Count == 0)
			{
				currentWave = null;
				lastWaveTime = Time.time;
			}

			if(Time.time > (timeBetweenWaves + lastWaveTime))
			{
				currentWave = GetNextWave();
			}
		}
		else
		{
			attackInfo = (AttackInfo)currentWave[0];
			currentWave.RemoveAt(0);
 
		}

		return attackInfo;
	}

	ArrayList GetNextWave()
	{
		ArrayList nextWave = null;

		if (waveList != null && waveList.Count > 0)
		{
			nextWave = (ArrayList)waveList[0];
			waveList.RemoveAt(0);
			
			waveCount++;
			Debug.Log("Wave " + waveCount);
		}

		return nextWave;
	}

}
