using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EmoteButton : MonoBehaviour {
	private void Awake() {
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	private void OnClick() {
		if (this.emote != null) {
			string[] array = this.emote.name.Split(new char[] { '/' });
			ReplaceableSingleton<Player>.main.Emote(array[1]);
		}
		if (this.image != null) {
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.image.transform.DOScale(Vector3.one * EmoteButton.PULSE_SCALE_FACTOR, 0.1f));
			sequence.Append(this.image.transform.DOScale(Vector3.one * EmoteButton.NORMAL_SCALE_FACTOR, 0.1f));
		}
	}

	public Item emote;
	public GameObject image;
	public static float NORMAL_SCALE_FACTOR = 1.4f;
	public static float PULSE_SCALE_FACTOR = 1.8f;
}
