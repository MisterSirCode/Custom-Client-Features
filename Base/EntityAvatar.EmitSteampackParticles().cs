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
        Vector2 emitterPosition = this.steampackItem.emitterPosition;
        float[] slotVertices = base.GetSlotVertices("suit");
        Vector2 p0 = new Vector2(slotVertices[6], slotVertices[7]);
        Vector2 px = new Vector2(slotVertices[0], slotVertices[1]);
        Vector2 py = new Vector2(slotVertices[4], slotVertices[5]);
        Vector2 pointOnSlot = p0 + emitterPosition.x * (px - p0) + emitterPosition.y * (py - p0);
        this.emitter.Spawn(pointOnSlot.x, pointOnSlot.y, 1);
        this.lastSteamParticleAt = Time.time;
    }
}
