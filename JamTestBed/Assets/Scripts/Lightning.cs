using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{

	public GameObject CloudPrefab = null;
	public GameObject StrikePrefab = null;


	enum LightningStageType { growing, striking, shrinking, dead };
	LightningStageType lightningStage = LightningStageType.growing;



	public int maxFirstSubClouds = 10;
	public int minFirstSubClouds = 5;

	public int maxSubCloudDepth = 10;
	public int minSubCloudDepth = 5;

	public float firstRadius = 10;
	public float minSubCloudShrink = 0.25f;
	public float maxSubCloudShrink = 0.75f;
	
	public float flatenScaler = 0.75f;

	public Shader CloudShader;
	private Material cloudMat;

	public Color cloudStart = Color.white;
	public Color cloudEnd = Color.black;

	private float cloudStartTime = 0;
	public float  maxGrowingTime = 0;

	private float growingTimer = 0;
	public float growTickRate = 10;

	public float upSpeed = 1;

	private Vector3 startingScale;


	public float maxCoudScale = 10;

	private GameObject cloudContainter;


	public float rangeStrikeScaler = 1;


	public int  maxStrikes = 5;
	private float sizeMod = 0;


	public float maxTimeStriking = 50;
	private float timeStartStrike  = 0;
	private float shrinkStartTime = 0;
	public float maxShrinkTime = 5;

	// Use this for initialization
	void Start()
	{
		CreateClouds();
	}

	// Update is called once per frame
	void Update()
	{

		switch (lightningStage)
		{
			case LightningStageType.growing:
				{
					GrowingUpdate();
				}
				break;
			case LightningStageType.striking:
				{
					StrikingUpdate();
				}
				break;
			case LightningStageType.shrinking:
				{
					ShrinkingUpdate();
				}
				break;
			case LightningStageType.dead:
				{
					DestroySelf();
				}
				break;

		}

	}

	void DestroySelf()
	{
//		Debug.Log("Destroy cloud");
		Destroy(gameObject);
	}


	void StrikingUpdate()
	{

		if(Time.time > ( timeStartStrike + maxTimeStriking*sizeMod))
		{
			lightningStage = LightningStageType.shrinking;
			shrinkStartTime =  Time.time;
		}
	}

	void GrowingUpdate()
	{		
		sizeMod = (Time.time  -  cloudStartTime ) /  maxGrowingTime ;
		sizeMod = Mathf.Clamp01(sizeMod);
		 
		cloudMat.color = Color.Lerp(cloudStart, cloudEnd , sizeMod);
		
		transform.Translate(Vector3.up*Time.deltaTime * upSpeed);
		
		if (Time.time > (growingTimer + 1 / growTickRate))
		{
			growingTimer = Time.time;
			transform.localScale = startingScale + startingScale * maxCoudScale * sizeMod;
 		}	

	}

	void ShrinkingUpdate()
	{

		float t = (Time.time  -  shrinkStartTime ) /  (maxShrinkTime*sizeMod);
  
		//cloudMat.color = Color.Lerp(cloudStart, cloudEnd , t);

		if(t > 0)
		{
			transform.Translate(-Vector3.up*Time.deltaTime * upSpeed);
		}

		if(t  >= 1 )
		{
			lightningStage = LightningStageType.dead;
			return;
		}
			
		
		if (Time.time > (growingTimer + 1 / growTickRate))
		{
			growingTimer = Time.time;
			transform.localScale = Vector3.Lerp(startingScale , Vector3.zero , t);
//			Debug.Log(t);	
		}	

	}
 

	void CreateClouds()
	{
		if (CloudPrefab != null) 
		{

			cloudContainter = new GameObject();
			cloudContainter.name = "cloudContainter";
			cloudContainter.transform.parent = transform;


			cloudMat = new Material(CloudShader);
			cloudMat.color = cloudStart;
		
			float numParentCloud =  Random.Range(minFirstSubClouds ,maxFirstSubClouds);

			GameObject cloud = Instantiate(CloudPrefab, transform.position, transform.rotation) as GameObject;
			cloud.transform.parent = cloudContainter.transform;
			cloud.transform.localScale *= firstRadius;
			cloud.renderer.material = cloudMat;
			cloud.name = "StartCloud";


			for (float parentCloud = 0; parentCloud < numParentCloud; parentCloud++)
			{
				AddSubClouds(cloud.transform.position, firstRadius, Random.Range(minSubCloudDepth,maxSubCloudDepth));
			}

			cloudStartTime = Time.time;
			startingScale = transform.localScale;
		}
 
	}

	void AddSubClouds(Vector3 parentPos, float parentRadius, float deapth)
	{
		if (deapth > 0)
		{
			deapth--;

			float newRadius = Random.Range(parentRadius * minSubCloudShrink, parentRadius * maxSubCloudShrink);

			Vector3 pos = ( Random.onUnitSphere* newRadius);

			pos = new Vector3(pos.x, pos.y * flatenScaler, pos.z) + parentPos;


			GameObject cloud = Instantiate(CloudPrefab, pos, transform.rotation) as GameObject;

			cloud.transform.parent = cloudContainter.transform;
			cloud.transform.localScale *= Mathf.Clamp(newRadius, 0.01f , firstRadius);
			cloud.renderer.material = cloudMat;
			cloud.name = "SubCloud " + deapth;

			AddSubClouds(pos, newRadius, deapth);
		}
	}

	public void FinishGrowing()
	{
		lightningStage = LightningStageType.striking;

		for (int i = 0; i < (maxStrikes * sizeMod); i++)
		{
			Vector2 pos = Random.insideUnitCircle * transform.localScale.x * rangeStrikeScaler;
			GameObject strike = Instantiate(StrikePrefab, transform.position + new Vector3(pos.x, 0, pos.y), transform.rotation) as GameObject;
			LightningStrike strikeScript = strike.GetComponent<LightningStrike>();
			strikeScript.SetUpLightningStrike(strikeScript.maxDepth);

			strike.transform.parent = transform;
		}

		timeStartStrike = Time.time;
		startingScale = transform.localScale;

 	}


 
}
