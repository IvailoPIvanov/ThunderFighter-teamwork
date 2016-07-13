namespace ThunderFighter.Models.Missiles
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Missiles.Abstract;

    internal class SidewinderMissile : Missile
    {
        public SidewinderMissile(Field field, Point2D position) : 
            this(field, position, EntityStateType.Strong)
        {
        }

        public SidewinderMissile(Field field, Point2D position, EntityStateType entityState) : 
            this(field, position, SidewinderMissile.BodyStates(), entityState)
        {
        }

        public SidewinderMissile(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) : 
            base(field, position, bodyStates, entityState)
        {
            // defines missile movement direction
            this.DeltaX = 3;
            this.DeltaY = 0;
        }

        public override void Move()
        {
            // change this formula if you want to change how missile moves
            this.Position.X += this.DeltaX;
            this.Position.Y += this.DeltaY;
        }

        private static IList<IList<Pixel>> BodyStates()
        {
            IList<IList<Pixel>> bodyStates = new List<IList<Pixel>>();

            IList<Pixel> strongBody = new List<Pixel>();
            strongBody.Add(new Pixel(4, -2, '*', ConsoleColor.Black));

            IList<Pixel> halfDestroyedBody = new List<Pixel>();
            halfDestroyedBody.Add(new Pixel(2, 2, '*', ConsoleColor.Red));

            IList<Pixel> destroyedBody = new List<Pixel>();
            destroyedBody.Add(new Pixel(3, 4, '+', ConsoleColor.Red));

            IList<Pixel> disappearedBody = new List<Pixel>();
            disappearedBody.Add(new Pixel(0, 0, ' ', Console.BackgroundColor));

            bodyStates.Add(strongBody);        // EntityState.Strong
            bodyStates.Add(halfDestroyedBody); // EntityState.HalfDestroyed
            bodyStates.Add(destroyedBody);     // EntityState.Destroyed
            bodyStates.Add(disappearedBody);   // EntityState.Disappeared

            return bodyStates;
        }
    }
}
