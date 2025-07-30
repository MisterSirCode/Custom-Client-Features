public class EntitySlotRelativePositionSender : EntityTargeter, Sender {
    public EntitySlotRelativePositionSender(int entityId)
        : base(entityId) {
        this.slotRelativePosition = new Vector2(0.5f, 0);
    }

    public EntitySlotRelativePositionSender(Entity entity, Vector2 slotRelativePosition, string slot = null)
        : base(entity, slot) {
        this.slotRelativePosition = slotRelativePosition;
    }

    public Vector2 Origin(Dictionary<string, object> details) {
        if (!this.entity.IsVisible()) {
            if (this.entity.isBlock) {
                return this.entity.Center();
            }
            return this.entity.cTransform.position;
        } else {
            if (this.slot == null) {
                object[] array = (object[])details.Get("sl");
                if (array != null) {
                    int num = Convert.ToInt32(array[0]);
                    this.slot = ((EntityAnimated)this.entity).GetConfigSlot(num);
                }
            }
            if (this.entity is EntityAnimated && this.slot != null) {
                return ((EntityAnimated)this.entity).GetPointOnSlotAtRelativePosition(this.slot, this.slotRelativePosition);
            }
            return this.entity.Center();
        }
    }

    public override bool Valid() {
        return this.entity != null;
    }

    private Vector2 slotRelativePosition;
}
