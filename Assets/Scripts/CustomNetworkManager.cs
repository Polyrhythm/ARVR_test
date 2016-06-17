using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {
	public GameObject ARPlayer;
	public GameObject VRPlayer;
	public GameObject arSpawn;
	public GameObject vrSpawn;
    private GameObject arPlayer;
    private GameObject vrPlayer;

	public override void OnClientConnect(NetworkConnection conn) {
		ClientScene.AddPlayer (conn, 0);
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		if (CustomNetworkManager.singleton.numPlayers == 0) {
			Debug.Log ("vrSpawn");
			vrPlayer = (GameObject)Instantiate (VRPlayer, vrSpawn.transform.position, Quaternion.identity);
			NetworkServer.AddPlayerForConnection (conn, vrPlayer, playerControllerId);
		} else {
			Debug.Log ("arSpawn");
			arPlayer = (GameObject)Instantiate (ARPlayer, arSpawn.transform.position, Quaternion.identity);
			NetworkServer.AddPlayerForConnection (conn, arPlayer, playerControllerId);
		}
	}
}
