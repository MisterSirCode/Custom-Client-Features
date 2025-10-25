public Item ActivateMomentaryAccessory(Item.Action action) {
    float lastActivation = this.lastAccessoryActivatedAt.GetFloat(action, -100f);
    Item nonMomentary = null;
    foreach (Item accessory in this.inventory.accessories) {
        if (accessory.action == action) {
            if (accessory.firingInterval > 0f || accessory.firingDuration > 0f) {
                if (lastActivation + accessory.firingDuration + accessory.firingInterval <= Time.time) {
                    this.lastAccessoryActivatedAt[action] = Time.time;
                    return accessory;
                }
            } else {
                if (nonMomentary == null) {
                    nonMomentary = accessory;
                }
            }
        }
    }
    return nonMomentary;
}
