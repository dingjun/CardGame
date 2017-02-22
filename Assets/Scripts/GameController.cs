using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public Deck deck;
	public Player[] players;
	public Hand[] hands;
	public UnityEngine.UI.Text message;

	public int landlordIndex;
	public int playerInTurnIndex;
	public int highestHandIndex;

	void Awake () {
		landlordIndex = 0;          // Player 1 is default landlord
		playerInTurnIndex = -1;
		highestHandIndex = -1;
	}

	// Use this for initialization
	void Start () {
		deck.Deal(players, landlordIndex);

		players[0].Sort();
		players[1].Sort();
		players[2].Sort();

		playerInTurnIndex = landlordIndex;
		players[playerInTurnIndex].ToggleInTurn(true);
	}

	// Update is called once per frame
	void Update () {

	}

	public void NewGame () {
		SceneManager.LoadScene("DouDizhu");
	}

	public void Pass () {
		if (playerInTurnIndex < 0) {
			return;
		}

		message.text = "";

		if (highestHandIndex < 0) {
			message.text = "Cannot Pass";
			return;
		}

		// next player's turn
		players[playerInTurnIndex].ToggleInTurn(false);
		playerInTurnIndex = (playerInTurnIndex + 1) % 3;
		players[playerInTurnIndex].ToggleInTurn(true);

		if (playerInTurnIndex == highestHandIndex) {
			// all other players passed
			highestHandIndex = -1;
		}
	}

	public void Cancel () {
		if (playerInTurnIndex < 0) {
			return;
		}

		message.text = "";

		if (players[playerInTurnIndex].selectedCards.Count == 0) {
			return;
		}

		players[playerInTurnIndex].Cancel();
	}

	public void Dou () {
		if (playerInTurnIndex < 0) {
			return;
		}

		message.text = "";

		if (players[playerInTurnIndex].selectedCards.Count == 0) {
			message.text = "No Cards Selected";
			return;
		}

		Hand highestHand = null;
		if (highestHandIndex >= 0) {
			highestHand = hands[highestHandIndex];
		}

		if (players[playerInTurnIndex].Dou(highestHand)) {
			highestHandIndex = playerInTurnIndex;

			if (players[playerInTurnIndex].cards.Count == 0) {
				// win
				if (playerInTurnIndex == landlordIndex) {
					message.text = "Landlord WINS!";
				}
				else {
					message.text = "Peasant WINS!";
				}
				players[playerInTurnIndex].ToggleInTurn(false);
				playerInTurnIndex = -1;
			}
			else {
				Pass();
			}
		}
		else {
			message.text = "Cannot Dou";
		}
	}
}
