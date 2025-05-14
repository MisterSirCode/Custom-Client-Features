private void Update() {
    if (this.serverManager != null) {
        if (!this.serverManager.IsConnected() && this._state != global::GameManager.GameState.Preparing && this._state != global::GameManager.GameState.Transitioning && this._state != global::GameManager.GameState.Aborted) {
            this.AbortGame("Lost connection.");
        }
        if (global::GameManager.IsGame()) {
            this.commandManager.RunCommands();
        }
    }
    if (!global::GameManager.IsGame()) {
        ControllerToggler.enablegui = true;
        return;
    }
    ControllerToggler.enablegui = false;
}