using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class vrActions : NetworkBehaviour {
	public GameObject arrowPrefab;
	public int thrust;

	[Command]
	void CmdFireArrow() {
		Transform head = transform.Find ("vrCube");

		GameObject arrow = (GameObject) Instantiate(
			arrowPrefab,
			head.position - head.forward + new Vector3(0.2f, -0.1f, 0),
			Quaternion.identity
		);

		Physics.IgnoreCollision (arrow.GetComponent<Collider> (), GetComponent<Collider> ());

		arrow.GetComponent<Rigidbody> ().AddForce(head.forward * thrust, ForceMode.Impulse);
		arrow.transform.LookAt (head.position + head.forward);
		NetworkServer.Spawn (arrow);

		Destroy (arrow, 4.0f);
	}

	void Update() {
		if (!isLocalPlayer)
			return;

		if (Input.GetMouseButtonDown (0) && !VRMove.isWalking) {
			CmdFireArrow ();
		}
	}
}
