	protected override void Awake() {
		if (ExternalConsole.GetInstance() == null) {
			new ExternalConsole().Initialize();
			GameObject controllerHost = new GameObject();
			controllerHost.AddComponent<ExternalControllerManager>();
			UnityEngine.GameObject.Instantiate(controllerHost);
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