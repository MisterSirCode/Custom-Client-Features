...

        this.scrubbable = base.ConfigBool("scrubbable", false);
        this.jiggle = base.ConfigFloat("jiggle", 0.0f);

        List<object> emitterPosition = (List<object>)this.data.Get("emitter_position");
        if (emitterPosition != null) {
            this.emitterPosition = new Vector3((float)emitterPosition[0], (float)emitterPosition[1]);
        } else {
            if ("gun" == base.ConfigString("action")?.ToLower()) {
                this.emitterPosition = new Vector3(0.5f, 0.0f);
            } else {
                this.emitterPosition = new Vector3(0.5f, -0.4f);
            }
        }

...