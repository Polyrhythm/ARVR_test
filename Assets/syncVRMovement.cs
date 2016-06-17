using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class syncVRMovement : NetworkBehaviour {
	[SyncVar]
	private Vector3 syncPos;

	[SyncVar]
	private Quaternion syncRot;

	[SerializeField] Transform vrTransform;
	[SerializeField] float lerpRate = 15;

	void FixedUpdate() {
		LerpPosition ();

		if (isLocalPlayer && base.connectionToServer.connectionId == 0) {
			transmitPosition (this.gameObject.transform.localPosition, GameObject.Find("vrCube").transform.localRotation);
		}
	}

	void LerpPosition() {
		if (isLocalPlayer)
			return;

		vrTransform.localPosition = Vector3.Lerp (vrTransform.localPosition, syncPos, Time.deltaTime * lerpRate);
		vrTransform.localRotation = Quaternion.Lerp (vrTransform.localRotation, syncRot, Time.deltaTime * lerpRate);
	}

	[Command]
	void CmdProvidePosToServer(Vector3 pos, Quaternion rot) {
		syncPos = pos;
		syncRot = rot;
	}

	[ClientCallback]
	public void transmitPosition(Vector3 pos, Quaternion rot) {
		CmdProvidePosToServer (pos, rot);
	}
}
