private void OnPrimaryItemChanged(Item item)  {
    if (item != null && (item.placeable || item.consumable)) {
        this.primaryItemPlaceable = true;
        if (item.tileable || item.spriteLayers == null || item.spriteLayers.Count == 0) {
            tk2dSpriteCollectionData tk2dSpriteCollectionData = Singleton<AtlasManager>.main.Collection("inventory");
            if (tk2dSpriteCollectionData.GetSpriteDefinition(item.inventorySpriteName) != null) {
                this.placeSprite.SetSprite(tk2dSpriteCollectionData, item.inventorySpriteName);
            } else {
                this.placeSprite.SetSprite(tk2dSpriteCollectionData, "inventory/ground/onyx");
            }
            float num = Singleton<AtlasManager>.main.BaseBlockScale() / Mathf.Max(this.placeSprite.GetCurrentSpriteDef().rect.width, this.placeSprite.GetCurrentSpriteDef().rect.height);
            this.placeSprite.scale = Vector3.one * num;
            FixedSize fixedSize;
            fixedSize.width = 1;
            fixedSize.height = 1;
        } else {
            tk2dSpriteDefinition tk2dSpriteDefinition = item.guiSprite;
            if (tk2dSpriteDefinition == null) {
                Item.SpriteLayer spriteLayer = item.spriteLayers[0];
                tk2dSpriteDefinition = spriteLayer.sprites[0].sprite;
            }
            if (tk2dSpriteDefinition != null) {
                tk2dSpriteCollectionData collection = tk2dSpriteDefinition.collection;
                this.placeSprite.SetSprite(collection, tk2dSpriteDefinition.name);
                this.placeSprite.scale = Vector3.one;
                FixedSize fixedSize = SpriteHelper.BlockSize(tk2dSpriteDefinition);
            }
        }
    } else {
        this.primaryItemPlaceable = false;
    }
}
