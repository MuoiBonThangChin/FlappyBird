using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DifficultType
{
	Easy,
	Hard
}
public class ObstacleBehaviour : MonoBehaviour {
	
	[SerializeField] private float moveSpeed;
	[SerializeField] private Transform transTop;
	[SerializeField] private Transform transBot;
	private float posEasyMin = -6f;
	private float posEasyMax = -3f;

	void Update () {
		if(GameManager.Instance.GameState()){
			// Continuosly move the obstacles to the left if the game hasn't ended
			transform.position = new Vector2(transform.position.x - Time.deltaTime * moveSpeed, transform.position.y);
		}
	}

	
	public void SetDif(DifficultType _type)
    {
		float _y = Random.Range(posEasyMin, posEasyMax);
		transBot.localPosition = Vector3.up * _y;
		switch (_type)
        {
			case DifficultType.Easy: 
				
				transTop.localPosition = Vector3.up * (_y + 11f);
				break;
			case DifficultType.Hard:
				transTop.localPosition = Vector3.up * (_y + +10f);
				break;

		}
    }
}
