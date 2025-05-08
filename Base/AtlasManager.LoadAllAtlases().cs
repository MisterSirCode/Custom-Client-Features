public void LoadAllAtlases() {
    if (ExternalAtlasLoader.instance != null && ExternalAtlasLoader.instance.atlases.Count > 0) {
        foreach(ExternalAtlas atlas in ExternalAtlasLoader.instance.atlases) {
            this.LoadExternalAtlas(atlas, true, true);
        }
    }
}