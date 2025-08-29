public void Mod(Item item, int mod, float zIndex)
{
    Item.Mod mod2 = item.mod;
    if (mod2 != Item.Mod.Height)
    {
        if (mod2 == Item.Mod.Decay)
        {
            if (mod == 1)
            {
                Item.SpriteLayer spriteLayer = Config.main.decayAccents.Get(item.material);
                if (spriteLayer != null && spriteLayer.sprites.Count > 0)
                {
                    Item.SpriteInfo spriteInfo = spriteLayer.sprites[this.RandomSpriteOption(spriteLayer.sprites.Count, this.block.x, this.block.y)];
                    this.NextSprite().PlacePositioned(spriteInfo.sprite, Item.Alignment.None, 1f, 0f, false, zIndex - 0.1f, Color.white);
                    return;
                }
            }
            // Added section: place severe decay accent sprite if decay mod is 2 or greater
            else if (mod > 1)
            {
                Item.SpriteLayer spriteLayer2 = Config.main.decaySingles[this.RandomSpriteOption(Config.main.decaySingles.Count, this.block.x, this.block.y)];
                if (spriteLayer2 != null && spriteLayer2.sprites.Count > 0)
                {
                    Item.SpriteInfo spriteInfo2 = spriteLayer2.sprites[Math.Min(mod - 2, spriteLayer2.sprites.Count)];
                    this.NextSprite().PlacePositioned(spriteInfo2.sprite, Item.Alignment.None, 1f, 0f, false, zIndex - 0.1f, new Color(0.75f, 0.75f, 0.75f, 0.75f));
                    return;
                }
            }
        }
    }
    else if (mod < item.modMax)
    {
        Rect clipRect = this.sprite.ClipRect;
        clipRect.height *= (float)mod / (float)item.modMax;
        this.sprite.ClipRect = clipRect;
        this._transform.position += new Vector3(0f, (float)(item.modMax - mod) / (float)item.modMax * -0.5f);
    }
}
