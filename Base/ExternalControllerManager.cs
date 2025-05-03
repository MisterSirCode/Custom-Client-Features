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
		this.deadzone = 0.1f;
		this.usingGamepad = false;
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

	public bool AnyGamepadInput() {
		return this.LeftDown() || this.RightDown() || this.RightStick_X() > 0 || this.RightStick_Y() > 0;
	}

	public void Update() {
		bool debug = true;
		if (Input.anyKeyDown) this.usingGamepad = false;
		if (this.AnyGamepadInput()) this.usingGamepad = true;
		if (this.RightDown()) ExternalMouseOperations.MouseEvent(ExternalMouseOperations.MouseEventFlags.LeftDown);
		if (this.RightUp()) ExternalMouseOperations.MouseEvent(ExternalMouseOperations.MouseEventFlags.LeftUp);
		ExternalConsole.Log("Gamepad Status", this.usingGamepad.ToString());
		if (this.ControllerEnabled() && debug) {
			ExternalConsole.Log("Left Stick", "X: " + this.LeftStick_X().ToString() + " Y: " + this.LeftStick_Y().ToString());
			ExternalConsole.Log("Right Stick", "X: " + this.RightStick_X().ToString() + " Y: " + this.RightStick_Y().ToString());
			ExternalConsole.Log("Triggers", "Left: " + this.LeftTrigger().ToString() + " Right: " + this.RightTrigger().ToString());
		}
	}

	public void StepPlayerControls() {
		if (!ReplaceableSingleton<Player>.main.IsAlive() && (Input.anyKeyDown || this.AnyGamepadInput()))
			ReplaceableSingleton<Player>.main.Respawn();
		if (GameManager.IsGame() && this.usingGamepad) {
			Player.main;
		}
	}

	public static ExternalControllerManager instance;
	public float deadzone;
	public bool usingGamepad;
}