using UnityEngine;
using System.Collections;

public class arrow : MonoBehaviour {
	private const string ENEMY = "Enemy";

	void FixedUpdate() {
		transform.LookAt (transform.position, transform.forward);
	}

	void OnCollisionEnter(Collision collision) {
		GameObject objectHit = collision.gameObject;

		Debug.Log ("hit! " + objectHit);

		if (objectHit.tag == ENEMY) {
			Destroy (gameObject);
			Destroy (objectHit);

			// EnemySight enemy = objectHit.GetComponent<EnemySight> ();
			// if (enemy.disabled)
			//	return;

			// enemy.disable ();
		}
	}
}
