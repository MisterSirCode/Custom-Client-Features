private void Awake() {
    global::InControl.TouchManager.ControlsEnabled = GameManager.IsMobile() || GameManager.IsSteamdeck();
    if (!this.ShouldBeEnabled()) {
        base.enabled = false;
        return;
    }
    TouchScreenKeyboard.hideInput = true;
    this.miningSpikeTransform = this.miningSpike.GetComponent<RectTransform>();
    Messenger.AddListener<ItemSlot, int>("itemDragBegan", new Callback<ItemSlot, int>(this.OnItemDragBegan));
    Messenger.AddListener<ItemSlot, int>("itemDragEnded", new Callback<ItemSlot, int>(this.OnItemDragEnded));
}