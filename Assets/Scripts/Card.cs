using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour {

	public Sprite[] faces;
	public Sprite back;

	public int index;       // sprite index
	public int order;       // order for displaying
	public int value;       // face value
	public int rank;        // card rank (3-17)
	public bool isFaceUP;
	public bool isSelected;

	private int[] indexToOrder = new int[54] {
		9, 5, 53, 49, 45, 41, 37, 33, 29, 25, 21, 17, 13,
		8, 4, 52, 48, 44, 40, 36, 32, 28, 24, 20, 16, 12,
		7, 3, 51, 47, 43, 39, 35, 31, 27, 23, 19, 15, 11,
		6, 2, 50, 46, 42, 38, 34, 30, 26, 22, 18, 14, 10,
		1, 0
	};

	private SpriteRenderer spriteRenderer;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		index = 0;
		order = 0;
		value = 0;
		rank = 0;
		isFaceUP = false;
		isSelected = false;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
		Player player = transform.parent.gameObject.GetComponent<Player>();

		if (player && player.ifInTurn == true) {
			Select(!isSelected);
		}
	}

	private void Show () {
		if (isFaceUP) {
			spriteRenderer.sprite = faces[index];
		}
		else {
			spriteRenderer.sprite = back;
		}
	}

	// based on card index
	private void SetValue () {
		if (index < 52) {
			value = (index % 13) + 1;
		}
		else {
			value = 14;
		}
	}

	// based on card value
	private void SetRank () {
		if (value == 14) {
			rank = index - 52 + 16;
		}
		else if (value <= 2) {
			rank = value + 13;
		}
		else {
			rank = value;
		}
	}

	public void SetIndex (int newIndex) {
		index = newIndex;
		order = indexToOrder[index];
		SetValue();
		SetRank();
		Show();
	}

	public void Flip (bool nextIsFaceUP) {
		isFaceUP = nextIsFaceUP;
		Show();
	}

	public void ChangeParent (Transform parent) {
		transform.parent = parent;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
	}

	public void Select (bool nextIsSelected) {
		Vector3 selectedOffset = new Vector3(0.0f, 0.25f, 0.0f);

		if (isSelected != nextIsSelected) {
			Player player = transform.parent.gameObject.GetComponent<Player>();

			if (nextIsSelected) {
				// selected
				transform.localPosition += selectedOffset;
				player.selectedCards.Add(this);
				player.selectedCards = player.selectedCards.OrderBy(card => card.order).ToList();
			}
			else {
				// unselected
				transform.localPosition -= selectedOffset;
				player.selectedCards.Remove(this);
			}
			isSelected = nextIsSelected;
		}
	}
}
