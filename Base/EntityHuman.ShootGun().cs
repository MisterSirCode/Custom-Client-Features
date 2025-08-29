public Entity ShootGun(Item item, Vector2 pt, bool burst = true) {
    this.AnimateEye("angry", 1f);
    EntityConfig entityConfig = Config.main.entitiesByName.Get(item.spawn);
    if (entityConfig != null) {
        this.lastShootAt = Time.time;
        float num = Mathf.Lerp(0.5f, 0f, (!this.isPlayer) ? 0f : ReplaceableSingleton<Player>.main.AimSteadiness());
        pt.x += global::UnityEngine.Random.Range(-num, num);
        pt.y += global::UnityEngine.Random.Range(-num, num);
        EntityBullet entityBullet = Singleton<EntityBulletPool>.main.Spawn();
        if (entityBullet != null) {
            entityBullet.config = entityConfig;
            entityBullet.item = item;
            entityBullet.local = true;
            entityBullet.parentEntity = this;
            entityBullet.isPlayerBullet = this.isPlayer;
            entityBullet.canHurtPlayer = this.CanShootPlayer();
            entityBullet.Begin(null);
            EntitySlotRelativePositionSender entitySender = new EntitySlotRelativePositionSender(this, item.emitterPosition, (!this.IsVisible()) ? null : "tool-end");
            PositionReceiver positionReceiver = new PositionReceiver(pt);
            positionReceiver.TargetProjectile(entityBullet, entitySender, null, item.speed);
            float armWorldDirection = pt.x <= base.cTransform.position.x ? (this.toolArmBone.rotation + 180f) : (360f - this.toolArmBone.rotation);
            if (item.shootEmitter != null) {
                item.shootEmitter.angleBase = -armWorldDirection;
                Vector2 pointOnSlot = base.GetPointOnSlotAtRelativePosition("tool-end", item.emitterPosition);
                item.shootEmitter.Spawn(pointOnSlot.x, pointOnSlot.y, global::UnityEngine.Random.Range(3, 6));
            }
            base.SetSlotOpacity("tool-end", (item.rate <= 10f) ? 1f : global::UnityEngine.Random.Range(0.2f, 0.8f));
            if (burst && item.burst > 0) {
                base.StartCoroutine(this.ShootBurst(item, pt, item.burst, 0.03f));
            }
            if (item.soundLoop != null) {
                this.soundSource.Loop("weapon", SoundSource.GroupClip(item.soundLoop), 0.5f, Time.deltaTime * 10f);
            }
            return entityBullet;
        }
    }
    return null;
}
