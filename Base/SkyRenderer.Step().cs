	private void Step() {		
        if (ReplaceableSingleton<ZoneRenderer>.main.IsCavernVisible()) {
            float t = ReplaceableSingleton<Player>.main.Depth();
            this.zenithColor = Color.Lerp(this.cavernShallowColors[0], this.cavernShallowColors[1], t);
            this.horizonColor = Color.Lerp(this.cavernDeepColors[0], this.cavernDeepColors[1], t);
        } else {			
            List<List<Color>> list = this.SkyGradients(null);
			List<List<Color>> list2 = this.SkyGradients("acidic");
			Color[] array = this.ColorForTimeOfDay(list);
			Color[] array2 = this.ColorForTimeOfDay(list2);
			if (list2.Count == 0) {				
                this.horizonColor = array[0];
				this.zenithColor = array[1];
			} else {				
                Color color = array[0];
				Color color2 = array[1];
				Color color3 = array2[0];
				Color color4 = array2[1];
				this.horizonColor = Color.Lerp(color, color3, ReplaceableSingleton<Zone>.main.acidity);
				this.zenithColor = Color.Lerp(color2, color4, ReplaceableSingleton<Zone>.main.acidity);
			}
			this.horizonColor = Color.Lerp(this.horizonColor, this.precipitationColor, ReplaceableSingleton<Zone>.main.precipitation * 0.5f);
			this.zenithColor = Color.Lerp(this.zenithColor, this.precipitationColor, ReplaceableSingleton<Zone>.main.precipitation * 0.75f);
		}
		if (SkyboxConsoleCommand.activeWithColor) {
			this.horizonColor = SkyboxConsoleCommand.customColor;
			this.zenithColor = SkyboxConsoleCommand.customColor;
		}
		this.cMaterial.SetColor("_Color", this.horizonColor);
		this.cMaterial.SetColor("_Color2", this.zenithColor);
		Messenger.Broadcast<Dictionary<string, Color>>("skyColorsChanged", this.ColorsDictionary());
		if (this.supernovaStart > 0f) {			
            if (Time.time > this.supernovaEnd) {				
                this.supernovaStart = (this.supernovaEnd = 0f);
			} else {				
                float num2 = this.supernovaEnd - this.supernovaStart;
				float num3 = (Time.time - this.supernovaStart) / num2;
				Color color5 = Color.black;
				Color color6 = Color.black;
				float num4;
				if (num3 < 0.25f) {					
                    num4 = (0.25f - num3) * 4f;
				} else if (num3 < 0.5f) {					
                    color5 = new Color(0.19607843f, 0f, 0f, 1f);
					color6 = Color.black;
					num4 = (0.5f - num3) * 4f;
				} else if (num3 < 0.9f) {					
                    color5 = new Color(0.7058824f, 0.15686275f, 0f, 1f);
					color6 = new Color(0.19607843f, 0f, 0f, 1f);
					num4 = (0.9f - num3) / 0.4f;
				} else {					color5 = Color.white;
					color6 = Color.white;
					num4 = (1f - num3) * 10f;
				}
				Color color7 = Color.Lerp(color5, color6, num4);
				this.supernovaRenderer.material.color = Color.Lerp(Color.black, color7, global::UnityEngine.Random.Range(0.9f, 0.9f + num3 * 0.1f));
			}
		} else {
            this.supernovaRenderer.material.color = Color.black;
		}
	}