...

		this.scrubbable = base.ConfigBool("scrubbable", false);
		this.jiggle = base.ConfigFloat("jiggle", 0.0f);
		this.firingInterval = this.data.ContainsKey("firing_interval") ? (float)this.data.Get("firing_interval") : -1f;

...