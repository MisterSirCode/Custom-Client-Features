private void UpdateGraphics() {
    Item item = this._zoneBlock.EffectiveFrontItem();
    Item backItem = this._zoneBlock.backItem;
    Item baseItem = this._zoneBlock.baseItem;
    Item liquidItem = this._zoneBlock.liquidItem;
    bool whole = item.whole;
    int num = this._zoneBlock.Wholeness();
    if (item.visible) {
        this.frontSprite = this.GetAccessorySprite();
        this.frontSprite.Place(item, (int)this._zoneBlock.frontMod, (float)((!whole) ? 2 : (-2)));
    }
    if (this._zoneBlock.liquidItem.visible && this._zoneBlock.liquidMod > 0) {
        this.liquidSprite = this.GetAccessorySprite();
        this.liquidSprite.Place(this._zoneBlock.liquidItem, (int)this._zoneBlock.liquidMod, -1f);
    }
    if (!item.opaque || (item.mod == Item.Mod.Decay && this._zoneBlock.frontMod >= 2) || (item.maskContinuity != null && num != 15)) {
        if (backItem.visible) {
            this.backSprite = this.GetAccessorySprite();
            this.backSprite.Place(backItem, (int)this._zoneBlock.backMod, 3f);
        }
        if ((!backItem.opaque || (backItem.mod == Item.Mod.Decay && this._zoneBlock.backMod >= 2) || (backItem.maskContinuity != null && num != 15)) && baseItem.visible) {
            this.baseSprite = this.GetAccessorySprite();
            this.baseSprite.Place(baseItem, 0, 4f);
        }
    }
    if (!whole && (item.tileable || item.opaque || backItem.tileable || backItem.opaque || baseItem.opaque)) {
        if (Config.main.wholenessSprites != null) {
            this.GetAccessorySprite().PlaceContinuity(Config.main.wholenessSprites, 15 - num, true, 2.5f, new Color(1f, 1f, 1f, 0.5f));
        }
        ZoneBlock zoneBlock = this._zoneBlock.Top();
        if (zoneBlock != null) {
            bool flag = zoneBlock.frontItem.shadow;
            if (!flag) {
                Item item2 = ((zoneBlock.Left() == null) ? null : zoneBlock.Left().frontItem);
                if (item2 != null) {
                    flag = item2.shadow && !item2.tileable && item2.blockSize.width > 1;
                }
            }
            if (flag) {
                this.GetAccessorySprite().PlacePositioned(Singleton<AtlasManager>.main.Sprite("borders/earth-deep", true), Item.Alignment.None, 1f, 0f, false, 2.5f, new Color(1f, 1f, 1f, 0.5f));
            }
        }
    }
    if (liquidItem != null && liquidItem.light > 0f && this.blockLight == null) {
        this.UpdateLighting(liquidItem);
    } else {
        this.UpdateLighting(item);
    }
    this.UpdateSignage(item);
}