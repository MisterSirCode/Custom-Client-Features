private void StompComplete() {
    this.lastStompedAt = Time.time;
    this._velocity.y = this._velocity.y * 0.333f;
    Effect effect = this.stompAccessory?.emitter ?? Config.main.EffectByName("shadow steam");
    if (effect != null) {
        effect.Spawn(this.position.x, -this.position.y, global::UnityEngine.Random.Range(8, 12));
    }
}
