using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

	public List<Card> cards;
	public List<Card> selectedCards;
	public bool ifInTurn;

	private HandChecker handChecker;
	private Hand hand;

	void Awake () {
		cards = new List<Card>();
		selectedCards = new List<Card>();
		ifInTurn = false;

		handChecker = GetComponent<HandChecker>();
		hand = GetComponentsInChildren<Hand>()[0];
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void Align () {
		Vector3 newPosition = Vector3.zero;
		foreach (Card card in cards) {
			card.transform.localPosition = newPosition;
			newPosition += new Vector3(0.4f, 0.0f, -0.001f);
		}
	}

	private void Flip (bool isFaceUP) {
		foreach (Card card in cards) {
			card.Flip(isFaceUP);
		}
	}

	public void Sort () {
		cards = cards.OrderBy(card => card.order).ToList();
		Align();
	}

	public void ToggleInTurn (bool nextIfInTurn) {
		Vector3 scaleInTurn = new Vector3(2.0f, 2.0f, 2.0f);
		Vector3 scaleNotInTurn = Vector3.one;

		if (nextIfInTurn) {
			hand.Clear();
			transform.localScale = scaleInTurn;
			Flip(true);
		}
		else {
			Cancel();
			transform.localScale = scaleNotInTurn;
			Flip(false);
		}
		ifInTurn = nextIfInTurn;
	}

	public void Cancel () {
		foreach (Card card in cards) {
			card.Select(false);
		}
	}

	public bool Dou (Hand highestHand) {
		HandChecker.Type selectedType;
		int selectedChainLength;
		int selectedRank;
		handChecker.Check(selectedCards, out selectedType, out selectedChainLength, out selectedRank);

		if (selectedType == HandChecker.Type.None) {
			return false;
		}

		if (highestHand) {
			if (selectedType < HandChecker.Type.Bomb) {
				// regular hand
				if (highestHand.type != selectedType
				|| highestHand.chainLength != selectedChainLength
				|| highestHand.rank >= selectedRank) {
					return false;
				}
			}
			else if (selectedType == HandChecker.Type.Bomb) {
				// bomb
				if (highestHand.type == HandChecker.Type.Bomb
				&& highestHand.rank > selectedRank) {
					return false;
				}
			}
		}

		int i = 0;
		while (i < cards.Count) {
			if (cards[i].isSelected) {
				cards[i].Select(false);
				hand.cards.Add(cards[i]);
				cards[i].ChangeParent(hand.transform);
				cards.RemoveAt(i);
			}
			else {
				++i;
			}
		}
		Align();
		hand.Align();

		hand.type = selectedType;
		hand.chainLength = selectedChainLength;
		hand.rank = selectedRank;

		return true;
	}

	//public void PutBack () {
	//	deck.cards.AddRange(cards);
	//	foreach (Card card in cards) {
	//		card.Flip(false);
	//		card.ChangeParent(deck.transform);
	//	}
	//	cards.Clear();
	//}
}
