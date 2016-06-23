using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHealth : NetworkBehaviour {
	public const int maxHealth = 3;

	[SyncVar]
	public int health = maxHealth;

	public void takeDamage(int amount) {
		if (!isServer)
			return;

		health -= amount;

		if (health <= 0) {
			health = maxHealth;
			RpcRespawn ();
		}
	}

	[ClientRpc]
	void RpcRespawn() {
		GameObject vrSpawn = GameObject.Find("VRSpawn");
		transform.position = vrSpawn.transform.position;
	}
}
