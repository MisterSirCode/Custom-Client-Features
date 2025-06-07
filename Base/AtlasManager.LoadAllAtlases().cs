public void LoadAllAtlases() {
    if (ExternalAtlasLoader.instance != null && ExternalAtlasLoader.instance.atlases.Count > 0) {
        foreach (ExternalAtlas externalAtlas in ExternalAtlasLoader.instance.atlases) {
            if (!externalAtlas.name.Contains("_entity")) {
                this.LoadExternalAtlas(externalAtlas, "Graphics/WorldSpriteMaterial");
            }
        }
    }
}