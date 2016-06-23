using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCamera : NetworkBehaviour {
    public GameObject imageTarget;
	private string OFFLINE_CAMERA = "OfflineCamera";
	public GameObject OnlineCamera;
	private GameObject vrWorld;
	private string VR_WORLD = "VRWorld";

	[Command]
	public void CmdVRChild(GameObject vrPlayer, GameObject vrSpawn) {
		vrPlayer.transform.SetParent (vrSpawn.transform);
	}

	public void OnTrackingLost() {
		if (isServer)
			return;

		if (!isLocalPlayer || base.connectionToServer.connectionId != 1)
			return;

		vrWorld.SetActive (false);
	}

	public void OnTrackingFound() {
		if (isServer)
			return;

		if (!isLocalPlayer || base.connectionToServer.connectionId != 1)
			return;

		vrWorld.SetActive (true);
	}

   public override void OnStartLocalPlayer () {
		base.OnStartLocalPlayer ();

		GameObject offlineCamera = GameObject.Find (OFFLINE_CAMERA);
		offlineCamera.SetActive (false);
		vrWorld = GameObject.Find (VR_WORLD);

		Debug.Log ("onStartLocalPlayer " + base.connectionToServer.connectionId);

		GameObject camera;
		camera = Instantiate (OnlineCamera) as GameObject;
		if (base.connectionToServer.connectionId == 0) {
			camera.transform.parent = this.gameObject.transform;
			camera.transform.position = this.gameObject.transform.position;
			CmdVRChild (this.gameObject, vrWorld);
		} else {
			camera.SetActive (true);
			GameObject.Find ("PlayerCube(Clone)").transform.SetParent (vrWorld.transform);
			GameObject.Find ("SkyDome").SetActive (false);
		}

	}
}