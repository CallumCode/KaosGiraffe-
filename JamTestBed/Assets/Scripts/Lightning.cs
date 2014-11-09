using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{

	public GameObject CloudPrefab = null;

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

				}
				break;
			case LightningStageType.shrinking:
				{

				}
				break;
			case LightningStageType.dead:
				{

				}
				break;

		}

	}

	void GrowingUpdate()
	{

	}


	void CreateClouds()
	{

		if (CloudPrefab != null) 
		{
			float numParentCloud = Random.Range(minFirstSubClouds ,maxFirstSubClouds);
			for (float parentCloud = 0; parentCloud < numParentCloud; parentCloud++)
			{
				GameObject cloud = Instantiate(CloudPrefab, transform.position, transform.rotation) as GameObject;
				cloud.transform.parent = transform;
				cloud.transform.localScale *= firstRadius;

				AddSubClouds(cloud.transform.position, firstRadius, Random.Range(minSubCloudDepth,maxSubCloudDepth));

			}

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

			cloud.transform.parent = transform;
			cloud.transform.localScale *= Mathf.Clamp(newRadius, 0.01f , firstRadius);

			AddSubClouds(pos, newRadius, deapth);
		}
	}

	public void FinishGrowing()
	{
		lightningStage = LightningStageType.striking;
	}
}
