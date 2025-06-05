public void Configure(Dictionary<string, object> _config)  {
	this.config = _config;
	string @string = this.config.GetString("activity", (!this.config.GetBool("pvp", false)) ? null : "PvP");
	this.biomeIcon.sprite = GameGui.GetSprite("biome-" + this.config.GetString("biome"));		
	if (this.config.GetString("biome") == "ocean") {
		Sprite sprite = ExternalSpriteLoader.GetSprite("biome-ocean");
		this.biomeIcon.sprite = sprite;
	}
	if (this.config.GetString("activity") == "market") {
		Sprite sprite2 = ExternalSpriteLoader.GetSprite("market-world");
		this.biomeIcon.sprite = sprite2;
	}
	this.primaryLabel.text = this.config.GetString("name") + ((@string == null) ? string.Empty : string.Concat(new string[] {
		" <color=#",
		GameGui.AltTitleColorHex,
		">",
		@string.Capitalize(),
		"</color>"
	}));
	if (this.config.GetString("gen_date") != null) {
		DateTime dateTime = DateTime.Parse(this.config.GetString("gen_date"));
		this.dateLabel.text = dateTime.ToString("MMM").ToUpper() + "\n" + dateTime.ToString("yy");
	} else {
		this.dateLabel.text = "?";
	}
	this.lockedIcon.enabled = this.config.GetBool("protected", false);
	this.bonusIcon.enabled = !this.lockedIcon.enabled && this.config.GetBool("premium", false);
	if (this.config.GetString("name") != ReplaceableSingleton<Zone>.main.name) {
		int @int = this.config.GetInt("players", 0);
		this.secondaryLabel.text = ((@int <= 0) ? "No occupants" : (@int + " occupant" + ((@int <= 1) ? string.Empty : "s")));
	} else {
		this.secondaryLabel.text = "You are here.";
	}
	this.chartOne.fillAmount = this.config.GetFloat("explored", 0f);
}