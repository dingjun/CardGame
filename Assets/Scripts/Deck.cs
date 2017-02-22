using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	public GameObject cardPrefab;
	public List<Card> cards;

	void Awake () {
		Random.InitState(System.Environment.TickCount);

		cards = new List<Card>();
		for (int i = 0; i < 54; ++i) {
			GameObject cardGameobject = Instantiate(cardPrefab, transform, false);
			Card card = cardGameobject.GetComponent<Card>();
			card.SetIndex(i);
			cards.Add(card);
		}
		Shuffle();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void Shuffle () {
		for (int i = 0; i < cards.Count - 2; ++i) {
			int j = Random.Range(i, cards.Count - 1);

			// swap
			Card tempCard = cards[i];
			cards[i] = cards[j];
			cards[j] = tempCard;
		}
	}

	public void Deal (Player[] players, int landlordIndex) {
		int index = 0;

		// 17 cards for each player
		for (int i = 0; i < 51; ++i) {
			players[index].cards.Add(cards[0]);
			cards[0].ChangeParent(players[index].transform);
			cards.RemoveAt(0);
			index = (index + 1) % 3;
		}

		// 3 cards for landlord
		for (int i = 0; i < 3; ++i) {
			players[landlordIndex].cards.Add(cards[0]);
			cards[0].ChangeParent(players[landlordIndex].transform);
			cards.RemoveAt(0);
		}
	}

	//public void Collect (Player[] players, Hand[] hands) {
	//	for (int i = 0; i < 3; ++i) {
	//		players[i].PutBack();
	//		hands[i].PutBack();
	//	}
	//}
}
