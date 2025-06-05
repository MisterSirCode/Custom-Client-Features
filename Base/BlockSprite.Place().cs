public void Place(Item item, Item.SpriteLayer spriteLayer, Item.SpriteInfo spriteInfo, int mod, float zIndex, Color color, BlockSprite blockSprite = null) {
    if (blockSprite == null) {
        blockSprite = this.NextSprite();
    }
    color.a = spriteLayer.opacity;
    if (item != null && (item.tileable || spriteLayer.tileable)) {
        FixedSize fixedSize = SpriteHelper.BlockSize(spriteInfo.sprite);
        if (spriteLayer.type == Item.SpriteLayerType.Modtile) {
            int num = mod % fixedSize.width;
            int num2 = mod / fixedSize.width;
            blockSprite.PlaceTileable(num, num2, spriteInfo.sprite, fixedSize, zIndex, color);
        } else {
            blockSprite.PlaceTileable(spriteInfo.sprite, fixedSize, zIndex, color);
        }
        blockSprite.Mod(item, mod, zIndex);
        blockSprite.Mask(item, mod, zIndex);
    } else {
        blockSprite.PlacePositioned(spriteInfo.sprite, spriteLayer.alignment, spriteLayer.scale, this.Angle(item, spriteInfo, mod), this.Flip(item, spriteInfo, mod), zIndex, color);
    }
    if (spriteLayer.type == Item.SpriteLayerType.Animated) {
        blockSprite.TryToAnimate(item, spriteLayer, mod, zIndex, color);
    }
    if (item.jiggle != 0.0f) {
        blockSprite.TryToAnimateJiggle(item);
    }
}
