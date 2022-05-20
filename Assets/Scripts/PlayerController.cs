using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float thrust, minTiltSmooth, maxTiltSmooth, hoverDistance, hoverSpeed;
	[SerializeField] Animator[] playerSkin;
	[SerializeField] Material matPlayer;
	private bool isDead = false;
	private Animator animPlayer;
	private bool start;
	private float timer, tiltSmooth, y;
	private Rigidbody2D playerRigid;
	private Quaternion downRotation, upRotation;

	public float speedDown = 1f; 



	void Start () {
		tiltSmooth = maxTiltSmooth;
		playerRigid = GetComponent<Rigidbody2D> ();
		downRotation = Quaternion.Euler (0, 0, -90);
		upRotation = Quaternion.Euler (0, 0, 35);

		//Random skin
		animPlayer = playerSkin[Random.Range(0, playerSkin.Length)];
		animPlayer.gameObject.SetActive(true);
		SetGrayscale(0);
	}

	void Update () {
		if (isDead) return;
		if (!start) {
			// Hover the player before starting the game
			timer += Time.deltaTime;
			y = hoverDistance * Mathf.Sin (hoverSpeed * timer);
			transform.localPosition = new Vector3 (0, y, 0);
		} else {
			// Rotate downward while falling
			transform.rotation = Quaternion.Lerp (transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
			transform.localPosition -= Vector3.up * speedDown * Time.deltaTime;
		}
		// Limit the rotation that can occur to the player
		transform.rotation = new Quaternion (transform.rotation.x, transform.rotation.y, Mathf.Clamp (transform.rotation.z, downRotation.z, upRotation.z), transform.rotation.w);
	}

	void LateUpdate () {
		if (GameManager.Instance.GameState ()) {
			if (Input.GetMouseButtonDown (0)) {
				if(!start){
					// This code checks the first tap. After first tap the tutorial image is removed and game starts
					start = true;
					GameManager.Instance.GetReady ();
					animPlayer.speed = 2;
				}
				//playerRigid.gravityScale = 1f;
				tiltSmooth = minTiltSmooth;
				transform.rotation = upRotation;
				//playerRigid.velocity = Vector2.zero;
				// Push the player upwards
				//playerRigid.AddForce (Vector2.up * thrust);
				SoundManager.Instance.PlayTheAudio("Flap");
				transform.localPosition += Vector3.up * speedDown*60f * Time.deltaTime; 
			}
		}
		//if (playerRigid.velocity.y < -1f) {
		//	// Increase gravity so that downward motion is faster than upward motion
		//	tiltSmooth = maxTiltSmooth;
		//	playerRigid.gravityScale = 2f;
		//}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.transform.CompareTag ("Score")) {
			Destroy (col.gameObject);
			GameManager.Instance.UpdateScore ();
		} else if (col.transform.CompareTag ("Obstacle")) {
			// Destroy the Obstacles after they reach a certain area on the screen
			//foreach (Transform child in col.transform.parent.transform) {
			//	child.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			//}
			KillPlayer ();
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.transform.CompareTag ("Ground")) {
			Debug.Log("Die");
			playerRigid.simulated = false;
			KillPlayer ();
			transform.rotation = downRotation;
		}
	}

	public void KillPlayer () {
		GameManager.Instance.EndGame ();
		playerRigid.velocity = Vector2.zero;
		// Stop the flapping animation
		animPlayer.enabled = false;
		if (!isDead)
		{
			isDead = true;
			StartCoroutine(IE_Grayscale(0.5f));
		}
	}


	// Player Die
	private IEnumerator IE_Grayscale(float _duration)
    {
		float _time = 0;
		while (_time < _duration)
        {
			_time += Time.deltaTime;
			yield return null;
			SetGrayscale(_time/ _duration); 
		}
		SetGrayscale(1f);

	}
	private void SetGrayscale(float _amount)
    {
		matPlayer.SetFloat("_GrayscaleAmount", _amount);
    }
}