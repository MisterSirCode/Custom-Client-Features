using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExternalConsole {
	public ExternalConsole() {
	}

	public void Initialize() {
		if (ExternalConsole.instance != null) {
			return;
		}
		ExternalConsole.instance = this;
		this.modules = new List<ExternalConsoleModule>();
        this.enabled = false;
		this.invCatOverride = false;
	}

	public static ExternalConsole GetInstance() {
		return ExternalConsole.instance;
	}

	public void Draw() {
		if (GUI.Button(this.visible ? new Rect(5f, 200f, 120f, 30f) : new Rect(5f, 200f, 30f, 30f), new GUIContent(this.visible ? "<color=#afa>Debug Console</color>" : "<color=#faa>x</color>"))) {
			this.visible = !this.visible;
		}
		if (this.visible) {
			int rows = 0;
			foreach(ExternalConsoleModule module in this.modules) {
				int width = 50 + (int)Mathf.Clamp((float)(module.name.Length + " - locked: ".Length + module.output.Length) * 8f, 200f, 1500f);
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

	public static ExternalConsoleModule GetModule(string name) {
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

	public static void AddModule(ExternalConsoleModule module) {
		if (ExternalConsole.instance == null) {
			return;
		}
		ExternalConsole.instance.modules.Add(module);
	}

	public static void Log(string name, object output) {
		ExternalConsoleModule module = ExternalConsole.GetModule(name);
		if (module != null) {
			if (module.state) module.output = output.ToString();
			return;
		}
		ExternalConsoleModule newModule = new ExternalConsoleModule(name, "value", true);
		newModule.output = output.ToString();
		ExternalConsole.AddModule(newModule);
	}

	public static void Log(string name, string output) {
		ExternalConsole.Log(name, (object)output);
	}

	public static void HandleException(string name, Exception err) {
		ExternalConsole.Log(name + " Message", err.Message);
		ExternalConsole.Log(name + " Stack", err.StackTrace);
		ExternalConsole.Log(name + " Source", err.Source);
	}

	public static void Button(string name, Action action) {
		ExternalConsoleModule module = ExternalConsole.GetModule(name);
		if (module != null) {
			module.action = action;
			return;
		}
		ExternalConsoleModule newModule = new ExternalConsoleModule(name, action);
		ExternalConsole.AddModule(newModule);
	}

	public static void CopyButton(string name, Func<object> function) {
		ExternalConsole.Button("Copy " + name, () => {
			GUIUtility.systemCopyBuffer = function().ToString();
		});
	}

	public static void CopyButton(string name, object obj) {
		ExternalConsole.Button("Copy " + name, () => {
			GUIUtility.systemCopyBuffer = obj.ToString();
		});
	}

	public static string Concat(Dictionary<string, object> dict) {
		return String.Join(", ", dict.Keys.ToList<string>().ToArray());
	}

	public static string Concat(List<string> list) {
		return String.Join(", ", list.ToArray());
	}

	public static string Concat(string[] array) {
		return String.Join(", ", array);
	}

	public bool visible;
    public bool enabled;
	public bool invCatOverride;
	private static ExternalConsole instance;
	public List<ExternalConsoleModule> modules;
}
