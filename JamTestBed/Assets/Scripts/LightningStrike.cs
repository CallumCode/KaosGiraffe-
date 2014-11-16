using UnityEngine;
using System.Collections;

public class LightningStrike : MonoBehaviour
{
	public GameObject StrikePrefab;

	LineRenderer lineRenderer;

	public int maxNumVerts = 10;

	  int currentNumVerts = 10;

	public float maxDepth = 3;
	public float currentdepth = 3;

	public Material lightningMat;

	public float width = 0.2f;

	public float maxRadius = 10;

	public float strikeRate = 1;
	
	private float strikeTimer = 0;


	public GameObject StrikeDamage;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		
		if (Time.time > (strikeTimer + 1 / strikeRate) )
		{
			strikeTimer = Time.time;

			Vector3 posStart = transform.position;
			if (transform.parent != null)
			{
				posStart = transform.parent.transform.position;
			}

			Ray ray = new Ray(posStart, -Vector3.up);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				float dist = Vector3.Distance(transform.position, hit.point);

				ResetPos(dist);

			}
		}
 	}


 
	public void SetUpLightningStrike(float depth )
	{
		Vector3 posStart = transform.position;
		if (transform.parent != null)
		{
			posStart = transform.parent.transform.position;
		}
		Ray ray = new Ray(posStart, -Vector3.up);
		RaycastHit hit;

 
		if (Physics.Raycast(ray, out hit))
		{
			strikeTimer = Time.time; 
		//	float temp = (depth / maxDepth) * maxNumVerts;
			//currentNumVerts = Mathf.RoundToInt(temp);
			//currentdepth--;
			
		//	Debug.Log(currentNumVerts);

			gameObject.AddComponent<LineRenderer>();
			lineRenderer = GetComponent<LineRenderer>();

			lineRenderer.SetVertexCount(currentNumVerts);
			lineRenderer.renderer.material = lightningMat;
			lineRenderer.SetWidth(width, width);


			float dist = Vector3.Distance(transform.position, hit.point);

			ResetPos(dist);
		}
		else
		{
			Debug.Log(" no target for lighting strike");

		}
	}


	void ResetPos(float dist )
	{
		for (float vert = (currentNumVerts - 1); vert >= 0; vert--)
		{

			float t = (vert / currentNumVerts);
			t = Mathf.Clamp01(t);

			Vector2 randMove = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1) * maxRadius * (vert / currentNumVerts);
			Vector3 pos = transform.position + Vector3.down * dist * t;
			pos += new Vector3(randMove.x, 0, randMove.y);
 			lineRenderer.SetPosition((int)vert, pos);
 
			if(vert == (currentNumVerts-1))
			{
				StrikeDamage.transform.position = pos;

			}
		}
	}

	void CreateSubForks(Vector3 pos)
 	{
 			GameObject strike = Instantiate(StrikePrefab, pos , transform.rotation) as GameObject;
			LightningStrike strikeScript = strike.GetComponent<LightningStrike>();
			strikeScript.SetUpLightningStrike(currentdepth);

			strike.transform.parent = transform;
 

	}
}
