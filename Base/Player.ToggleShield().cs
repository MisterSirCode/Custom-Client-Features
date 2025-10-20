public void ToggleShield(bool on) {
    if (on) {
        if (this.activeShield == null) {
            Item shield = AccessoryWithAction(Item.Action.Shield);
            if (Time.time > lastActivatedShieldAt + shieldCooldown) {
                List<Item> accessories = new(inventory.accessories);
                accessories.Reverse();
                foreach (Item accessory in accessories) {
                    if (accessory != null && accessory.category == "prosthetics" && accessory.name.Contains("torso") && accessory.firingDuration > 0.0f) {
                        shield = accessory;
                    }
                }
            }
            this.activeShield = shield;
            if (this.activeShield != null && this.activeShield.firingDuration > 0.0f) {
                this.lastActivatedShieldAt = Time.time;
                this.shieldCooldown = this.activeShield.firingDuration + this.activeShield.firingInterval;
            }
        }
    } else {
        if (this.activeShield != null && this.activeShield.firingDuration > 0.0f && this.lastActivatedShieldAt + this.activeShield.firingDuration > Time.time) return;
        this.activeShield = null;
        this.avatar.shield.color = GraphicsHelper.HexColor("b744ff");
    }
    this.avatar.shieldItem = this.activeShield;
    Command.Identity identity = Command.Identity.InventoryUse;
    object[] array = new object[4];
    array[0] = 1;
    array[1] = (this.activeShield == null) ? 0 : this.activeShield.code;
    array[2] = (this.activeShield != null) ? 1 : 0;
    Command.Send(identity, array);
    Messenger.Broadcast<bool>("playerShieldActivated", on);
}
