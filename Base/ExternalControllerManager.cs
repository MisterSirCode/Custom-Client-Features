using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ExternalControllerManager
{
	public void Initialize()
	{
		if (ExternalControllerManager.instance != null) {
			return;
		}
		ExternalControllerManager.instance = this;
	}

	public static ExternalControllerManager GetInstance()
	{
		return ExternalControllerManager.instance;
	}

	public static ExternalControllerManager instance;
}