public void LoadAllAtlases() {
    if (ExternalAtlasLoader.instance != null && ExternalAtlasLoader.instance.atlases.Count > 0) {
        foreach (ExternalAtlas externalAtlas in ExternalAtlasLoader.instance.atlases) {
            this.LoadExternalAtlas(externalAtlas, "Graphics/WorldSpriteMaterial");
        }
    }
}