	private bool CanEvadePrecipitationDamage(Item.Damage type)
	{
		if (this.AccessoryWithUse(Item.Use.Hazmat) != null) return true;
		int num = this.AdjustedSkill("survival");
		if (type == Item.Damage.Acid) {
			return num >= 10;
		}
		return type == Item.Damage.Fire && num >= 15;
	}