private void Start() {
	if (transform.Find("Parchment(Clone)/Scroll Rect").GetComponent<ScrollSpeedSetting>() == null) {
		transform.Find("Parchment(Clone)/Scroll Rect").gameObject.AddComponent<ScrollSpeedSetting>();
	}
}
