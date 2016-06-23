using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ARActions : NetworkBehaviour {
	private GameObject arSpawn;
	private NetworkInstanceId arSpawnId;
	public GameObject enemyPrefab;
	private GameObject plane;

	[Client]
	public void OnTrackingFound() {
		if (arSpawn)
			return;

		arSpawn = GameObject.Find ("ARSpawns");
		arSpawnId = arSpawn.GetComponent<NetworkIdentity>().netId;
	}

	public void Update() {
		if (!isLocalPlayer)
			return;

		if (Input.GetMouseButtonDown (0)) {
			if (!plane)
				plane = GameObject.Find ("Plane");

			RaycastHit hit;
			Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);

			if (!Physics.Raycast (ray, out hit))
				return;

			Transform objectHit = hit.transform;

			if (!objectHit || objectHit.name != "Plane")
				return;

			Vector3 pos = hit.point;
			CmdAddARSpawnable (pos, arSpawnId);
		}
	}

	[Command]
	public void CmdAddARSpawnable(Vector3 pos, NetworkInstanceId spawnId) {
		GameObject arSpawn = NetworkServer.FindLocalObject (spawnId);

		GameObject spawnable = (GameObject) Instantiate(enemyPrefab, pos + new Vector3(0, 1, 0), Quaternion.identity);
		spawnable.transform.SetParent (arSpawn.transform);
		NetworkServer.Spawn (spawnable);
		RpcSetSpawnParent (spawnable, pos);
	}

	[ClientRpc]
	public void RpcSetSpawnParent(GameObject spawnable, Vector3 pos) {
		spawnable.transform.SetParent (arSpawn.transform);
	}
}
