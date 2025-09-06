public void UpdateNameLabel() {
    if (this.isPlayer || !this.config.named) {
        return;
    }
    if (this.nameLabel == null) {
        this.nameLabel = Singleton<EntityNameLabelPool>.main.Spawn();
			this.oldCollection = this.nameLabel.iconSprite.Collection;
    }
    this.nameLabel.name = ((this.nameIcon == null) ? base.name : ("   " + base.name));
    if (this.nameIcon != null) {
        this.nameLabel.iconSprite.Collection = Singleton<AtlasManager>.main.Collection("customEmoji");
        this.nameLabel.iconName = ((this.nameIcon == null) ? null : this.nameIcon.Replace("orders/", ""));
        if (this.nameLabel.iconSprite) {
            this.nameLabel.iconSprite.transform.localScale = new Vector3(5f, 5f, 1f);
        }
    } else {
        this.nameLabel.iconSprite.transform.localScale = new Vector3(1f, 1f, 1f);
        this.nameLabel.iconName = null;
    }
    this.nameLabel.nameLabel.color = ((!this.nameHighlighted) ? Color.white : GraphicsHelper.HexColor(GameGui.TitleColorHex));
    base.StartCoroutine(this.UpdateDistance());
}