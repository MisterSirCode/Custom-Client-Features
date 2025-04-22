	public override void Run()
	{
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
            new ExternalConsole().Initialize();
			new ExternalAssetManager().Initialize();
			ExternalAssetManager eam = ExternalAssetManager.GetInstance();
			eam.CreateAsset("bgm-song.for.a.dying.world", "song.for.a.dying.world.wav", "music");
			eam.CreateAsset("bgm-ghost.hammer", "ghost.hammer.wav", "music");
			eam.CreateAsset("bgm-android.lullabye", "android.lullabye.wav", "music");
			eam.CreateAsset("bgm-hidden.away.part.1", "hidden.away.part.1.wav", "music");
			eam.CreateAsset("bgm-minds.and.matters", "minds.and.matters.wav", "music");
			eam.CreateAsset("bgm-deepworld.soundtrack.vol.1.teaser", "deepworld.soundtrack.vol.1.teaser.wav", "music");
			eam.CreateAsset("bgm-slightly.mad.science.penultimate.cut", "slightly.mad.science.penultimate.cut.wav", "music");
			eam.CreateAsset("bgm-deepworld.v2", "deepworld.v2.wav", "music");
			ExternalMusicLoader.instance = null;
			ExternalConsole.Log("Asset Count", eam.loaded.Count.ToString());
			Camera.current.gameObject.AddComponent<ExternalMusicLoader>();
		}
		catch (Exception ex) {
			Debug.Log("Configuration failed: " + ex.Message + " --- " + ex.StackTrace);
			string message = (!Debug.isDebugBuild) ? "Oops - a configuration error occurred." : ("Config error: " + ex.Message + " - " + ex.StackTrace);
			Singleton<GameManager>.main.AbortGame(message);
		}
	}