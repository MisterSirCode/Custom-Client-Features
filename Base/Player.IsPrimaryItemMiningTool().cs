public bool IsPrimaryItemMiningTool()
{
    return this._primaryItem != null && this._primaryItem.IsMiningTool();
}