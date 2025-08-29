private void OnAccessoriesChanged(List<Item> accessories) {
    this.flyAccessory = null;
    this.stompAccessory = null;
    Item previousLightAccessory = this.lightAccessory;
    this.lightAccessory = null;

    foreach (Item accessory in this.inventory.accessories) {
        if (this.flyAccessory == null && (accessory.IsUsableType(Item.Use.Fly) || accessory.IsUsableType(Item.Use.Hover) || accessory.IsUsableType(Item.Use.Propel))) {
            this.flyAccessory = accessory;
            this.flyAccessoryPower = accessory.power;
        }

        if (accessory.light > 6f) {
            if (previousLightAccessory == null) {
                this.lightTurnedOn = true;
            }
            this.lightAccessory = accessory;
        }
    }

    foreach (Item hiddenItem in this.inventory.hidden) {
        if (hiddenItem.action == Item.Action.Exoleg) {
            this.stompAccessory = hiddenItem;
        }
    }

    this.avatar.steampackItem = this.flyAccessory;
    int backpackCode = this.flyAccessory != null ? this.flyAccessory.code : 0;
    this.UpdatePlayerLight();
    Dictionary<string, object> dictionary = new Dictionary<string, object> { { "u", backpackCode } };
    this.avatar.Change(dictionary);
    foreach (string skillName in this.skills.Keys) {
        this.skillBonuses[skillName] = this.inventory.SkillBonus(skillName);
    }
    Messenger.Broadcast<bool>("playerShieldEquipped", this.AccessoryWithAction(Item.Action.Shield) != null);
    this.SkillsChanged();
    this.MaxHealthChanged();
    this.MaxSteamChanged();
}
