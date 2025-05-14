using System;
using UnityEngine;
using UnityEngine.UI;

public class ControllerToggler : MonoBehaviour {	
    public void Start() {
		this.style.normal.background = null;
	}

	public void OnGUI() {
		if (!ControllerToggler.enablegui) return;
		bool flag = false;
		if (PlayerPrefs.HasKey("controllerEnabled")) {
			flag = PlayerPrefs.GetInt("controllerEnabled") == 1;
		}
		if (GUI.Button(new Rect(10f, 64f, 200f, 50f), new GUIContent(flag ? "<color=#afa>Disable Controller</color>" : "<color=#faa>Enable Controller</color>"), this.style)) {
			PlayerPrefs.SetInt("controllerEnabled", (!flag) ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	public GUIStyle style = new GUIStyle();
	public static bool enablegui = false;
}