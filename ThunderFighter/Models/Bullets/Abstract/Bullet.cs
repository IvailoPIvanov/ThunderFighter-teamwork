namespace ThunderFighter.Models.Bullets.Abstract
{
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Common.Abstract;

    public abstract class Bullet : Entity, IMovable
    {
        private int deltaX;
        private int deltaY;

        protected Bullet(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) : 
            base(field, position, bodyStates, entityState)
        {
        }

        public int DeltaX
        {
            get
            {
                return this.deltaX;
            }

            set
            {
                this.deltaX = value;
            }
        }

        public int DeltaY
        {
            get
            {
                return this.deltaY;
            }

            set
            {
                this.deltaY = value;
            }
        }

        public virtual void Move()
        {
            this.Position.X += this.DeltaX;
            this.Position.Y += this.DeltaY;
        }
    }
}
