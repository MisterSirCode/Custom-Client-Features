public void BlockDamage(Item blockItem, Vector3 velocity = default(Vector3), bool isFeet = false)
{
    if (Time.time > this.lastEnvironmentalDamageAt + 1f) {
        if (velocity.magnitude >= blockItem.damageMinVelocity) {
            if (blockItem.damageType == Item.Damage.Piercing && this.AccessoryWithUse(Item.Use.Hazmat) != null) return;
            float num = (!isFeet || this.stompAccessory == null) ? 0f : this.stompAccessory.Defense("all");
            if (blockItem.damageType == Item.Damage.Piercing) {
                this.Damage(blockItem.damageType, blockItem.damageAmount * (1f - num), null);
            }
        }
        this.lastEnvironmentalDamageAt = Time.time;
    }
}