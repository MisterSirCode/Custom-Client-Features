protected override void Start() {
    if (GameManager.IsTiny() && !GameManager.IsSteamdeck()) {
        Hotbar.SlotCount = 20;
    }
    this._locationCode = "h";
    this.AddSlots(Hotbar.SlotCount);
    if (GameManager.IsTiny() && !GameManager.IsSteamdeck()) {
        this.SetSlotGroup(0);
        GameObject gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Hotbar Toggle"));
        gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            this.NextSlotGroup();
        });
        gameObject.transform.SetParent(this.slotContainer, false);
    }
}