namespace ThunderFighter.Models.Bullets
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Models.Bullets.Abstract;
    using ThunderFighter.Models.Common;

    internal class LightweightBullet : Bullet
    {
        public LightweightBullet(Field field, Point2D position) : 
            this(field, position, EntityStateType.Strong)
        {
        }

        public LightweightBullet(Field field, Point2D position, EntityStateType entityState) : 
            this(field, position, LightweightBullet.BodyStates(), entityState)
        {
        }

        public LightweightBullet(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) : 
            base(field, position, bodyStates, entityState)
        {
            // defines bullet movement direction
            this.DeltaX = 3;
            this.DeltaY = 0;
        }

        private static IList<IList<Pixel>> BodyStates()
        {
            IList<IList<Pixel>> bodyStates = new List<IList<Pixel>>();

            IList<Pixel> strongBody = new List<Pixel>();
            strongBody.Add(new Pixel(7, 3, '-', Theme.Contrast));

            List<Pixel> halfDestroyedBody = new List<Pixel>();
            halfDestroyedBody.Add(new Pixel(-1, 2, '*', ConsoleColor.Red));
            halfDestroyedBody.Add(new Pixel(0, 1, '*', ConsoleColor.Red));
            halfDestroyedBody.Add(new Pixel(1, 4, '*', ConsoleColor.Red));
            halfDestroyedBody.Add(new Pixel(2, 3, '*', ConsoleColor.Red));
            halfDestroyedBody.Add(new Pixel(1, 4, '*', ConsoleColor.Red));
            halfDestroyedBody.Add(new Pixel(0, 5, '*', ConsoleColor.Red));
            halfDestroyedBody.Add(new Pixel(-1, 4, '*', ConsoleColor.Red));
            halfDestroyedBody.Add(new Pixel(-2, 3, '*', ConsoleColor.Red));

            IList<Pixel> destroyedBody = new List<Pixel>();
            destroyedBody.Add(new Pixel(-2, 1, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(2, 1, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(2, 5, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(-2, 5, '+', ConsoleColor.Red));

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
