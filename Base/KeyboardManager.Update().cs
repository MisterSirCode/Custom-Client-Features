private void Update() {
	if (this.IsKeyDown("Dismiss") || Input.GetKeyDown(KeyCode.Escape)) {
		Messenger.Broadcast("guiDismissed");
	}
	if (ReplaceableSingleton<global::Bytebin.Console>.main.IsActive()) {
		if (this.IsKeyDown("Console Previous")) {
			ReplaceableSingleton<global::Bytebin.Console>.main.PreviousCommand();
		}
		if (this.IsKeyDown("Console Next")) {
			ReplaceableSingleton<global::Bytebin.Console>.main.NextCommand();
			return;
		}
	}
	else if (Zone.IsActive() && !ReplaceableSingleton<GameGui>.main.IsModal()) {
		if (!global::InControl.TouchManager.ControlsEnabled) {
			float axisRaw = Input.GetAxisRaw("Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Vertical");
			ReplaceableSingleton<Player>.main.InputMovement(axisRaw, axisRaw2);
		}
		float num = -Input.GetAxis("Zoom");
		if (Input.GetKey(KeyCode.LeftShift)) {
			num *= Time.deltaTime;
		}
		ReplaceableSingleton<CameraManager>.main.Zoom(num);
		if (this.IsKeyDown("Console") || Input.GetKeyDown(KeyCode.BackQuote)) {
			ReplaceableSingleton<global::Bytebin.Console>.main.Activate(true);
		}
		else if (this.IsKeyDown("Command")) {
			ReplaceableSingleton<global::Bytebin.Console>.main.Activate(true, "/");
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			ReplaceableSingleton<Player>.main.Respawn();
		}
		if (this.IsKeyDown("Jerky")) {
			Health.UseAsNecessary();
		}
		if (this.IsKeyDown("Flashlight")) {
			Messenger.Broadcast("onPlayerLightToggled");
		}
		if (this.IsKeyDown("Canister")) {
			Consumable.Get(Item.Get("consumables/canister")).Use(null);
		}
		if (this.IsKeyDown("Chat")) {
			Messenger.Broadcast("chatToggled");
		}
		if (this.IsKeyDown("Emotes")) {
			Messenger.Broadcast("emotesToggled");
		}
		if (this.IsKeyDown("Profile")) {
			Messenger.Broadcast("profileToggled");
		}
		if (this.IsKeyDown("Social Panel")) {
			Messenger.Broadcast("socialToggled");
		}
		if (this.IsKeyDown("Shop")) {
			Messenger.Broadcast("shopToggled");
		}
		if (this.IsKeyDown("Portal")) {
			Messenger.Broadcast("portalToggled");
		}
		if (this.IsKeyDown("Help")) {
			Messenger.Broadcast("helpToggled");
		}
		if (this.IsKeyDown("Inventory")) {
			Messenger.Broadcast("inventoryToggled");
		}
		if (this.IsKeyDown("Crafting Panel")) {
			Messenger.Broadcast("craftingToggled");
		}
		if (this.IsKeyDown("Map")) {
			Messenger.Broadcast("mapToggled");
		}
		if (this.IsKeyDown("Show Protector Overlay")) {
			Messenger.Broadcast("protectorOverlayToggled");
		}
		if (this.IsKeyDown("Shield")) {
			Messenger.Broadcast("shieldOn");
		}
		if (this.IsKeyUp("Shield")) {
			Messenger.Broadcast("shieldOff");
		}
		if (this.IsKeyDown("Stealth")) {
			Stealth.UseTheBest();
		}
		if (this.IsKeyDown("Teleport")) {
			Teleport.UseTheBest();
		}
		if (this.IsKeyDown("Hotbar1")) {
			this.SetHotbar(0);
		}
		else if (this.IsKeyDown("Hotbar2")) {
			this.SetHotbar(1);
		}
		else if (this.IsKeyDown("Hotbar3")) {
			this.SetHotbar(2);
		}
		else if (this.IsKeyDown("Hotbar4")) {
			this.SetHotbar(3);
		}
		else if (this.IsKeyDown("Hotbar5")) {
			this.SetHotbar(4);
		}
		else if (this.IsKeyDown("Hotbar6")) {
			this.SetHotbar(5);
		}
		else if (this.IsKeyDown("Hotbar7")) {
			this.SetHotbar(6);
		}
		else if (this.IsKeyDown("Hotbar8")) {
			this.SetHotbar(7);
		}
		else if (this.IsKeyDown("Hotbar9")) {
			this.SetHotbar(8);
		}
		else if (this.IsKeyDown("Hotbar10")) {
			this.SetHotbar(9);
		}
		if (ReplaceableSingleton<Player>.main.admin || Application.isEditor) {
			if (this.IsKeyDown("Debug Panel")) {
				Messenger.Broadcast("debugPanelToggled");
			}
			if (this.IsKeyDown("Block Info")) {
				Messenger.Broadcast<Vector2>("blockInfoRequest", ReplaceableSingleton<GameGui>.main.pointerBlockPosition);
			}
			if (this.IsKeyDown("Clip")) {
				ReplaceableSingleton<Player>.main.ToggleClipping();
			}
			if (this.IsKeyDown("Admin Spawn")) {
				new AdminSpawn();
			}
		}
	}
}
