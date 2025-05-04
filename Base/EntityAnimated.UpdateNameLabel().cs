protected void UpdateNameLabel() {
    if (this.isPlayer || !this.config.named) {
        return;
    }
    if (this.nameLabel == null) {
        this.nameLabel = Singleton<EntityNameLabelPool>.main.Spawn();
    }
    this.nameLabel.name = ((this.nameIcon == null) ? base.name : ("   " + base.name));
    if (this.nameIcon.Contains("crow-5")) {
        ExternalConsole.Log("Found One", this.nameIcon);
        Sprite crow = ExternalSpriteLoader.GetSprite("crow-5");
        GameObject texObj = tk2dSprite.CreateFromTexture(crow.texture, tk2dSpriteCollectionSize.Default(), crow.rect, Vector2.zero);
        this.nameLabel.iconSprite = texObj.GetComponent<tk2dSprite>();
        this.nameLabel.gameObject.AddComponent<Image>().sprite = crow;
    } else {
        ExternalConsole.Log("Loading Sprite", this.nameIcon);
        this.nameLabel.iconName = ((this.nameIcon == null) ? null : this.nameIcon.Replace("orders/", "order-"));
    }
    this.nameLabel.nameLabel.color = ((!this.nameHighlighted) ? Color.white : GraphicsHelper.HexColor(GameGui.TitleColorHex));
    base.StartCoroutine(this.UpdateDistance());
}