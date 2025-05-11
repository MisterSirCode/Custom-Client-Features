using System;
using UnityEngine;
using UnityEngine.UI;

public class EmotesWindow : MonoBehaviour {
	private void Awake() {
		Debug.Log("Awakening emotes window!");
		this.primaryWindow = base.GetComponent<PrimaryWindow>();
		GameObject gameObject = new GameObject("Emotes Panel", new Type[] {
			typeof(GridLayoutGroup),
			typeof(CanvasRenderer),
			typeof(EmotesPanel)
		});
		GridLayoutGroup component = gameObject.GetComponent<GridLayoutGroup>();
		component.cellSize.Set(100f, 100f);
		component.spacing.Set(10f, 10f);
		this.panel = gameObject.GetComponent<EmotesPanel>();
		this.primaryWindow.AddTabWithGameObject("Emotes", "icon-detail-profile", gameObject);
		this.primaryWindow.HideTabPanel();
		this.primaryWindow.Ready();
		Messenger.AddListener("emotesToggled", new Callback(this.OnEmotesToggled));
	}

	private void OnEmotesToggled() {
		this.panel.UpdateEmotes();
		this.primaryWindow.Toggle();
	}

	private PrimaryWindow primaryWindow;
	private EmotesPanel panel;
}
