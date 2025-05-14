public static bool IsSteamdeck() {
		return ExternalControllerManager.instance.ControllerEnabled() && (PlayerPrefs.HasKey("controllerEnabled") ? PlayerPrefs.GetInt("controllerEnabled") == 1 : true);
}