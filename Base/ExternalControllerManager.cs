using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ExternalControllerManager : MonoBehaviour {
	public void Start() {
		if (ExternalControllerManager.instance != null) {
			return;
		}
		ExternalControllerManager.instance = this;
	}

	public static ExternalControllerManager GetInstance() {
		return ExternalControllerManager.instance;
	}

	public bool ControllerEnabled() {
		return Input.GetJoystickNames().Length != 0;
	}

	public float LeftStick_X() {
		float val = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public float LeftStick_Y() {
		float val = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public float RightStick_X() {
		float val = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public float RightStick_Y() {
		float val = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public float LeftTrigger() {
		float val = GamePad.GetState(PlayerIndex.One).Triggers.Left;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public float RightTrigger() {
		float val = GamePad.GetState(PlayerIndex.One).Triggers.Right;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public bool LeftDown() {
		return this.LeftTrigger() > 0.25f;
	}

	public bool LeftUp() {
		return this.LeftTrigger() <= 0.25f;
	}

	public bool RightDown() {
		return this.RightTrigger() > 0.25f;
	}

	public bool RightUp() {
		return this.RightTrigger() <= 0.25f;
	}

	public bool AnyGamepadButtons() {
		return this.LeftDown() || this.RightDown();
	}

	public bool AnyGamepadAxis() {
		return this.LeftDown() || this.RightDown() || this.RightStick_X() != 0 || this.RightStick_Y() != 0;
	}

	public void Update() {
		if (Input.anyKeyDown && !this.AnyGamepadAxis()) this.usingGamepad = false;
		if (this.AnyGamepadAxis()) this.usingGamepad = true;
		if (this.ControllerEnabled()) {
			if (this.RightDown()) {
				ExternalMouseOperations.MouseEvent(ExternalMouseOperations.MouseEventFlags.LeftDown);
				this.rightNeedsUp = true;
			}
			if (this.RightUp() && this.rightNeedsUp) ExternalMouseOperations.MouseEvent(ExternalMouseOperations.MouseEventFlags.LeftUp);
			if (this.LeftDown()) {
				ExternalMouseOperations.MouseEvent(ExternalMouseOperations.MouseEventFlags.RightDown);
				this.leftNeedsUp = true;
			}
			if (this.LeftUp() && this.leftNeedsUp) ExternalMouseOperations.MouseEvent(ExternalMouseOperations.MouseEventFlags.RightUp);
			ExternalConsole.Log("Left Stick", "X: " + this.LeftStick_X().ToString() + " Y: " + this.LeftStick_Y().ToString());
			ExternalConsole.Log("Right Stick", "X: " + this.RightStick_X().ToString() + " Y: " + this.RightStick_Y().ToString());
			ExternalConsole.Log("Triggers", "Left: " + this.LeftTrigger().ToString() + " Right: " + this.RightTrigger().ToString());
		}
	}

	public void StepPlayerControls() {
		if (this.deathPanel == null) {
			this.deathPanel = GameObject.Find("/Canvas/Screen Panel/Death Panel");
		}
		if (ReplaceableSingleton<Player>.main.IsAlive() && this.deathPanel.active) {
			this.deathPanel.SetActive(false);
		}
		if (!ReplaceableSingleton<Player>.main.IsAlive() && (Input.anyKeyDown || this.AnyGamepadButtons())) {
			ReplaceableSingleton<Player>.main.Respawn();
			this.deathPanel.SetActive(false);
		}
		if (GameManager.IsGame() && this.usingGamepad) {
			Player.main;
		}
	}

	public void Awake() {
		this.deadzone = 0.1f;
		this.usingGamepad = false;
		this.rightNeedsUp = false;
		this.leftNeedsUp = false;
		DontDestroyOnLoad(this.gameObject);
	}

	public static ExternalControllerManager instance;
	public GameObject deathPanel;
	public float deadzone;
	public bool usingGamepad;
	public bool rightNeedsUp;
	public bool leftNeedsUp;
}