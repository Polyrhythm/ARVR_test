using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {
	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public Vector3 personalLastSighting;
	public bool disabled;

	private GameObject player;
	private SphereCollider col;
	private Vector3 previousSighting;
	private Vector3 resetPosition = new Vector3(0, 0, 0);
	private float distanceToPlayer;
	private EnemyCombat combat;
	private NavMeshAgent nav;
	private control_script controller;

	void Start() {
		disabled = false;
		col = GetComponent<SphereCollider> ();

		personalLastSighting = resetPosition;
		previousSighting = resetPosition;
		combat = GetComponent<EnemyCombat> ();
		nav = GetComponent<NavMeshAgent> ();
		controller = GetComponent<control_script> ();
	}

	public void disable() {
		disabled = true;
		controller.Damage ();

		StartCoroutine (delayEnable (5));
	}

	IEnumerator delayEnable(int seconds) {
		yield return new WaitForSeconds (seconds);

		enable ();
	}

	void enable() {
		disabled = false;
		controller.OtherIdle ();
	}

	void Update() {
		if (disabled)
			return;

		if (!player)
			player = GameObject.FindGameObjectWithTag ("Player");

		previousSighting = personalLastSighting;

		if (previousSighting == resetPosition) {
			if (transform.position == previousSighting) {
				controller.OtherIdle ();
			}

			return;
		}

		controller.Walk ();

		nav.SetDestination (previousSighting);

		distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);
		if (distanceToPlayer <= 2) {
			combat.hurtPlayer (player, controller);
		}
	}

	void OnTriggerStay(Collider other) {
		if (disabled)
			return;
		
		if (other.gameObject == player) {
			playerInSight = false;
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle (direction, transform.forward);

			if (angle < fieldOfViewAngle * 0.5f) {
				// Player is inside of the FOV of the enemy.
				RaycastHit hit;

				if (
					Physics.Raycast (
						transform.position + new Vector3(0, 0.2f, 0),
						direction.normalized,
						out hit,
						col.radius
					)
				) {
					if (hit.collider.gameObject == player) {
						playerInSight = true;
						personalLastSighting = player.transform.position; 
					}
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == player) {
			playerInSight = false;
		}
	}
}

