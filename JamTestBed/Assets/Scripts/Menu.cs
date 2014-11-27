using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {


	public enum LastGameSate{none, win , lose};
	public LastGameSate lastGameSate = LastGameSate.none;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		switch(lastGameSate)
		{
		case LastGameSate.win:
			{
			GUI.Label( new Rect (20,40,200,20), "You saved your tower");
			}
		break;
			
		case LastGameSate.lose:
			{
				GUI.Label( new Rect (20,40,200,20), "Your tower was destroyed" );
			}
			break;
		}


		if (GUI.Button ( new Rect (20,100,80,20), "Play Game")) 
		{
			Application.LoadLevel ("Game");
		}

	}

}
