public void LoadAtlases() {
    this._maskTexture = this.LoadAtlas("masks", true, false, "Graphics/WorldSpriteMaterial");
    this._maskOpaque = this.Sprite("masks/opaque", false).uvs;
    this.LoadAtlas("accents", true, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("effects", true, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("front-0", true, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("front-1", true, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("front-quality", true, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("front-whole", true, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("signs", true, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-temperate", false, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-temperate-background", false, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-arctic", false, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-arctic-background", false, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-desert", false, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-desert-background", false, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-hell", false, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-hell-background", false, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-brain", false, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-brain-background", false, false, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-deep", false, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("biome-space", false, true, "Graphics/WorldSpriteMaterial");
    if (this.loadClassic) {
        this.LoadAtlas("biome-classic", false, true, "Graphics/WorldSpriteMaterial");
    }
    this.LoadAtlas("liquid", true, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("back", true, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("base", true, true, "Graphics/WorldSpriteMaterial");
    this.LoadAtlas("entities", true, false, "Graphics/WorldEntityMaterial");
    this._characterTexture = this.LoadAtlas("characters-animated", true, false, "Graphics/WorldAvatarMaterial");
    this.LoadAtlas("entities-animated", true, false, "Graphics/WorldEntityMaterial");
    this._inventoryTexture = this.LoadAtlas("inventory", true, false, "Graphics/GuiSpriteMaterial");
    this.LoadAllAtlases();
    if (this.loadSpine) {
        base.gameObject.AddComponent(typeof(SpineManager));
    }
}