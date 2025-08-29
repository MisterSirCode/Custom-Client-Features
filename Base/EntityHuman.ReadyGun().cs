public bool ReadyGun(Item item, Vector2 pt) {
    this.AnimateTool(pt);
    this.UpdateArms(1f);
    EntityConfig entityConfig = Config.main.entitiesByName.Get(item.spawn);
    if (entityConfig != null) {
        float firingInterval;
        if (item.firingInterval < 0) {
            float num = item.rate;
            if (num == 0f) {
                return false;
            }
            if (!this.isPlayer) {
                float num2 = (float)Config.PlayerSettings.quality * 0.4f + (float)ReplaceableSingleton<Ecosystem>.main.entityBulletCount * 0.003f;
                num = Mathf.Lerp(num, num * 0.333f, num2);
            }
            firingInterval = 1f / num;
        } else {
            firingInterval = item.firingInterval;
        }
        if (PlayerDevice.isTouchScreen) {
            firingInterval /= 0.8f;
        }
        if (Time.time < this.lastShootAt + firingInterval) {
            return false;
        }
    }
    return true;
}
