using UnityEngine;
using System.Collections;

public class StatBar : MonoBehaviour
{
 
 
	float minScale = 0.01f;
	float maxScale = 0.25f;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	


	
	}
	
	public void SetStat(float stat)
	{
		float t = Mathf.Clamp01(stat);
		float x = Mathf.Lerp(minScale, maxScale, t);
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

	}

}
