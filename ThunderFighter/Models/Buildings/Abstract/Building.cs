namespace ThunderFighter.Models.Buildings.Abstract
{
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Common.Abstract;

    public abstract class Building : Entity, IMovable
    {
        private static decimal deltaX = -0.5M;
        private decimal positionX;

        protected Building(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) :
            base(field, position, bodyStates, entityState)
        {
            this.positionX = position.X;
        }

        public static decimal DeltaX
        {
            get
            {
                return Building.deltaX;
            }

            private set
            {
                Building.deltaX = value;
            }
        }

        public void Move()
        {
            this.positionX += Building.DeltaX;
            this.Position.X = (int)this.positionX;
        }

        public abstract int PointsGainOnDestroy();
    }
}
