public override void Awake() {
	base.Awake();
	Messenger.AddListener<EntityAvatar>("peerEntered", new Callback<EntityAvatar>(this.OnPeerEntered));
	Messenger.AddListener<EntityAvatar>("peerExited", new Callback<EntityAvatar>(this.OnPeerExited));
	transform.Find("Parchment(Clone)/Scroll Rect").gameObject.AddComponent<ScrollSpeedSetting>();
}
