namespace ThunderFighter.Models.Enemies
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Enemies.Abstract;    

    internal class CrazyCrawlEnemy : Enemy, IBulletShooter
    {
        public CrazyCrawlEnemy(Field field, Point2D position) :
            this(field, position, EntityStateType.Strong)
        {
        }

        public CrazyCrawlEnemy(Field field, Point2D position, EntityStateType entityState) :
            this(field, position, CrazyCrawlEnemy.BodyStates(), entityState)
        {
        }

        public CrazyCrawlEnemy(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) :
            base(field, position, bodyStates, entityState)
        {
            // you can override here initial bomb movement direction values set in base constructor
            this.DeltaX = -2;
            this.DeltaY = 0;
        }

        public override void Move()
        {
            // moves normally and when reach 1/2 of field start moving in a crazy way
            if (this.Position.X < this.Field.PlayWidth / 2)
            {
                this.DeltaX = RandomProvider.Instance.Next(-2, 1);
                this.DeltaY = RandomProvider.Instance.Next(-1, 2);
            }

            this.EnemyPositionX += this.DeltaX;
            this.EnemyPositionY += this.DeltaY;

            this.Position.X = (int)this.EnemyPositionX;
            this.Position.Y = (int)this.EnemyPositionY;
        }

        public override void BulletShoot()
        {
            throw new NotImplementedException();
        }

        public override int PointsGainOnDie()
        {
            return (int)PointsGainType.Bug;
        }

        private static IList<IList<Pixel>> BodyStates()
        {
            IList<IList<Pixel>> bodyStates = new List<IList<Pixel>>();

            IList<Pixel> strongBody = new List<Pixel>();
            strongBody.Add(new Pixel(0, 0, 'O', Theme.Contrast));
            strongBody.Add(new Pixel(1, -1, '(', Theme.Red));
            strongBody.Add(new Pixel(2, -1, '(', Theme.Red));
            strongBody.Add(new Pixel(2, 1, '(', Theme.Red));
            strongBody.Add(new Pixel(1, 1, '(', Theme.Red));
            strongBody.Add(new Pixel(1, 0, 'O', Theme.Contrast));
            strongBody.Add(new Pixel(2, 0, 'O', Theme.Contrast));
            strongBody.Add(new Pixel(3, 0, '<', ConsoleColor.Red));

            IList<Pixel> halfDestroyedBody = new List<Pixel>();
            halfDestroyedBody.Add(new Pixel(0, 0, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(-1, 1, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(0, 1, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(1, 1, '*', ConsoleColor.DarkMagenta));
            halfDestroyedBody.Add(new Pixel(0, 2, '*', ConsoleColor.DarkMagenta));

            IList<Pixel> destroyedBody = new List<Pixel>();
            destroyedBody.Add(new Pixel(0, -1, '+', ConsoleColor.DarkYellow));
            destroyedBody.Add(new Pixel(2, -1, '+', ConsoleColor.DarkYellow));
            destroyedBody.Add(new Pixel(-1, 1, '+', ConsoleColor.DarkYellow));
            destroyedBody.Add(new Pixel(2, 0, '+', ConsoleColor.DarkYellow));
            destroyedBody.Add(new Pixel(1, -2, '+', ConsoleColor.DarkYellow));
            destroyedBody.Add(new Pixel(2, 2, '+', ConsoleColor.DarkYellow));

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
