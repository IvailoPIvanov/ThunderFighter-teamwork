namespace ThunderFighter.Models.Fighters
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Bombs;
    using ThunderFighter.Models.Bullets;
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Fighters.Abstract;

    internal class ThunderFighterOne : Fighter, IBulletShooter, IBomber
    {
        public ThunderFighterOne(Field field, Point2D position) :
            this(field, position, EntityStateType.Strong)
        {
        }

        public ThunderFighterOne(Field field, Point2D position, EntityStateType state) :
            this(field, position, ThunderFighterOne.BodyStates(), state)
        {
        }

        public ThunderFighterOne(Field field, Point2D position, IList<IList<Pixel>> bodies, EntityStateType state) :
            base(field, position, bodies, state)
        {
        }

        public override void BulletShoot()
        {
            var bullet = new LightweightBullet(this.Field, new Point2D(this.Position));

            // redefines bullet speed and direction
            bullet.DeltaX = 3;
            bullet.DeltaY = 0;

            this.Bullets.Add(bullet);
        }

        public override void ThrowBomb()
        {
            var bomb = new PavewayBomb(this.Field, new Point2D(this.Position));

            this.Bombs.Add(bomb);
        }

        private static IList<IList<Pixel>> BodyStates()
        {
            IList<IList<Pixel>> bodyStates = new List<IList<Pixel>>();

            IList<Pixel> strongBody = new List<Pixel>();
            strongBody.Add(new Pixel(1, 0, '/', Theme.Contrast));  // wing 1
            strongBody.Add(new Pixel(2, 0, '\\', Theme.Contrast));
            strongBody.Add(new Pixel(1, 1, '\\', Theme.Contrast));
            strongBody.Add(new Pixel(2, 1, '*', ConsoleColor.Red));
            strongBody.Add(new Pixel(3, 1, '\\', Theme.Contrast));
            strongBody.Add(new Pixel(1, 2, '/', Theme.Contrast));
            strongBody.Add(new Pixel(4, 2, '\\', Theme.Contrast));  // wing 1
            strongBody.Add(new Pixel(0, 3, ']', ConsoleColor.Red));   // body
            strongBody.Add(new Pixel(1, 3, '=', Theme.Contrast));
            strongBody.Add(new Pixel(2, 3, '=', Theme.Contrast));
            strongBody.Add(new Pixel(3, 3, '=', Theme.Contrast));
            strongBody.Add(new Pixel(4, 3, '=', Theme.Contrast));
            strongBody.Add(new Pixel(5, 3, 'D', Theme.Contrast));
            strongBody.Add(new Pixel(6, 3, '>', Theme.Contrast)); // body
            strongBody.Add(new Pixel(1, 6, '\\', Theme.Contrast));  // wing 2
            strongBody.Add(new Pixel(2, 6, '/', Theme.Contrast));
            strongBody.Add(new Pixel(1, 5, '/', Theme.Contrast));
            strongBody.Add(new Pixel(2, 5, '*', ConsoleColor.Red));
            strongBody.Add(new Pixel(3, 5, '/', Theme.Contrast));
            strongBody.Add(new Pixel(1, 4, '\\', Theme.Contrast));
            strongBody.Add(new Pixel(4, 4, '/', Theme.Contrast)); // wing 2

            IList<Pixel> halfDestroyedBody = new List<Pixel>();
            halfDestroyedBody.Add(new Pixel(1, 2, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(2, 2, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(3, 2, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(0, 3, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(1, 3, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(2, 3, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(3, 3, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(4, 3, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(1, 4, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(2, 4, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(3, 4, '*', ConsoleColor.DarkMagenta));

            IList<Pixel> destroyedBody = new List<Pixel>();
            destroyedBody.Add(new Pixel(2, 0, '+', ConsoleColor.Red)); 
            destroyedBody.Add(new Pixel(-1, 1, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(4, 1, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(1, 2, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(3, 2, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(-2, 3, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(4, 3, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(1, 4, '+', ConsoleColor.Red)); 
            destroyedBody.Add(new Pixel(3, 4, '+', ConsoleColor.Red)); 
            destroyedBody.Add(new Pixel(0, 5, '+', ConsoleColor.Red));
            destroyedBody.Add(new Pixel(6, 5, '+', ConsoleColor.Red)); 
            destroyedBody.Add(new Pixel(2, 6, '+', ConsoleColor.Red));

            IList<Pixel> disappearedBody = new List<Pixel>();
            disappearedBody.Add(new Pixel(2, 0, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(-1, 1, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(4, 1, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(1, 2, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(3, 2, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(-2, 3, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(4, 3, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(1, 4, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(3, 4, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(0, 5, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(6, 5, '+', ConsoleColor.Red));
            disappearedBody.Add(new Pixel(2, 6, '+', ConsoleColor.Red));

            bodyStates.Add(strongBody);        // EntityState.Strong
            bodyStates.Add(halfDestroyedBody); // EntityState.HalfDestroyed
            bodyStates.Add(destroyedBody);     // EntityState.Destroyed
            bodyStates.Add(disappearedBody);   // EntityState.Disappeared

            return bodyStates;
        }
    }
}
