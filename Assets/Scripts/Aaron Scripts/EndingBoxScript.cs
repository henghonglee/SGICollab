using UnityEngine;
using System.Collections;

public class EndingBoxScript : Photon.MonoBehaviour {
	private bool isBuilderAtEnd;
	private bool isMoverAtEnd;
	private bool isJumperAtEnd;
	private bool isViewerAtEnd;
	
	private int nextLevel;
	private bool alreadyLoading = false;
	
	public Texture aTexture;
	
	
	private string statusText = "";
	
	//This boolean tracks whether a player has reached the end.
	[HideInInspector]	
	public bool PlayersHaveReachedEnd = false;
	public bool isWaitingForNextStage = false;
	
	private GameManagerVik currGameManager = null;
	private ThirdPersonControllerNET currController = null;
	
	private int ReadyCount = 0;
	
	[RPC]
	void callReady()
	{	
		++ReadyCount;
		Debug.Log(ReadyCount);
	}
	
	
	// Use this for initialization
	void Start () {
		ReadyCount = 0;
		isWaitingForNextStage = PlayersHaveReachedEnd = false;
		
		isBuilderAtEnd = false;
		isMoverAtEnd = false;
		isJumperAtEnd = false;
		isViewerAtEnd = false;
		
		nextLevel = GameManagerVik.nextLevel;
		currGameManager = GameObject.Find("Code").GetComponent<GameManagerVik>();
		currController = GameObject.Find("Code").GetComponent<ThirdPersonControllerNET>();

		//last level check
		if (nextLevel > (Application.levelCount - 1)) 
			nextLevel = -1;
		
		//Debug.Log("nextLevel at start = "+nextLevel);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (currGameManager.level_tester_mode) return;
		if (alreadyLoading) return;
		if (PlayersHaveReachedEnd && ReadyCount >=4)
		{
			
					nextLevel += 1; 			
							//last level check
							if (nextLevel > (Application.levelCount - 1)) 
								nextLevel = -1;
							GameManagerVik.nextLevel = nextLevel;
						//		Debug.Log("nextLevel updated = "+nextLevel);
					
							alreadyLoading = true;
							
							ThirdPersonControllerNET.blockammo = 1; 
							ThirdPersonControllerNET.plankammo = 5;
					
				//		Playtomic.Log.LevelAverageMetric("Time", 0, Time.timeSinceLevelLoad);
				
				
					
							if (nextLevel > -1)
								Application.LoadLevel(nextLevel);
							else
							{
								PhotonNetwork.LeaveRoom();
							}
		}
	
	
	}
	
	
	 void OnTriggerEnter(Collider other) 
	{
			
			if (currGameManager.level_tester_mode)
			{
				PlayersHaveReachedEnd = true;
			}
			
			else
			{
  	 	   	 if(other.attachedRigidbody.name.Contains("Builder"))
				{
					isBuilderAtEnd =true;
				}
			
				if(other.attachedRigidbody.name.Contains("Jumper"))
				{
					isJumperAtEnd =true;
				}	
		
				if(other.attachedRigidbody.name.Contains("Viewer"))
				{
					isViewerAtEnd =true;		
				}
			
				if(other.attachedRigidbody.name.Contains("Mover"))
				{
					isMoverAtEnd =true;
				}
		
				if (isMoverAtEnd && isViewerAtEnd && isJumperAtEnd && isBuilderAtEnd)
					PlayersHaveReachedEnd = true;
			}
	}
		
	void OnTriggerExit(Collider other) 
	{	
       	//we do not disable localPlayerAtEnd here. 
		if(other.attachedRigidbody.name.Contains("Builder"))
		isBuilderAtEnd =false;
	   	if(other.attachedRigidbody.name.Contains("Jumper"))
		isJumperAtEnd =false;
		if(other.attachedRigidbody.name.Contains("Viewer"))
		isViewerAtEnd =false;
		if(other.attachedRigidbody.name.Contains("Mover"))
		isMoverAtEnd =false;
    }
	
	void OnGUI()
	{
		if (PlayersHaveReachedEnd)
		{	
			GUI.DrawTexture(new Rect (Screen.width *0.125f, Screen.height *0.125f, Screen.width * 0.75f, Screen.height * 0.75f), aTexture, ScaleMode.StretchToFill);
		
			//Stats here. Note: you might want to stop stat collecting for a given stage when a player first reaches the end point.	
			
			
			if (nextLevel ==  -1)
			{
				if (GUI.Button(new Rect (Screen.width *0.4f, Screen.height *0.8f, Screen.width * 0.25f, Screen.height * 0.1f), "Complete!"))
				{
					PhotonNetwork.LeaveRoom();
				}
			
			}
			else
			{
			
		
				
				if (GUI.Button(new Rect (Screen.width *0.4f, Screen.height *0.8f, Screen.width * 0.25f, Screen.height * 0.1f), "Go To Next Stage"))
				{
					
					if (currGameManager.level_tester_mode)
					{
						nextLevel += 1; 			
						//last level check
						if (nextLevel > (Application.levelCount - 1)) 
							nextLevel = -1;
			
						GameManagerVik.nextLevel = nextLevel;
			
						ThirdPersonControllerNET.blockammo = 1;
						ThirdPersonControllerNET.plankammo = 5;
						
						
						
						if (nextLevel > -1)
							Application.LoadLevel(nextLevel);
						else
						{
							PhotonNetwork.LeaveRoom();
						}
					}
					else
					{
						
						photonView.RPC("callReady",PhotonTargets.All);	
						
						/*
						if(isBuilderAtEnd && isMoverAtEnd && isJumperAtEnd && isViewerAtEnd && !alreadyLoading)
						{
							nextLevel += 1; 			
							//last level check
							if (nextLevel > (Application.levelCount - 1)) 
								nextLevel = -1;
							GameManagerVik.nextLevel = nextLevel;
						//		Debug.Log("nextLevel updated = "+nextLevel);
					
							alreadyLoading = true;
							
							ThirdPersonControllerNET.blockammo = 1; 
							ThirdPersonControllerNET.plankammo = 5;
					
				//		Playtomic.Log.LevelAverageMetric("Time", 0, Time.timeSinceLevelLoad);
				
				
					
							if (nextLevel > -1)
								Application.LoadLevel(nextLevel);
							else
							{
								PhotonNetwork.LeaveRoom();
							}
					
						}
						else
						{
							statusText = "You must gather your party before venturing forth.";
						}*/
					}
				}
			}
			/*
			if (GUI.Button(new Rect (Screen.width *0.75f, Screen.height *0.8f, Screen.width * 0.25f, Screen.height * 0.1f), "Forgot something..."))
			{
				//shuts off the menu so player can move again. Player must step outside and reenter the exit zone. 
				localPlayerHasReachedEnd = false;
				currController.deactivateMenu();
			}*/
			
			GUI.Label(	new Rect (Screen.width *0.125f, Screen.height *0.9f, Screen.width * 0.75f, Screen.height * 0.1f), statusText);
				
		}	
	
	}

}
