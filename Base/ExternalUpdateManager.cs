using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using BestHTTP.JSON;
using UnityEngine;
using UnityEngine.UI;

public class ExternalUpdateManager : MonoBehaviour {
    public string UpdateUrl() {
        return "https://api.github.com/repos/MisterSirCode/Deepworld-Autoupdater/releases/latest";
    }

	public IEnumerator WaitForSomeCoroutines(IEnumerator[] ienumerators) {
        if (ienumerators != null & ienumerators.Length > 0) {
            Coroutine[] coroutines = new Coroutine[ienumerators.Length];
            for (int i = 0; i < ienumerators.Length; i++)
                coroutines[i] = StartCoroutine(ienumerators[i]);
            for (int i = 0; i < coroutines.Length; i++)
                yield return coroutines[i];
        } else yield return null;
    }

    public string GetFileNameFromUrl(string url) {
        Uri uri = new Uri(url);
        return uri.AbsolutePath;
    }

    public IEnumerator LoadJSONFile(string url, Action<object> callback) {
		WWW www = new WWW(url);
		yield return www;
		try {
			callback(Json.Decode(www.text));
			www.Dispose();
			yield break;
		} catch(Exception e) {
			ExternalConsole.HandleException("Loading URL Error", e); 
		}
	}

	public IEnumerator LoadMiscFile(string url, string path, string name, Action<string, byte[]> callback) {
		WWW www = new WWW(url);
		this.currentUpdateText = "<color=#fff>Downloading File: " + Regex.Replace(name, "\\w+/\\w+/", "") + "...</color>";
		yield return www;
		try {
			//yield return File.WriteAllBytes(path, www.bytes);
			callback(path, www.bytes);
			www.Dispose();
			yield break;
		} catch(Exception e) {
			ExternalConsole.Log("Loading File " + name + " Error", e); 
		}
	}
    
    public void Start() {
		if (ExternalUpdateManager.instance != null) {
			return;
		}
        ExternalUpdateManager.instance = this;
		this.labelStyle = new GUIStyle();
		this.labelStyle.alignment = TextAnchor.MiddleCenter;
		this.labelStyle.fontSize = 18;
		Transform gui = GameObject.Find("Canvas").transform.GetChild(2);
		this.dialog = gui.GetChild(5).gameObject;
		this.dialog.transform.SetParent(gui);
		this.homeDialog = this.dialog.GetComponent<HomeDialog>();
        StartCoroutine(this.LoadJSONFile(this.UpdateUrl(), (object item) => {
			this.jsonData = (Dictionary<string, object>)item;
			this.recordedVersion = jsonData.GetString("name");
			this.recordedTag = jsonData.GetString("tag_name");
			List<object> assets = this.jsonData.GetList("assets");
			bool isLargeUpdate = assets.Count == 0;
			string largeUpdateURL = this.jsonData.GetString("body");
			ExternalConsole.Log("Version Available", this.recordedVersion);
			if (GameManager.tag < int.Parse(this.recordedTag)) {
				ExternalUpdateManager.shouldShowPanel = true;
				this.manager = GameObject.FindObjectOfType<HomeManager>();
				this.manager.spinner.SetActive(false);
				this.homeDialog.Show("Message");
				this.homeDialog.backButton.SetActive(true);
				this.dialogItem = this.homeDialog.items.GetChild(4).gameObject.GetComponent<HomeDialogItemMessage>();
				if (isLargeUpdate) {
					this.homeDialog.submitLabel.text = "Download";
					this.homeDialog.backButton.GetComponent<Button>().onClick.AddListener(() => {
						if (ExternalUpdateManager.shouldShowPanel) {
							ExternalUpdateManager.shouldShowPanel = false;
						}
					});
					this.homeDialog.container.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => {
						if (ExternalUpdateManager.shouldShowPanel) {
							System.Diagnostics.Process.Start(new ProcessStartInfo {
								FileName = largeUpdateURL,
								UseShellExecute = true
							});
							ExternalUpdateManager.shouldShowPanel = false;
						}
					});
					this.dialogItem.messageLabel.text = "Please update to version " + this.recordedVersion + ".\n\n\nThis is a LARGE update. You must redownload the game.";
					this.dialogItem.messageLabel.alignment = TextAnchor.MiddleCenter;
					this.dialogItem.titleLabel.text = "Large Update Available";
				} else {
					this.homeDialog.submitLabel.text = "Update";
					this.homeDialog.backButton.GetComponent<Button>().onClick.AddListener(() => {
						if (ExternalUpdateManager.shouldShowPanel) {
							ExternalUpdateManager.shouldShowPanel = false;
						}
					});
					this.homeDialog.container.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => {
						if (ExternalUpdateManager.shouldShowPanel) {
							base.StartCoroutine(this.BeginUpdateCycle());
							ExternalUpdateManager.shouldShowPanel = false;
						}
					});
					this.dialogItem.messageLabel.text = "Please update to version " + this.recordedVersion + ".\n\n\nYou may need to upgrade if\n you would like to join.";
					this.dialogItem.messageLabel.alignment = TextAnchor.MiddleCenter;
					this.dialogItem.titleLabel.text = "Update Available";
					this.currentUpdateText = "<color=#fff>Loading Update...</color>";
				}
			}
		}));
	}

    public void OnGUI() {
		if (!GameManager.IsGame()) {
			if (this.manager.spinner.active && ExternalUpdateManager.isUpdating) {
				Vector2 vector = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
				ExternalConsole.Log("Position", vector);
				GUI.Label(new Rect(vector.x - 80f, vector.y + 60f, 160f, 30f), new GUIContent(this.currentUpdateText), this.labelStyle);
			}
		}
    }

	public void Update() {
	
	}

	public void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}

	public IEnumerator BeginUpdateCycle() {
		this.manager.spinner.SetActive(true);
		ExternalUpdateManager.isUpdating = true;
		List<IEnumerator> list = new List<IEnumerator>();
		List<object> assets = this.jsonData.GetList("assets");
		foreach (object item in assets) {
			Dictionary<string, object> dict = (Dictionary<string, object>)item;
			string name = dict.GetString("name");
			string adder = "";
			if (name == "Assembly-CSharp.dll") adder = "Managed/";
			else if (name.StartsWith("External.")) name = Regex.Replace(name, "External.(\\w+).", "External/$1/");
			list.Add(LoadMiscFile(dict.GetString("browser_download_url"), Application.dataPath + "/" + adder + name, name, (string name, byte[] data) => {
				ExternalConsole.Log("Loaded File " + name, data.Length);
			}));
		}
		yield return base.StartCoroutine(this.WaitForSomeCoroutines(list.ToArray()));
		this.currentUpdateText = "<color=#1f1>Restart your game to finalize changes!</color>";
		yield return new WaitForSeconds(5f);
		ExternalUpdateManager.isUpdating = false;
		this.manager.spinner.SetActive(false);
	}

    public static ExternalUpdateManager instance;
	public Dictionary<string, object> jsonData;
	public GameObject dialog;
	public HomeDialog homeDialog;
	public HomeDialogItemMessage dialogItem;
	public HomeManager manager;
	public GUIStyle labelStyle;
	public string recordedVersion;
	public string recordedTag;
	public static bool isUpdating;
	public static bool shouldShowPanel;
	public string currentUpdateText;
}