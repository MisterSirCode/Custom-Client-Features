private void EmitSteampackParticles() {
    if (this.isPlayer && ReplaceableSingleton<Player>.main.suppressed) {
        return;
    }

    if (this.steampackItem == null) {
        if (this.isPlayer) {
            return;
        }
        this.steampackItem = Item.Get("accessories/jetpack");
    }
    
    if (this.emitter != null && Time.time - this.lastSteamParticleAt > 1f / this.steamRate) {
        Vector2 pointOnSlot = base.GetPointOnSlotAtRelativePosition("suit", this.steampackItem.emitterPosition);
        this.emitter.Spawn(pointOnSlot.x, pointOnSlot.y, 1);
        this.lastSteamParticleAt = Time.time;
    }
}
