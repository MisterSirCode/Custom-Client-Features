using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalMusicLoader : MonoBehaviour
{
	public void Start()
	{
		controller = new AudioSource();
	}

	public IEnumerator LoadAudioFile(string path)
	{
		WWW www = new WWW(path);
		yield return www;
		try {
			testClip = www.GetAudioClip(false, false, AudioType.WAV);
			yield break;
		} catch(Exception e) {
			ExternalConsole.Log("Loading Asset Failed", e.ToString()); 
		}
	}

	public void PlayTestClip()
	{
		controller.PlayOneShot(this.testClip);
	}

	public AudioSource controller;
	public AudioClip testClip;
}