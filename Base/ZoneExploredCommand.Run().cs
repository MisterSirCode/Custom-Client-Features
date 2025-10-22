public override void Run() {
    foreach (object[] command in this.data) {
        Zone zone = ReplaceableSingleton<Zone>.main;
        int index = Convert.ToInt32(command[0]);
        zone.ExploreChunk(index);
        if (command.Length > 1) {
            zone.chunksExploredPercent = (float)command[1];
        } else {
            zone.chunksExploredPercent = zone.ExploredPercent();
        }
    }
}
