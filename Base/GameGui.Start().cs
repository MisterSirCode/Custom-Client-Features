private void Start() {
    Messenger.AddListener<Item, int, int>("inventoryQuantityChanged", new Callback<Item, int, int>(this.OnInventoryQuantityChanged));
    Messenger.AddListener<Dictionary<string, object>>("dialogDismissed", new Callback<Dictionary<string, object>>(this.OnDialogDismissed));
    Messenger.AddListener<GameObject>("activeDialogDismissed", new Callback<GameObject>(this.OnActiveDialogDismissed));
    Messenger.AddListener<object>("zoneEntered", new Callback<object>(this.OnZoneEntered));
    Messenger.AddListener("protectorOverlayToggled", new Callback(this.OnProtectorOverlay));
    Messenger.AddListener<GameObject>("resumeGame", new Callback<GameObject>(this.OnResumeGame));
    Messenger.AddListener("showSettings", new Callback(this.ShowSettings));
    Messenger.AddListener("showSupport", new Callback(this.ShowSupport));
    Messenger.AddListener<FixedPosition>("dialogBlockChanged", new Callback<FixedPosition>(this.OnDialogBlockChanged));
    Messenger.AddListener<bool>("configCompleted", new Callback<bool>(this.OnConfigCompleted));
    Messenger.AddListener<object>("playerNeedsRegistration", new Callback<object>(this.OnPlayerNeedsRegistration));
    Messenger.AddListener("playerRequestedRegistration", new Callback(this.OnPlayerRequestedRegistration));
    Messenger.AddListener<object>("closeLastDrawer", new Callback<object>(this.OnCloseLastDrawer));
    Messenger.AddListener<object>("openUrl", new Callback<object>(this.OnOpenUrl));
    Messenger.AddListener("guiDismissed", new Callback(this.OnGuiDismissed));
    if (GameManager.IsMobile()) {
        Messenger.AddListener<GameObject>("guiDismissedMobile", new Callback<GameObject>(this.OnGuiDismissedMobile));
    }
    Messenger.AddListener<string, object>("settingChanged", new Callback<string, object>(this.OnSettingChanged));
    Messenger.AddListener<ZoneBlock>("blockFrontItemChanged", new Callback<ZoneBlock>(this.OnFrontItemChanged));
    this.AddWindow((!GameManager.IsTiny()) ? "profile" : "game", "Profile", "icon-detail-profile", "Prefabs/Panels/Primary Window").gameObject.AddComponent<ProfileWindow>();
    this.AddWindow("emotes", "Emotes", "icon-detail-profile", "Prefabs/Panels/Primary Window").gameObject.AddComponent<EmotesWindow>();
    this.AddWindow((!GameManager.IsTiny()) ? "map" : "game", "Map", "icon-detail-map", "Prefabs/Panels/Primary Window").gameObject.AddComponent<MapWindow>();
    this.AddWindow("game", "Social", "icon-detail-social", "Prefabs/Panels/Primary Window").gameObject.AddComponent<SocialWindow>();
    this.AddWindow("game", "Shop", "icon-detail-shop", "Prefabs/Panels/Primary Window").gameObject.AddComponent<ShopWindow>();
    this.AddWindow("game", "Portal", "icon-detail-map", "Prefabs/Panels/Primary Window").gameObject.AddComponent<PortalWindow>();
    PrimaryWindow primaryWindow = this.AddWindow("game", "Inventory", "icon-detail-inventory", "Prefabs/Panels/Primary Window");
    primaryWindow.UseAltTabsPanel();
    primaryWindow.gameObject.AddComponent<InventoryWindow>();
    PrimaryWindow primaryWindow2 = this.AddWindow("game", "Crafting", "icon-detail-crafting", "Prefabs/Panels/Primary Window");
    primaryWindow2.UseAltTabsPanel();
    primaryWindow2.gameObject.AddComponent<CraftingWindow>();
    this.screenContainer.visible = false;
    this.protectorRangefinderActive = false;
    Messenger.AddListener<float>("uiScaleChanged", new Callback<float>(this.OnUiScaleChanged));
}