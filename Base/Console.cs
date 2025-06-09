using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bytebin {
	[AddComponentMenu("Bytebin/GUI/Console")]
	public class Console : ReplaceableSingleton<Console> {
		private void Start() {
			this.input.text = string.Empty;
			this.Activate(false);
			this.history = new List<string>();
			this.commandAliases = new Dictionary<string, string>();
			this.commandAliases["sp"] = "spawn";
		}

		public void PreviousCommand() {
			this.historyIndex--;
			if (this.historyIndex < 0) {
				this.historyIndex = 0;
			}
			this.SetHistory();
		}

		public void NextCommand() {
			this.historyIndex++;
			if (this.historyIndex > this.history.Count) {
				this.historyIndex = this.history.Count;
			}
			this.SetHistory();
		}

		private void SetHistory() {
			this.input.text = ((this.historyIndex >= this.history.Count) ? string.Empty : this.history[this.historyIndex]);
			this.input.MoveTextEnd(false);
		}

		public void ProcessCommand(string text) {
			string[] array;
			if (text.IndexOf(" ") >= 0) {
				array = text.Split(new char[] { ' ' });
			}
			else {
				(array = new string[1])[0] = text;
			}
			string[] array2 = array;
			string text2 = array2[0].Substring(1);
			if (text2.Length > 0) {
				if (text2 != null) {
					if (text2 == "t") {
						return;
					}
				}
				if (this.commandAliases.ContainsKey(text2)) {
					text2 = this.commandAliases[text2];
				}
				Type type = Type.GetType(text2.Capitalize() + "ConsoleCommand");
				if (type != null) {
					ConsoleCommand consoleCommand = (ConsoleCommand)Activator.CreateInstance(type);
					consoleCommand.arguments = array2.Subarray(1);
					if (!consoleCommand.RequiresAdmin() || ReplaceableSingleton<Player>.main.admin || Application.isEditor) {
						consoleCommand.Run();
					}
					else {
						Notification.Create("Invalid command /" + text2, 1);
					}
				}
				else if (((Dictionary<string, object>)Config.main.data["commands"]).ContainsKey(text2)) {
					Command.Send(Command.Identity.Console, new object[] {
						text2,
						array2.Subarray(1)
					});
				}
				else if (ReplaceableSingleton<Player>.main.admin && text2 == "sale") {
					Command.Send(Command.Identity.Admin, new object[] {
						text2,
						array2.Subarray(1)
					});
				}
				else {
					Notification.Create("Invalid command /" + text2, 1);
				}
			}
		}

		public void Activate(bool isActive) {
			this.Activate(isActive, string.Empty);
		}

		public void Activate(bool isActive, string value) {
			if (isActive != this.IsActive()) {
				Transform transform = base.transform.Find("Console Input Caret");
				if (transform != null) {
					transform.localScale = ((!isActive) ? Vector3.zero : Vector3.one);
				}
				if (isActive) {
					this.input.enabled = true;
					EventSystem.current.SetSelectedGameObject(this.input.gameObject, null);
					this.input.OnPointerClick(new PointerEventData(EventSystem.current));
					this.input.text = string.Empty;
					this.historyIndex = this.history.Count;
					base.StartCoroutine(this.Append(value));
				}
				else {
					this.input.enabled = false;
					Console.deactivatedAt = Time.time;
				}
				foreach (MonoBehaviour monoBehaviour in this.enableWhileActive) {
					monoBehaviour.enabled = isActive;
				}
				Messenger.Broadcast<bool>("console.activated", isActive);
			}
		}

		private IEnumerator Append(string val) {
			yield return null;
			this.input.text = val;
			this.input.MoveTextEnd(false);
			yield break;
		}

		public bool IsActive() {
			return this.input.enabled;
		}

		public void OnSubmit(string val) {
			if (!Input.GetKey(KeyCode.Return) && !GameManager.IsMobile()) {
				return;
			}
			val = val.Replace("\r\n", string.Empty).Trim();
			if (val.Length > 0) {
				this.history.Add(val);
				if (this.history.Count > Console.maxHistory) {
					this.history.RemoveAt(0);
				}
				if (val[0] == '/') {
					this.ProcessCommand(val);
				}
				else {
					Command.Send(Command.Identity.Chat, new object[] { null, val });
				}
			}
			this.Activate(false);
		}

		public InputField input;
		public MonoBehaviour[] enableWhileActive;
		public static float deactivatedAt;
		private Dictionary<string, string> commandAliases;
		private List<string> history;
		private int historyIndex;
		private static int maxHistory = 500;
	}
}
