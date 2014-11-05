using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour 
{
	private float shieldAplha = 0;
	public float shieldAplhaRate = 1;

	public float hitAlphaLoss = 0.2f;
	public float shieldHitCount = 0;
	public float maxShieldHitCount = 1;

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
		Quaternion quatHit = Quaternion.FromToRotation(Vector3.up, contact.normal);
 
 		shieldAplha = 1;
		renderer.material.SetFloat("_diameter", transform.localScale.x * 2);
		Vector4 localPoint = transform.InverseTransformPoint(contact.point);
		localPoint.Scale(transform.localScale);
		localPoint.w = 1;
		renderer.material.SetVector("_hitPoint", localPoint);


  	}


	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("HIT");
		float newAlpha = renderer.material.color.a - hitAlphaLoss;
		if (newAlpha < 0.2f) newAlpha = 0.2f;

		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, newAlpha);

		foreach (ContactPoint contact in collision.contacts)
		{
			
 			Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (shieldHitCount <= maxShieldHitCount)
			{
				ShieldHit(contact);
			}

		}
	}
}
