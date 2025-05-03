private void Start() {
    if (ExternalConsole.GetInstance() == null) {
        new ExternalConsole().Initialize();
    }
    if (ExternalControllerManager.GetInstance() == null) {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<ExternalControllerManager>();
        global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
    }
    if (GameManager.IsGame()) {
        this._skippedMainMenu = true;
        this.LoginAsCurrentUser();
    }
}