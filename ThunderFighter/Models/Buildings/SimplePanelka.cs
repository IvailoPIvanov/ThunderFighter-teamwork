namespace ThunderFighter.Models.Buildings
{
    using System;
    using System.Collections.Generic;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Contracts;
    using ThunderFighter.Models.Buildings.Abstract;
    using ThunderFighter.Models.Common;

    internal class SimplePanelka : Building, IBulletShooter
    {
        public SimplePanelka(Field field, Point2D position) :
            this(field, position, EntityStateType.Strong)
        {
        }

        public SimplePanelka(Field field, Point2D position, EntityStateType entityState) :
            this(field, position, SimplePanelka.BodyStates(), entityState)
        {
        }

        public SimplePanelka(Field field, Point2D position, IList<IList<Pixel>> bodyStates, EntityStateType entityState) :
            base(field, position, bodyStates, entityState)
        {
        }

        public void BulletShoot()
        {
            throw new NotImplementedException();
        }

        public override int PointsGainOnDestroy()
        {
            return (int)PointsGainType.Panelka;
        }

        private static IList<IList<Pixel>> BodyStates()
        {
            IList<IList<Pixel>> bodyStates = new List<IList<Pixel>>();

            IList<Pixel> strongBody = new List<Pixel>();

            int w = RandomProvider.Instance.Next(2, 16);
            if (w % 2 != 0)
            {
                w = w + 1;
            }

            int h = RandomProvider.Instance.Next(2, 8);
            int r = RandomProvider.Instance.Next(33, 128);

            for (int i = 1; i < w; i++)
            {
                for (int j = 0; j <= h; j++)
                {
                    strongBody.Add(new Pixel(0, j * (-1), '|', Theme.Contrast));
                    strongBody.Add(new Pixel(w, j * (-1), '|', Theme.Contrast));
                    strongBody.Add(new Pixel(i, h * -1, '—', Theme.Contrast));

                    if (i % 2 != 0 && i > 0 && j < h && j > 0)
                    {
                        strongBody.Add(new Pixel(i, j * -1, (char)r, Theme.Light));
                    }
                }
            }

            IList<Pixel> halfDestroyedBody = new List<Pixel>();
            halfDestroyedBody.Add(new Pixel(1, -1, '$', Theme.Contrast));
            halfDestroyedBody.Add(new Pixel(2, 0, '@', Theme.Contrast));
            halfDestroyedBody.Add(new Pixel(3, -1, '#', Theme.Contrast));

            IList<Pixel> destroyedBody = new List<Pixel>();
            destroyedBody.Add(new Pixel(0, -2, '*', Theme.Red));
            destroyedBody.Add(new Pixel(2, 0, '#', Theme.Red));
            destroyedBody.Add(new Pixel(4, -1, '@', Theme.Red));

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
