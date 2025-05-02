using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ExternalControllerManager : MonoBehaviour
{
	public void Start()
	{
		if (ExternalControllerManager.instance != null) {
			return;
		}
		ExternalControllerManager.instance = this;
		this.deadzone = 0.07f;
	}

	public void Update()
	{
		if (this.ControllerEnabled()) {
			ExternalConsole.Log("Left Stick", "X: " + this.LeftStick_X() + " Y: " + this.LeftStick_Y());
		}
	}

	public static ExternalControllerManager GetInstance()
	{
		return ExternalControllerManager.instance;
	}

	public bool ControllerEnabled()
	{
		return Input.GetJoystickNames().Length != 0;
	}

	public float LeftStick_X()
	{
		float val = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public float LeftStick_Y()
	{
		float val = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public float RightStick_X()
	{
		float val = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public float RightStick_Y()
	{
		float val = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
		return Mathf.Abs(val) >= this.deadzone ? val : 0;
	}

	public static ExternalControllerManager instance;
	public float deadzone;
}