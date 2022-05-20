using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

	[SerializeField] private float waitTime;
	[SerializeField] private GameObject obstaclePrefabs;

	private DifficultType[] pat = {
		DifficultType.Easy,
		DifficultType.Easy,
		DifficultType.Easy,
		DifficultType.Hard,
		DifficultType.Hard,
	};
	private int countDiff = 0;
	private int sumDiff;

	private float tempTime;

	void Start(){
		tempTime = waitTime - Time.deltaTime;
		sumDiff = pat.Length;
	}

	void LateUpdate () {
		if(GameManager.Instance.GameState()){
			tempTime += Time.deltaTime;
			if(tempTime > waitTime){
				// Wait for some time, create an obstacle, then set wait time to 0 and start again
				tempTime = 0;
				GameObject pipeClone = Instantiate(obstaclePrefabs, transform.position, transform.rotation);
				pipeClone.GetComponent<ObstacleBehaviour>().SetDif(pat[countDiff]);
					countDiff++;
				if (countDiff >= sumDiff) countDiff = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.transform.parent != null){
			Destroy(col.gameObject.transform.parent.gameObject);
		}else{
			Destroy(col.gameObject);
		}
	}

}
