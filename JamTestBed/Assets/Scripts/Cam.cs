using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {

    public float speed = 1;
    public float camSens = 0.25f;
	public float zoomSpeed = 1;
	public float moveToDist = 25;

    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

	private bool moveToTarget = false;
	private Transform target; 

	// Use this for initialization
	void Start ()
	{
 
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleleLook();
        HandleMove();
     
		HandleMoveTo();
	}

    private void HandleleLook()
    {  
        if (Input.GetMouseButtonDown(1))
        {
            lastMouse = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            lastMouse = Input.mousePosition - lastMouse;

            lastMouse = new  Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);

            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);

            transform.eulerAngles = lastMouse;

            lastMouse = Input.mousePosition;
        }

    }
    private void HandleMove()
    {
        Vector3 velocity = GetBaseInput();
        velocity *= speed;

        transform.Translate(velocity * Time.deltaTime);
    }
	
	
    private void HandleMoveTo()
    {
		if(moveToTarget)
		{
			transform.LookAt(target);

			if (Vector3.Distance(transform.position, target.position) > moveToDist)
			{
				transform.Translate(zoomSpeed * Time.deltaTime * transform.forward);
			} 
		}
		
	}

    //returns the basic values, if it's 0 than it's not active.
    private Vector3 GetBaseInput() 
    {        
        Vector3 velocity  = Vector3.zero;

        if (Input.GetKey (KeyCode.W))
        {
            velocity += new Vector3(0, 0 , 1);
        }

        if (Input.GetKey (KeyCode.S))
        {
            velocity += new Vector3(0, 0 , -1);
        }

        if (Input.GetKey (KeyCode.A))
        {
            velocity += new Vector3(-1, 0 , 0);
        }

        if (Input.GetKey (KeyCode.D))
        {
            velocity += new Vector3(1, 0 , 0);
        }

        return velocity;
    }

	public void StopMoveTo()
	{
		moveToTarget = false;
	}

	public void MoveToPostion(Transform inTarget)
	{
		moveToTarget = true;
		target = inTarget;
	}
}
