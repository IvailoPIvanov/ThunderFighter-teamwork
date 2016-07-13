namespace ThunderFighter.Models.Enemies.Abstract
{
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Bullets.Abstract;    
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Common.Abstract;
    using ThunderFighter.Models.Missiles.Abstract;

    public abstract class Enemy : Entity, IMovable, IBulletShooter
    {
        private static uint bulletsEngaged = 0;

        private decimal deltaX;
        private decimal deltaY;

        private decimal enemyPositionX;
        private decimal enemyPositionY;

        private List<Bullet> bullets;
        private List<Missile> missiles;

        private bool isShootingEnabled;

        protected Enemy(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) :
            base(field, position, bodyStates, entityState)
        {
            // defines initial enemy movement direction values
            this.DeltaX = -1.0M;
            this.DeltaY = 0M;

            this.EnemyPositionX = position.X;
            this.EnemyPositionY = position.Y;

            this.bullets = new List<Bullet>();
            this.missiles = new List<Missile>();

            this.IsShootingEnabled = false;
        }

        public static uint BulletsEngaged
        {
            get
            {
                return Enemy.bulletsEngaged;
            }

            set
            {
                Enemy.bulletsEngaged = value;
            }
        }

        public bool IsShootingEnabled
        {
            get
            {
                return this.isShootingEnabled;
            }

            set
            {
                this.isShootingEnabled = value;
            }
        }

        public decimal DeltaX
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

        public decimal DeltaY
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

        internal List<Bullet> Bullets
        {
            get
            {
                return this.bullets;
            }

            set
            {
                this.bullets = value;
            }
        }

        internal List<Missile> Missiles
        {
            get
            {
                return this.missiles;
            }

            set
            {
                this.missiles = value;
            }
        }

        protected decimal EnemyPositionX
        {
            get
            {
                return this.enemyPositionX;
            }

            set
            {
                this.enemyPositionX = value;
            }
        }

        protected decimal EnemyPositionY
        {
            get
            {
                return this.enemyPositionY;
            }

            set
            {
                this.enemyPositionY = value;
            }
        }

        public virtual void Move()
        {
            this.EnemyPositionX += this.DeltaX;            

            // fly in a safe distance above buildings
            if (this.EnemyPositionY + this.DeltaY <= this.Field.PlayHeight - 10)
            {
                this.EnemyPositionY += this.DeltaY;
            }

            this.Position.X = (int)this.EnemyPositionX;
            this.Position.Y = (int)this.EnemyPositionY;            
        }

        public abstract void BulletShoot();

        public abstract int PointsGainOnDie();
    }
}
