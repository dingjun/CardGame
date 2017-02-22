using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandChecker : MonoBehaviour {

	public enum Type {
		None,
		Solo, Chain,
		Pair, PairChain,
		Trio, Trio1, Trio2,
		Airplane, Airplane1, Airplane2,
		// -------------TODO-------------
		Four1, Four2,
		SpaceShuttle, SpaceShuttle1, SpaceShuttle2,
		// -------------TODO-------------
		Bomb, Rocket
	};

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private int[] Count (List<Card> cards) {
		int[] stat = new int[5] { 0, 0, 0, 0, 0 };

		for (int i = 1, index = 0; i <= cards.Count; ++i) {
			if (i == cards.Count || cards[i].value != cards[index].value) {
				++stat[i - index];
				index = i;
			}
		}

		return stat;
	}

	private bool CheckChain (List<Card> cards) {
		if (cards[0].rank >= 15) {
			return false;
		}

		for (int i = 1; i < cards.Count; ++i) {
			if (cards[i].rank != cards[i - 1].rank - 1) {
				return false;
			}
		}
		return true;
	}

	public void Check (List<Card> cards, out Type type, out int chainLength, out int rank) {
		type = Type.None;
		chainLength = 0;
		rank = 0;

		int[] stat = Count(cards);

		if (cards.Count <= 5) {
			switch (cards.Count) {
				case 1:
					// Solo
					type = Type.Solo;
					rank = cards[0].rank;
					break;
				case 2:
					if (stat[2] == 1) {
						if (cards[0].value == 14) {
							// Rocket
							type = Type.Rocket;
						}
						else {
							// Pair
							type = Type.Pair;
							rank = cards[0].rank;
						}
					}
					break;
				case 3:
					if (stat[3] == 1) {
						// Trio
						type = Type.Trio;
						rank = cards[0].rank;
					}
					break;
				case 4:
					if (stat[4] == 1) {
						// Bomb
						type = Type.Bomb;
						rank = cards[0].rank;
					}
					else if (stat[3] == 1) {
						// Trio1
						type = Type.Trio1;
						rank = cards[1].rank;
					}
					break;
				case 5:
					if (stat[1] == 5 && CheckChain(cards)) {
						// Chain(5)
						type = Type.Chain;
						chainLength = 5;
						rank = cards[0].rank;
					}
					else if (stat[3] == 1 && stat[2] == 1) {
						// Trio2
						type = Type.Trio2;
						rank = cards[2].rank;
					}
					break;
			}
		}
		else {
			if (stat[1] == cards.Count && CheckChain(cards)) {
				// Chain
				type = Type.Chain;
				chainLength = cards.Count;
				rank = cards[0].rank;
			}
			else if (stat[2] * 2 == cards.Count) {
				List<Card> chainCards = new List<Card>(); ;
				for (int i = 0; i < cards.Count; i += 2) {
					chainCards.Add(cards[i]);
				}
				if (CheckChain(chainCards)) {
					// PairChain
					type = Type.PairChain;
					chainLength = chainCards.Count;
					rank = chainCards[0].rank;
				}
			}
			else if (stat[3] * 3 == cards.Count) {
				List<Card> chainCards = new List<Card>(); ;
				for (int i = 0; i < cards.Count; i += 3) {
					chainCards.Add(cards[i]);
				}
				if (CheckChain(chainCards)) {
					// Airplane
					type = Type.Airplane;
					chainLength = chainCards.Count;
					rank = chainCards[0].rank;
				}
			}
			else if (stat[1] == stat[3] && stat[1] + stat[3] * 3 == cards.Count) {
				List<Card> chainCards = new List<Card>();
				for (int i = 0; i < cards.Count - 2; ++i) {
					if (cards[i].value == cards[i + 2].value) {
						chainCards.Add(cards[i]);
						i += 2;
					}
				}
				if (CheckChain(chainCards)) {
					// Airplane1
					type = Type.Airplane1;
					chainLength = chainCards.Count;
					rank = chainCards[0].rank;
				}
			}
			else if (stat[2] == stat[3] && stat[2] * 2 + stat[3] * 3 == cards.Count) {
				List<Card> chainCards = new List<Card>();
				for (int i = 0; i < cards.Count - 2; i += 2) {
					if (cards[i].value == cards[i + 2].value) {
						chainCards.Add(cards[i]);
						++i;
					}
				}
				if (CheckChain(chainCards)) {
					// Airplane2
					type = Type.Airplane2;
					chainLength = chainCards.Count;
					rank = chainCards[0].rank;
				}
			}
		}
	}
}
