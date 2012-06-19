using UnityEngine;
using System.Collections;

public class buttonToDestroy : Photon.MonoBehaviour {
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerEnter(Collider other){
		print("Destroyed all built objects!");
		//   print("Starting " + Time.time);
        StartCoroutine(WaitAndPrint(1.0F));
       // print("Before WaitAndPrint Finishes " + Time.time);
    
  
		GameObject[] platformsCreated = GameObject.FindGameObjectsWithTag("PlacedPlatform");
		foreach(GameObject creation in platformsCreated){
			creation.transform.position += Vector3.up * 100.0F;
			creation.renderer.enabled = false;
		//	PhotonNetwork.Destroy(creation);
		}
		
		GameObject[] blocksCreated = GameObject.FindGameObjectsWithTag("PlacedBlock");
		foreach(GameObject creation in blocksCreated){
			creation.transform.position += Vector3.up * 100.0F;
			creation.renderer.enabled = false;
		//	PhotonNetwork.Destroy(creation);
		}
		
		ThirdPersonControllerNET.blockammo = 1;
		ThirdPersonControllerNET.plankammo = 5;
		//triggerCsScript.objectsTriggeringSameButton = 0;
		
	}
  IEnumerator WaitAndPrint(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        GameObject[] platformsCreated = GameObject.FindGameObjectsWithTag("PlacedPlatform");
		foreach(GameObject creation in platformsCreated){
		//	creation.transform.position += Vector3.up * 100.0F;
			PhotonNetwork.Destroy(creation);
		}
		
		GameObject[] blocksCreated = GameObject.FindGameObjectsWithTag("PlacedBlock");
		foreach(GameObject creation in blocksCreated){
		//	creation.transform.position += Vector3.up * 100.0F;
			PhotonNetwork.Destroy(creation);
		}
		
    }
}
