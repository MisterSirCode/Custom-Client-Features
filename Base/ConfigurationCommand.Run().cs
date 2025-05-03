	public override void Run() {
		try {
			int entityId = Convert.ToInt32(this.data[0]);
			Dictionary<string, object> config = (Dictionary<string, object>)this.data[1];
			Dictionary<string, object> configData = (Dictionary<string, object>)this.data[2];
			Dictionary<string, object> config2 = (Dictionary<string, object>)this.data[3];
			ReplaceableSingleton<Player>.main.Preconfigure(entityId, config);
			Singleton<GameManager>.main.Configure(configData);
			ReplaceableSingleton<Zone>.main.Configure(config2);
			ReplaceableSingleton<Player>.main.Configure(config);
			Singleton<ServerManager>.main.OnConfigurationEnded();
			Messenger.Broadcast<bool>("configCompleted", ConfigurationCommand.initial);
			ConfigurationCommand.initial = false;
			// Setup
			if (ExternalAssetManager.instance == null) {
				ExternalAssetManager eam = new ExternalAssetManager().Initialize();
				eam.AutoCreateAssets();
				string[] joystickNames = Input.GetJoystickNames();
				for (int i = 0; i < joystickNames.Length; i++) {
					ExternalConsole.Log(joystickNames[i], "");
				}
				ExternalConsole.Log("Asset Count", eam.loaded.Count.ToString());
				Camera.current.gameObject.AddComponent<ExternalMusicLoader>();
				Camera.current.gameObject.AddComponent<ExternalSpriteLoader>();
				ExternalConsole.Button("Play Music", delegate {
					ExternalMusicLoader.instance.PlayNextInQueue();
				});
			}
		}
		catch (Exception ex) {
			Debug.Log("Configuration failed: " + ex.Message + " --- " + ex.StackTrace);
			string message = (!Debug.isDebugBuild) ? "Oops - a configuration error occurred." : ("Config error: " + ex.Message + " - " + ex.StackTrace);
			Singleton<GameManager>.main.AbortGame(message);
		}
	}