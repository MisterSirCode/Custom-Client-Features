	private void OnSettingChanged(string name, object val) {
		if (name != null) {
			if (!(name == "sfxVolume")) {
				if (name == "musicVolume") {
					this.SetVolume("musicVolume", val);
					ExternalMusicLoader.instance.controller.volume = val;
				}
			}
			else {
				this.SetVolume("sfxVolume", val);
				this.SetVolume("guiVolume", val);
			}
		}
	}