using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009BC RID: 2492
public class ExternalConsole
{
	// Token: 0x060049E4 RID: 18916
	public ExternalConsole()
	{
	}

	public void Initialize()
	{
		if (ReplaceableSingleton<Player>.main == null) {
			return;
		}
		if (ExternalConsole.instance != null) {
			return;
		}
		ExternalConsole.instance = this;
		this.modules = new List<ExternalConsoleModule>();
        this.enabled = true;
	}

	public static ExternalConsole GetInstance()
	{
		return ExternalConsole.instance;
	}

	public void Draw()
	{
		if (GUI.Button(this.visible ? new Rect(5f, 200f, 120f, 30f) : new Rect(5f, 200f, 30f, 30f), new GUIContent(this.visible ? "<color=#afa>Debug Console</color>" : "<color=#faa>x</color>"))) {
			this.visible = !this.visible;
		}
		if (this.visible) {
			int rows = 0;
			foreach(ExternalConsoleModule module in this.modules) {
				int width = 50 + (int)Mathf.Clamp((float)(module.name.Length + " - locked: ".Length + module.output.Length) * 8f, 200f, 1000f);
				try {
					if (module.type == "value") {
						if (GUI.Button(new Rect(5f, 200f + (float)((rows + 1) * 35), width, 30f), new GUIContent(module.name + (module.state ? " - <color=#afa>Active</color>: " : " - <color=#faa>Locked</color>: ") + module.output))) {
							module.Toggle();
						}
					} else if (module.type == "bool") {
						if (GUI.Button(new Rect(5f, 200f + (float)((rows + 1) * 35), width, 30f), new GUIContent(module.name + (module.state ? " - <color=#afa>Active</color>: " : " - <color=#faa>Locked</color>: ") + module.output))) {
							module.Toggle();
						}
					} else if (module.type == "button") {
						if (GUI.Button(new Rect(5f, 200f + (float)((rows + 1) * 35), 200f, 30f), new GUIContent(module.name))) {
							module.action();
						}
					} else {
						GUI.Button(new Rect(5f, 200f + (float)((rows + 1) * 35), 200f, 30f), new GUIContent(module.name + ": " + module.output));
					}
				} catch(Exception e) {
					GUI.Button(new Rect(5f, 200f + (float)((rows + 1) * 35), 200f, 30f), new GUIContent("Failed to draw log with error " + e.ToString()));
				}
				rows++;
			}
		}
	}

	public static ExternalConsoleModule GetModule(string name) 
	{
		if (ExternalConsole.instance == null) {
			return null;
		}
		foreach (ExternalConsoleModule module in ExternalConsole.instance.modules) {
			if (module.name.Equals(name)) {
				return module;
			}
		}
		return null;
	}

	public void Log(string name, string output) 
	{
		ExternalConsoleModule module = ExternalConsole.GetModule(name);
		if (module != null) {
			if (module.state) module.output = output;
			return;
		}
		ExternalConsoleModule newModule = new ExternalConsoleModule(name, "value", false);
		newModule.output = output;
		this.modules.Add(newModule);
	}

	public void Button(string name, Action action)
	{
		ExternalConsoleModule module = ExternalConsole.GetModule(name);
		if (module != null) {
			module.action = action;
			return;
		}
		ExternalConsoleModule newModule = new ExternalConsoleModule(name, action);
		this.modules.Add(newModule);
	}

	public bool visible;
    public bool enabled;
	private static ExternalConsole instance;
	public List<ExternalConsoleModule> modules;
}
