public void UpdateNameLabel() {
    if (this.isPlayer) return;
    if (this.config.named) {
        if (this.nameLabel != null) {
            if (this.nameIcon != null) {
                if (this.nameIcon.Length > 0) {
                    this.nameLabel.iconSprite.Collection = Singleton<AtlasManager>.main.Collection("customEmoji");
                    this.nameLabel.name = "   " + base.name;
                    this.nameLabel.iconName = this.nameIcon.Replace("orders/", "");
                    this.nameLabel.iconSprite.transform.localScale = new Vector3(5f, 5f, 1f);
                } else {
                    this.nameLabel.name = base.name;
                    this.nameLabel.iconName = null;
                    this.nameLabel.iconSprite.transform.localScale = new Vector3(0f, 0f, 0f);
                }
            } else {
                this.nameLabel.name = base.name;
                this.nameLabel.iconName = null;
                this.nameLabel.iconSprite.transform.localScale = new Vector3(0f, 0f, 0f);
            }
        } else {
            this.nameLabel = Singleton<EntityNameLabelPool>.main.Spawn();
            if (this.nameIcon != null) {
                if (this.nameIcon.Length > 0) {
                    this.nameLabel.iconSprite.Collection = Singleton<AtlasManager>.main.Collection("customEmoji");
                    this.nameLabel.name = "   " + base.name;
                    this.nameLabel.iconName = this.nameIcon.Replace("orders/", "");
                    this.nameLabel.iconSprite.transform.localScale = new Vector3(5f, 5f, 1f);
                } else {
                    this.nameLabel.name = base.name;
                    this.nameLabel.iconName = null;
                    this.nameLabel.iconSprite.transform.localScale = new Vector3(0f, 0f, 0f);
                }
            } else {
                this.nameLabel.name = base.name;
                this.nameLabel.iconName = null;
                this.nameLabel.iconSprite.transform.localScale = new Vector3(0f, 0f, 0f);
            }
        }
    } else {
        if (this.nameLabel != null || this.nameIcon == null || this.nameIcon.Length == 0) {
            this.nameLabel.iconSprite.transform.localScale = new Vector3(0f, 0f, 0f);
            this.nameLabel.iconName = null;
            this.nameLabel.name = null;
        }
    }
    base.StartCoroutine(this.UpdateDistance());
}