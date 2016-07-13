namespace ThunderFighter.Models.Buildings
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Buildings.Abstract;
    using ThunderFighter.Models.Common;

    internal class ShootingTower : Building, IBulletShooter
    {
        public ShootingTower(Field field, Point2D position) :
            this(field, position, EntityStateType.Strong)
        {
        }

        public ShootingTower(Field field, Point2D position, EntityStateType entityState) :
            this(field, position, ShootingTower.BodyStates(), entityState)
        {
        }

        public ShootingTower(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) :
            base(field, position, bodyStates, entityState)
        {
        }

        public void BulletShoot()
        {
            throw new NotImplementedException();
        }

        public override int PointsGainOnDestroy()
        {
            return (int)PointsGainType.Tower;
        }

        private static IList<IList<Pixel>> BodyStates()
        {
            IList<IList<Pixel>> bodyStates = new List<IList<Pixel>>();

            int h = RandomProvider.Instance.Next(2, 12);

            IList<Pixel> strongBody = new List<Pixel>();
            for (int i = 0; i < h; i++)
            {
                strongBody.Add(new Pixel(5, i * -1, '|', Theme.Magenta));
                strongBody.Add(new Pixel(0, i * -1, '|', Theme.Magenta));
            }

            for (int i = 2; i < h; i += 2)
            {
                strongBody.Add(new Pixel(2, i * -1, '[', Theme.Red));
                strongBody.Add(new Pixel(3, i * -1, ']', Theme.Red));
            }

            strongBody.Add(new Pixel(1, -h, '/', Theme.Magenta));
            strongBody.Add(new Pixel(2, -h - 1, '/', Theme.Magenta));
            strongBody.Add(new Pixel(3, -h - 1, '\\', Theme.Magenta));
            strongBody.Add(new Pixel(4, -h, '\\', Theme.Magenta));

            strongBody.Add(new Pixel(0, 0, '[', Theme.Red));
            strongBody.Add(new Pixel(1, 0, ']', Theme.Red));
            strongBody.Add(new Pixel(2, 0, '[', Theme.Red));
            strongBody.Add(new Pixel(3, 0, ']', Theme.Red));
            strongBody.Add(new Pixel(4, 0, '[', Theme.Red));
            strongBody.Add(new Pixel(5, 0, ']', Theme.Red));

            IList<Pixel> halfDestroyedBody = new List<Pixel>();
            halfDestroyedBody.Add(new Pixel(1, -1, '#', Theme.Red));
            halfDestroyedBody.Add(new Pixel(2, 0, '#', Theme.Red));
            halfDestroyedBody.Add(new Pixel(3, -1, '#', Theme.Red));

            IList<Pixel> destroyedBody = new List<Pixel>();
            destroyedBody.Add(new Pixel(0, 0, '|', Theme.Red));

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
