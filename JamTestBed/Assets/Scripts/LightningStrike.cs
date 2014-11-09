using UnityEngine;
using System.Collections;

public class LightningStrike : MonoBehaviour
{

	LineRenderer lineRenderer;


	public int numVerts = 10;
	public Material lightningMat;

	public float width = 0.2f;

	public float maxRadius = 10;

	public float strikeRate = 1;
	
	private float strikeTimer = 0;

	// Use this for initialization
	void Start()
	{
		SetUpLightningStrike();
	}

	// Update is called once per frame
	void Update()
	{

		if (Time.time > (strikeTimer + 1 / strikeRate) )
		{
			strikeTimer = Time.time;
			Ray ray = new Ray(transform.position, -Vector3.up);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				float dist = Vector3.Distance(transform.position, hit.point);

				ResetPos(dist);
			}
		}
 	}


 
	void SetUpLightningStrike()
	{
		Ray ray = new Ray(transform.position, -Vector3.up);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			strikeTimer = Time.time;

			gameObject.AddComponent<LineRenderer>();
			lineRenderer = GetComponent<LineRenderer>();

			lineRenderer.SetVertexCount(numVerts);
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
		for (float vert = (numVerts - 1); vert >= 0; vert--)
		{

			float t = (vert / numVerts);
			t = Mathf.Clamp01(t);

			Vector2 randMove = Random.insideUnitCircle * maxRadius * (vert / numVerts);
			Vector3 pos = transform.position + Vector3.down * dist * t;
			pos += new Vector3(randMove.x, 0, randMove.y);

			lineRenderer.SetPosition((int)vert, pos);
		}
	}
}
