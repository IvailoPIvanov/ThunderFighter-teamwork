namespace ThunderFighter.Models.Buildings
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Buildings.Abstract;
    using ThunderFighter.Models.Common;

    internal class SimpleHouse : Building, IBulletShooter
    {
        public SimpleHouse(Field field, Point2D position) :
            this(field, position, EntityStateType.Strong)
        {
        }

        public SimpleHouse(Field field, Point2D position, EntityStateType entityState) :
            this(field, position, SimpleHouse.BodyStates(), entityState)
        {
        }

        public SimpleHouse(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) :
            base(field, position, bodyStates, entityState)
        {
        }

        public void BulletShoot()
        {
            throw new NotImplementedException();
        }

        public override int PointsGainOnDestroy()
        {
            return (int)PointsGainType.House;
        }

        private static IList<IList<Pixel>> BodyStates()
        {
            IList<IList<Pixel>> bodyStates = new List<IList<Pixel>>();

            IList<Pixel> strongBody = new List<Pixel>();
            strongBody.Add(new Pixel(0, 0, '|', Theme.Contrast));
            strongBody.Add(new Pixel(0, -1, '/', Theme.Red));
            strongBody.Add(new Pixel(1, 0, '[', Theme.Green));
            strongBody.Add(new Pixel(1, -2, '/', Theme.Red));
            strongBody.Add(new Pixel(2, 0, ']', Theme.Green));
            strongBody.Add(new Pixel(2, -2, '\\', Theme.Red));
            strongBody.Add(new Pixel(2, -3, '_', Theme.Red));
            strongBody.Add(new Pixel(3, 0, '|', Theme.Contrast));
            strongBody.Add(new Pixel(3, -1, '\\', Theme.Red));
            strongBody.Add(new Pixel(3, -3, '_', Theme.Red));
            strongBody.Add(new Pixel(4, -1, '_', Theme.Red));
            strongBody.Add(new Pixel(4, -3, '_', Theme.Red));
            strongBody.Add(new Pixel(5, 0, '[', Theme.Green));
            strongBody.Add(new Pixel(5, -1, '_', Theme.Red));
            strongBody.Add(new Pixel(5, -3, 'I', Theme.Contrast));
            strongBody.Add(new Pixel(6, 0, ']', Theme.Green));
            strongBody.Add(new Pixel(6, -1, '_', Theme.Red));
            strongBody.Add(new Pixel(6, -3, '_', Theme.Red));
            strongBody.Add(new Pixel(7, -1, '_', Theme.Red));
            strongBody.Add(new Pixel(7, -2, '\\', Theme.Red));
            strongBody.Add(new Pixel(8, 0, '|', Theme.Contrast));
            strongBody.Add(new Pixel(8, -1, '\\', Theme.Red));

            IList<Pixel> halfDestroyedBody = new List<Pixel>();
            halfDestroyedBody.Add(new Pixel(1, -1, '#', Theme.Red));
            halfDestroyedBody.Add(new Pixel(2,  0, '#', Theme.Red));
            halfDestroyedBody.Add(new Pixel(3, -1, '#', Theme.Red));

            IList<Pixel> destroyedBody = new List<Pixel>();
            destroyedBody.Add(new Pixel(0, -2, '#', Theme.Contrast));
            destroyedBody.Add(new Pixel(2, 0, '#', Theme.Contrast));
            destroyedBody.Add(new Pixel(4, -1, '#', Theme.Contrast));

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
