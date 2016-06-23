using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour {
	private float lastBlowDealt;

	void Start() {
		lastBlowDealt = Time.time;
	}

	public void hurtPlayer(GameObject player, control_script enemyController) {
		if (Time.time - lastBlowDealt >= 2) {
			PlayerHealth playerHealth = player.GetComponent<PlayerHealth> ();
			enemyController.Attack ();
			playerHealth.takeDamage (1);
			lastBlowDealt = Time.time;
		}
	}
}
