using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

	public List<Card> cards;
	public HandChecker.Type type;
	public int chainLength;
	public int rank;

	void Awake () {
		cards = new List<Card>();
		Initialize();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void Initialize () {
		type = HandChecker.Type.None;
		chainLength = 0;
		rank = 0;
	}

	public void Clear () {
		foreach (Card card in cards) {
			Destroy(card.gameObject);
		}
		cards.Clear();
		Initialize();
	}

	public void Align () {
		Vector3 newPosition = Vector3.zero;
		foreach (Card card in cards) {
			card.transform.localPosition = newPosition;
			newPosition += new Vector3(0.4f, 0.0f, -0.001f);
		}
	}

	//public void PutBack () {
	//	deck.cards.AddRange(cards);
	//	foreach (Card card in cards) {
	//		card.Flip(false);
	//		card.ChangeParent(deck.transform);
	//	}
	//	cards.Clear();
	//	Initialize();
	//}
}
