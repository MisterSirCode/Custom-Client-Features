private void Update() {
    if (this._state == Zone.State.Ready) {
        this.Ready();
    }
    if (this._state != Zone.State.Active) {
        return;
    }
    Player main = ReplaceableSingleton<Player>.main;
    if (main.isZoneTeleporting) {
        return;
    }
    this.CalculateScreenCoordinates();
    this.paused = Singleton<GameManager>.main.serverTrouble;
    HashSet<int> hashSet = new HashSet<int>();
    this.RequestScreenChunks(hashSet);
    if (this.ShouldWaitForChunks(hashSet)) {
        this.WaitForChunks();
    } else {
        if (this._surroundingsState != Zone.State.Active) {
            if (this._surroundingsState == Zone.State.Pending) {
                Messenger.Broadcast<object>("zonePopulated", this);
            }
            this._surroundingsState = Zone.State.Active;
        }
        ReplaceableSingleton<ZoneRenderer>.main.ShowSpinner(false);
        if (ReplaceableSingleton<Player>.main.isTeleporting) {
            ReplaceableSingleton<Player>.main.CompleteTeleportation();
        }
        this.RequestSurroundingChunks(hashSet);
        this.RequestChunksByVelocity(hashSet);
    }
    if (hashSet.Count > 0 && (double)Time.realtimeSinceStartup > (double)this.lastChunkRequestAt + 0.5) {
        this.ProcessChunkRequests(hashSet);
    }
    if (this.cleanedChunks.Count > 0 && Time.realtimeSinceStartup > this.lastChunksIgnoreAt + 0.666f) {
        this.IgnoreCleanedChunks();
    }
    if (this.paused) {
        ReplaceableSingleton<ZoneRenderer>.main.StepPaused();
    } else {
        main.Step();
        ExternalControllerManager.instance.StepPlayerControls();
        this.CalculateSurroundings();
        this.ProcessStatus();
        this.ProcessLocalEffects();
        this.ProcessWeather();
        this.CleanupChunks();
        ReplaceableSingleton<ZoneRenderer>.main.Step();
    }
}