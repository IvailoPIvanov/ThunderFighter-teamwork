namespace ThunderFighter.Models.Fighters.Abstract
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Constants;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Exceptions;
    using ThunderFighter.Contracts;
    using ThunderFighter.Controls;
    using ThunderFighter.Models.Bombs.Abstract;
    using ThunderFighter.Models.Bullets.Abstract;
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Common.Abstract;
    using ThunderFighter.Models.Missiles.Abstract;

    public abstract class Fighter : Entity, IMovable, IBulletShooter, IBomber
    {
        private string name;
        private List<Bullet> bullets;
        private List<Bomb> bombs;
        private List<Missile> missiles;

        private MoveDirectionType moveDirection;

        protected Fighter(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) :
            base(field, position, bodyStates, entityState)
        {
            this.Name = Constants.NameNotSet;
            this.bullets = new List<Bullet>();
            this.bombs = new List<Bomb>();
            this.missiles = new List<Missile>();

            this.moveDirection = MoveDirectionType.OnHold;

            ConsoleKeyboardHandler.Instance.KeyDown += this.Instance_KeyDown;
        }

        internal string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value.Length < 3 && value.Length > 10)
                {
                    throw new NameFormatException(value);
                }

                this.name = value;
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

        internal List<Bomb> Bombs
        {
            get
            {
                return this.bombs;
            }

            set
            {
                this.bombs = value;
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

        public virtual void Move()
        {
            if (this.moveDirection == MoveDirectionType.Left && this.Body.Exists(pixel => pixel.Coordinate.X > this.Width))
            {
                this.Position.X--;
            }
            else if (this.moveDirection == MoveDirectionType.Right && this.Body.Exists(pixel => pixel.Coordinate.X + this.Width < this.Field.PlayWidth - (this.Field.PlayWidth / 3)))
            {
                this.Position.X++;
            }
            else if (this.moveDirection == MoveDirectionType.Down && this.Body.Exists(pixel => pixel.Coordinate.Y + this.Height < this.Field.PlayHeight - 1))
            {
                this.Position.Y++;
            }
            else if (this.moveDirection == MoveDirectionType.Up && this.Body.Exists(pixel => pixel.Coordinate.Y > this.Height))
            {
                this.Position.Y--;
            }

            this.moveDirection = MoveDirectionType.OnHold;
        }

        public abstract void BulletShoot();

        public abstract void ThrowBomb();

        protected virtual void HandleKeyDown(ConsoleKeyDownEventArgs args)
        {
            if (args.KeyInfo.Key == ConsoleKey.Spacebar)
            {
                this.BulletShoot();
            }
            else if (args.KeyInfo.Key == ConsoleKey.B)
            {
                this.ThrowBomb();
            }
            else if (args.KeyInfo.Key == ConsoleKey.LeftArrow)
            {
                this.moveDirection = MoveDirectionType.Left;
            }
            else if (args.KeyInfo.Key == ConsoleKey.RightArrow)
            {
                this.moveDirection = MoveDirectionType.Right;
            }
            else if (args.KeyInfo.Key == ConsoleKey.DownArrow)
            {
                this.moveDirection = MoveDirectionType.Down;
            }
            else if (args.KeyInfo.Key == ConsoleKey.UpArrow)
            {
                this.moveDirection = MoveDirectionType.Up;
            }
        }

        private void Instance_KeyDown(object sender, ConsoleKeyDownEventArgs e)
        {
            this.HandleKeyDown(e);
        }
    }
}