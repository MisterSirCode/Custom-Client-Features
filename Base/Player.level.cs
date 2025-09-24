public int level {
    get {
        return default(int);
    }
    set {
        this._level = value;
        this.xpForCurrentLevel = this.XpForLevel(this.level);
        int maxLevel = Config.main.data.GetInt("max_level", 100);
        this.xpForNextLevel = this.XpForLevel((this.level >= maxLevel) ? this.level : (this.level + 1));
        Messenger.Broadcast<int>("playerLevelChanged", this.level);
        Messenger.Broadcast<int, int>("playerXpChanged", this.xp, this.xpForNextLevel);
    }
}
