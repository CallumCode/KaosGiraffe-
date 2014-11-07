using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour 
{
	private float shieldAplha = 0;
	public float shieldAplhaRate = 1; 

	// Use this for initialization
	void Start () 
	{
		// dont colide with ground
		Physics.IgnoreLayerCollision(10, 8);
	}
	
	// Update is called once per frame
	void Update () 
	{
		shieldAplha -= shieldAplhaRate * Time.deltaTime;
		shieldAplha = Mathf.Clamp01(shieldAplha);
		renderer.material.SetFloat("_shieldAplha", shieldAplha);
	}

	void ShieldHit(ContactPoint contact)
	{ 
 		shieldAplha = 1;
		renderer.material.SetFloat("_diameter", transform.localScale.x * 2);

		Vector4 localPoint = transform.InverseTransformPoint(contact.point);

		localPoint.Scale(transform.localScale);
		localPoint.w = 1;
		renderer.material.SetVector("_hitPoint", localPoint);
   	}


	void OnCollisionEnter(Collision collision)
	{
  
		foreach (ContactPoint contact in collision.contacts)
		{
			
 			Debug.DrawRay(contact.point, contact.normal, Color.white);
	 
				Debug.Log("HIT");

				ShieldHit(contact);
 
		}
	}
}
