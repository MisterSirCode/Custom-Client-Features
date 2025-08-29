private void LoadGraphics() {
    this._wholenessSprites = SpriteHelper.ContinuitySpriteArray("borders", "earth-earth", null);
    this._shieldSprites = new List<tk2dSpriteDefinition> {
        Singleton<AtlasManager>.main.Sprite("shields/full-1", true),
        Singleton<AtlasManager>.main.Sprite("shields/full-2", true)
    };
    this.decayMasks.Clear();
    this.decaySingles.Clear();
    foreach (string str in new string[] { "c", "d", "e", "f", "g", "h" }) {
        Item.SpriteLayer spriteLayer = new Item.SpriteLayer();
        spriteLayer.masks = SpriteHelper.SequentialMaskInfoList(new List<object> { "masks/decay-" + str, 3 });
        this.decayMasks.Add(spriteLayer);
        Item.SpriteLayer spriteLayer2 = new Item.SpriteLayer();
        spriteLayer2.sprites = SpriteHelper.SequentialSpriteInfoList(new List<object> { "mask_borders/decay-" + str, 3 }, 0f, false);
        this.decaySingles.Add(spriteLayer2);
    }
    foreach (KeyValuePair<string, object> keyValuePair in this.data.GetDictionary("decay").GetDictionary("materials")) {
        Item.SpriteLayer spriteLayer3 = new Item.SpriteLayer();
        spriteLayer3.sprites = new List<Item.SpriteInfo>();
        spriteLayer3.collection = Singleton<AtlasManager>.main.Collection("accents");
        foreach (object obj in ((List<object>)keyValuePair.Value)) {
            List<Item.SpriteInfo> collection = SpriteHelper.SequentialSpriteInfoList((List<object>)obj, 0f, false);
            spriteLayer3.sprites.AddRange(collection);
        }
        this.decayAccents[keyValuePair.Key] = spriteLayer3;
    }
}
