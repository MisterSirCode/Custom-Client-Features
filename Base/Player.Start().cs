private void Start() {
    this.inventory = new Inventory();
    this.skills = new Dictionary<string, int>();
    this.skillBonuses = new Dictionary<string, int>();
    this.achievements = new List<string>();
    this.achievementProgress = new Dictionary<string, int>();
    this.followees = new Dictionary<string, string>();
    this.followers = new Dictionary<string, string>();
    Messenger.AddListener<Item>("activeHotbarItemChanged", new Callback<Item>(this.SetPrimaryItem));
    Messenger.AddListener<Item>("primaryItemChanged", new Callback<Item>(this.OnPrimaryItemChanged));
    Messenger.AddListener<object>("socialInfoReady", new Callback<object>(this.OnSocialInfoReady));
    Messenger.AddListener<List<Item>>("accessoriesChanged", new Callback<List<Item>>(this.OnAccessoriesChanged));
    Messenger.AddListener("onPlayerLightToggled", new Callback(this.OnPlayerLightToggled));
    Messenger.AddListener("shieldOn", new Callback(this.OnShieldOn));
    Messenger.AddListener("shieldOff", new Callback(this.OnShieldOff));
    Messenger.AddListener("shieldToggle", new Callback(this.OnShieldToggle));
    this.foregroundLayerMask = 1 << LayerMask.NameToLayer("Foreground");
    this.groundedLayerMask = this.foregroundLayerMask;
    this.entityLayerMask = 1 << LayerMask.NameToLayer("Entities");
    this.SetControllerPlatformMask();
    base.gameObject.AddComponent(typeof(HintManager));
}
