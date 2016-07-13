namespace ThunderFighter.Models.Common.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Contracts;

    public abstract class Entity : IDrawable, IClearable
    {
        private Field field;
        private Point2D position;
        private IList<IList<Pixel>> relativeBodyStates;
        private List<Pixel> body;

        private int previousLeft;
        private int previousRight;
        private int previousTop;
        private int previousBottom;

        private EntityStateType state;

        private int width;
        private int height;

        private bool isDestroyed;

        public Entity(Field field, Point2D position, IList<IList<Pixel>> relativeBodyStates, EntityStateType state)
        {
            this.Field = field;
            this.Position = position;
            this.relativeBodyStates = relativeBodyStates;
            this.State = (int)state;

            // TODO: refactor to not use 2 lists (very difficult task)
            //this.Body = relativeBodyStates[this.State]
            //    .ConvertAll(pixel => new Pixel(pixel.Coordinate.X, pixel.Coordinate.Y, pixel.Symbol, pixel.Color));

            this.Body = new List<Pixel>(relativeBodyStates[this.State].Count);
            for (int i = 0; i < relativeBodyStates[this.State].Count; i++)
            {
                //Pixel pixel = relativeBodyStates[this.State][i];

                this.Body.Add(new Pixel(relativeBodyStates[this.State][i].Coordinate.X, relativeBodyStates[this.State][i].Coordinate.Y, relativeBodyStates[this.State][i].Symbol, relativeBodyStates[this.State][i].Color));
            }

            this.CalculateWidthAndHeightOfEntityBody();

            this.ReCalculateBody();

            this.previousLeft = 0;
            this.previousTop = 0;
            this.previousRight = field.PlayWidth - 1;
            this.previousBottom = field.PlayHeight - 1;
        }

        public Field Field
        {
            get
            {
                return this.field;
            }

            private set
            {
                this.field = value;
            }
        }

        public List<Pixel> Body
        {
            get
            {
                return this.body;
            }

            private set
            {
                this.body = value;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }

            private set
            {
                this.width = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }

            private set
            {
                this.height = value;
            }
        }

        public bool IsDestroyed
        {
            get
            {
                return this.isDestroyed;
            }

            private set
            {
                this.isDestroyed = value;
            }
        }

        public int State
        {
            get
            {
                return (int)this.state;
            }

            internal set
            {
                this.state = (EntityStateType)value;
            }
        }

        public Point2D Position
        {
            get
            {
                return this.position;
            }

            protected set
            {
                this.position = value;
            }
        }

        public void Draw()
        {
            this.Draw(false);
        }

        public void Draw(bool drawForced)
        {
            this.ReCalculateBody();

            if (this.state == EntityStateType.HalfDestroyed)
            {
                this.state = EntityStateType.Destroyed;
            }
            else if (this.state == EntityStateType.Destroyed)
            {
                this.state = EntityStateType.Disappeared;
            }
            else if (this.state == EntityStateType.Disappeared)
            {
                this.IsDestroyed = true;
            }

            // borders of rectangle which includes body in previous and new position
            int left = Math.Min(this.Field.PlayWidth - 1, Math.Max(0, Math.Min(this.previousLeft, this.Body.Select(pixel => pixel.Coordinate.X).Min())));
            int top = Math.Min(this.Field.PlayHeight - 1, Math.Max(0, Math.Min(this.previousTop, this.Body.Select(pixel => pixel.Coordinate.Y).Min())));
            int right = Math.Max(0, Math.Min(this.Field.PlayWidth - 1, Math.Max(this.previousRight, this.Body.Select(pixel => pixel.Coordinate.X).Max())));
            int bottom = Math.Max(0, Math.Min(this.Field.PlayHeight - 1, Math.Max(this.previousBottom, this.Body.Select(pixel => pixel.Coordinate.Y).Max())));

            this.previousLeft = left;
            this.previousTop = top;
            this.previousRight = right;
            this.previousBottom = bottom;

            foreach (Pixel pixel in this.Body)
            {
                if (!(pixel.Coordinate.X < 0 ||
                    pixel.Coordinate.X >= this.Field.PlayWidth ||
                    pixel.Coordinate.Y < 0 ||
                    pixel.Coordinate.Y >= this.Field.PlayHeight))
                {
                    pixel.Draw();
                }
            }

            // Avoids screen flickering: draw rectangle which includes body in previous and new position
            if (drawForced)
            {
                ScreenBuffer.DrawRectangle(left, top, right, bottom);
            }
        }

        public void Clear()
        {
            foreach (Pixel pixel in this.Body)
            {
                if (!(pixel.Coordinate.X < 0 ||
                    pixel.Coordinate.X >= this.Field.PlayWidth ||
                    pixel.Coordinate.Y < 0 ||
                    pixel.Coordinate.Y >= this.Field.PlayHeight))
                {
                    pixel.Clear();
                }
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                int prime = 31;

                foreach (var pixel in this.Body)
                {
                    hash = (hash * prime) + pixel.GetHashCode();
                }

                return hash;
            }
        }

        private void ReCalculateBody()
        {
            if (this.State != (int)EntityStateType.Strong)
            {
                //this.Body = this.relativeBodyStates[this.State]
                //    .ConvertAll(pixel => new Pixel(pixel.Coordinate.X, pixel.Coordinate.Y, pixel.Symbol, pixel.Color));

                this.Body = new List<Pixel>(this.relativeBodyStates[this.State].Count);
                for (int i = 0; i < this.relativeBodyStates[this.State].Count; i++)
                {
                    //Pixel pixel = this.relativeBodyStates[this.State][i];

                    this.Body.Add(new Pixel(this.relativeBodyStates[this.State][i].Coordinate.X, this.relativeBodyStates[this.State][i].Coordinate.Y, this.relativeBodyStates[this.State][i].Symbol, this.relativeBodyStates[this.State][i].Color));
                }

                this.CalculateWidthAndHeightOfEntityBody();
            }

            for (int i = 0; i < this.Body.Count; i++)
            {
                this.Body[i].Coordinate.X =
                    this.Position.X + this.relativeBodyStates[this.State][i].Coordinate.X;
                this.Body[i].Coordinate.Y =
                    this.Position.Y + this.relativeBodyStates[this.State][i].Coordinate.Y;
            }
        }

        private void CalculateWidthAndHeightOfEntityBody()
        {
            int left = this.Body.Select(pixel => pixel.Coordinate.X).Min();
            int right = this.Body.Select(pixel => pixel.Coordinate.X).Max();
            int top = this.Body.Select(pixel => pixel.Coordinate.Y).Min();
            int bottom = this.Body.Select(pixel => pixel.Coordinate.Y).Max();

            this.Width = Math.Abs(left - right);
            this.Height = Math.Abs(top - bottom);
        }
    }
}
