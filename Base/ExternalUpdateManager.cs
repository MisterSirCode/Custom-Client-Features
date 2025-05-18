using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.JSON;
using UnityEngine;

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

	public IEnumerator LoadMiscFile(string url, Action<string, byte[]> callback) {
		WWW www = new WWW(url);
		yield return www;
		string name = this.GetFileNameFromUrl(url);
		try {
			callback(name, www.bytes);
			www.Dispose();
			yield break;
		} catch(Exception e) {
			ExternalConsole.Log("Loading File " + name + " Error", e); 
		}
	}
    
    public void Start() {
        
	}

	public void Update() {
	
	}

	public void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
}