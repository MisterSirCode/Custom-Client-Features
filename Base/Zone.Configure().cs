public void Configure(Dictionary<string, object> config) {
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    this.documentId = config.GetString("id");
    this.name = config.GetString("name");
    this._description = config.GetString("description", "Little is known about this world...");
    this.recycle = !(bool)config.Get("static", false);
    this.seed = int.Parse(this.documentId.Substring(2, 6), NumberStyles.HexNumber);
    this.biome = config.GetString("biome");
    Config.main.LoadBiome(this.biome);
    if (this.biome != "ocean") {
        Singleton<AtlasManager>.main.LoadTexture("biome-" + ((!(this.biome == "plain")) ? this.biome : "temperate"));
        if (this.biome != "deep" && this.biome != "space") {
            Singleton<AtlasManager>.main.LoadTexture("biome-" + ((!(this.biome == "plain")) ? this.biome : "temperate") + "-background");
        }
    }
    Physics.gravity = new Vector3(0f, -9.81f * ((!(this.biome == "space")) ? 1f : 0.7f), 0f);
    this.isMember = config.GetBool("member", false);
    this.isPrivate = config.GetBool("private", false);
    this.isProtected = config.GetBool("protected", false);
    this.isProtectedPlayer = ((!config.ContainsKey("protected_player")) ? config.GetBool("protected", false) : config.GetBool("protected_player", false));
    this.isProtectedReason = config.GetString("protected_reason");
    this.pvp = config.GetBool("pvp", false);
    Physics2D.IgnoreLayerCollision(Ecosystem.playerLayer, Ecosystem.peerProjectileLayer, !this.pvp);
    Physics2D.IgnoreLayerCollision(Ecosystem.peerLayer, Ecosystem.playerProjectileLayer, !this.pvp);
    Physics2D.IgnoreLayerCollision(Ecosystem.peerLayer, Ecosystem.peerProjectileLayer, !this.pvp);
    this.precipitationType = Config.main.biomeData.GetString("precipitation");
    this.blockSize.width = Convert.ToInt32(((List<object>)config.Get("size"))[0]);
    this.blockSize.height = Convert.ToInt32(((List<object>)config.Get("size"))[1]);
    this.blockCount = this.blockSize.width * this.blockSize.height;
    this.chunkDimensions.width = Convert.ToInt32(((List<object>)config.Get("chunk_size"))[0]);
    this.chunkDimensions.height = Convert.ToInt32(((List<object>)config.Get("chunk_size"))[1]);
    this.chunkSize.width = this.blockSize.width / this.chunkDimensions.width;
    this.chunkSize.height = this.blockSize.height / this.chunkDimensions.height;
    this.chunkCount = this.chunkSize.width * this.chunkSize.height;
    this.blockRect = new Rect(0f, 0f, (float)this.blockSize.width, (float)this.blockSize.height);
    this.blockRectBounds = new Rect(0f, 0f, (float)this.blockSize.width, (float)this.blockSize.height);
    this.sunlight = new int[this.blockSize.width];
    ReplaceableSingleton<ZoneSunlightRenderer>.main.Setup();
    this.chunksExplored = config.GetList("chunks_explored").ConvertAll<bool>((object x) => x != null && (bool)x);
    this.chunksExploredCount = this.chunksExplored.Count((bool x) => x);
    this.surface = Array.ConvertAll<object, int>(((List<object>)config.Get("surface")).ToArray(), new Converter<object, int>(Convert.ToInt32));
    Messenger.Broadcast<int, int[]>("zoneSurfaceChanged", this.blockSize.height, this.surface);
    this.bookmarked = Convert.ToInt32(config.Get("bookmarked")) == 1;
    this.meta = new Meta(this);
    stopwatch.Stop();
    Messenger.Broadcast<Dictionary<string, object>>("zoneConfigured", config);
}