protected override void Awake() {
	if (ExternalConsole.GetInstance() == null) {
		new ExternalConsole().Initialize();
	}
	if (PlayerPrefs.HasKey("buildOverride")) BldoverConsoleCommand.buildOverride = PlayerPrefs.GetInt("buildOverride") == 1 ? true : false;
	if (ExternalControllerManager.GetInstance() == null) {
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<ExternalControllerManager>();
		global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
	}
	if (ExternalAssetManager.instance == null){
		new ExternalAssetManager().Initialize();
		ExternalAssetManager instance = ExternalAssetManager.GetInstance();
		instance.AutoCreateAssets();
		ExternalConsole.Log("Asset Count", instance.loaded.Count.ToString());
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<ExternalSpriteLoader>();
		gameObject.AddComponent<ExternalAtlasLoader>();
		global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
	}
	if (PlayerPrefs.GetInt("resetPrefs") == 1) {
		string @string = PlayerPrefs.GetString("grap_username");
		string string2 = PlayerPrefs.GetString("grap_authToken");
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetString("grap_username", @string);
		PlayerPrefs.SetString("grap_authToken", string2);
	}
	base.Awake();
	if (!this.destroyed) {
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this._serverTrouble = false;
		this.commandManager = new CommandManager();
		this._state = GameManager.GameState.Inactive;
		GameManager.defaultSkin = Resources.Load<GUISkin>((Screen.dpi < 200f) ? "Graphics/Default GUISkin" : "Graphics/Default Mobile GUISkin");
		GameManager.defaultHeaderSkin = Resources.Load<GUISkin>((Screen.dpi < 200f) ? "Graphics/Default Header GUISkin" : "Graphics/Default Mobile Header GUISkin");
		this.AddListeners();
		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		HOTween.Init(true, false, false);
	}
	Application.targetFrameRate = -1;
}