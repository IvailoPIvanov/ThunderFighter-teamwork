namespace ThunderFighter.Models.Enemies
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Constants;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Bullets;
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Enemies.Abstract;

    internal class BadShooterEnemy : Enemy, IBulletShooter
    {
        // random position where BadShooter enemy stops movement on X axis
        private int randomPositionToStopMovementOnXAxis;

        public BadShooterEnemy(Field field, Point2D position) :
            this(field, position, EntityStateType.Strong)
        {
        }

        public BadShooterEnemy(Field field, Point2D position, EntityStateType entityState) :
            this(field, position, BadShooterEnemy.BodyStates(), entityState)
        {
        }

        public BadShooterEnemy(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) :
            base(field, position, bodyStates, entityState)
        {
            // you can override here initial bomb movement direction values set in base constructor
            this.DeltaX = -1;
            this.DeltaY = -0.5M;

            this.randomPositionToStopMovementOnXAxis = RandomProvider.Instance.Next(this.Field.PlayWidth / 4, this.Field.PlayWidth - (this.Field.PlayWidth / 4));

            this.IsShootingEnabled = true;
        }

        public override void Move()
        {
            if (this.Position.X <= this.randomPositionToStopMovementOnXAxis)
            {
                this.DeltaX = 0;
            }

            if (this.Position.Y <= 0)
            {
                this.DeltaY = 0.5M;
            }

            if (this.Position.Y >= this.Field.PlayHeight - 10)
            {
                this.DeltaY = -0.5M;
            }

            this.EnemyPositionX += this.DeltaX;
            this.EnemyPositionY += this.DeltaY;

            this.Position.X = (int)this.EnemyPositionX;
            this.Position.Y = (int)this.EnemyPositionY;

            // TODO: shooting frequency - should depend on level
            if (this.IsShootingEnabled && Enemy.BulletsEngaged < Constants.EasyEnemyBulletsMaxCount && this.Position.X < this.Field.PlayWidth)
            {
                // shoot with 2% probability
                if (RandomProvider.Instance.Next(0, 50) == 0)
                {
                    this.BulletShoot();

                    Enemy.BulletsEngaged++;
                }
            }
        }

        public override void BulletShoot()
        {
            var bullet = new LightEnemyBullet(this.Field, new Point2D(this.Position));

            // redefines bullet speed and direction
            bullet.DeltaX = -2;
            bullet.DeltaY = 0;

            this.Bullets.Add(bullet);
        }

        public override int PointsGainOnDie()
        {
            return (int)PointsGainType.Shooter;
        }

        private static IList<IList<Pixel>> BodyStates()
        {
            IList<IList<Pixel>> bodyStates = new List<IList<Pixel>>();

            IList<Pixel> strongBody = new List<Pixel>();
            strongBody.Add(new Pixel(0, 0, ':', Theme.Contrast));
            strongBody.Add(new Pixel(1, -1, '<', Theme.Green));
            strongBody.Add(new Pixel(2, -1, '|', Theme.Green));
            strongBody.Add(new Pixel(2, 1, '|', Theme.Green));
            strongBody.Add(new Pixel(1, 1, '<', Theme.Green));
            strongBody.Add(new Pixel(1, 0, 'X', Theme.Contrast));
            strongBody.Add(new Pixel(2, 0, 'X', Theme.Contrast));
            strongBody.Add(new Pixel(3, 0, ':', Theme.Light));

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
