public void OnCollideWithEntity(Entity entity) {
    Entity attacker = entity;
    if (entity is EntityBullet) {
        entity.rigidbodyVelocity *= 0.2f;
        attacker = ((EntityBullet)entity).parentEntity;
    }
    if (this.movement == Player.Movement.Stomping) {
        this.StompEntity(entity);
        return;
    }
    if (entity.config.damageAmount > 0f && entity.alive) {
        this.Damage(entity.config.damageType, entity.config.damageAmount, attacker);
        if (entity.config.obstacle > 0f) {
            this.movementDamping = entity.config.obstacle;
        }
    }
}