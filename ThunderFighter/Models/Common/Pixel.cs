namespace ThunderFighter.Models.Common
{
    using System;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Contracts;

    public class Pixel : IDrawable, IClearable
    {
        private Point2D coordinate;
        private char symbol;
        private ConsoleColor color;

        public Pixel(int x, int y, char symbol, ConsoleColor color)
        {
            this.Coordinate = new Point2D(x, y);
            this.Symbol = symbol;
            this.Color = color;
        }

        public Point2D Coordinate
        {
            get
            {
                return this.coordinate;
            }

            internal set
            {
                this.coordinate = value;
            }
        }

        public char Symbol
        {
            get
            {
                return this.symbol;
            }

            internal set
            {
                this.symbol = value;
            }
        }

        public ConsoleColor Color
        {
            get
            {
                return this.color;
            }

            internal set
            {
                this.color = value;
            }
        }

        public static void Clear(int x, int y, bool clearForced)
        {
            ScreenBuffer.Clear(x, y, clearForced);
        }

        public static void Clear(int x, int y, string text, bool clearForced)
        {
            ScreenBuffer.Clear(x, y, text, clearForced);
        }

        public static void Draw(int x, int y, char symbol, ConsoleColor foregroundColor, bool drawForced)
        {
            ScreenBuffer.Draw(x, y, symbol, foregroundColor, drawForced);
        }

        public static void Draw(int x, int y, string text, ConsoleColor foregroundColor, bool drawForced)
        {
            ScreenBuffer.Draw(x, y, text, foregroundColor, drawForced);
        }

        public void Clear()
        {
            this.Clear(false);
        }

        public void Clear(bool clearForced)
        {
            ScreenBuffer.Clear(this.Coordinate.X, this.Coordinate.Y, clearForced);
        }

        public void Draw()
        {
            this.Draw(false);
        }

        public void Draw(bool drawForced)
        {
            ScreenBuffer.Draw(this.Coordinate.X, this.Coordinate.Y, this.Symbol, this.Color, drawForced);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                int prime = 31;

                unchecked
                {
                    hash = (hash * prime) + this.Coordinate.X.GetHashCode();
                    hash = (hash * prime) + this.Coordinate.Y.GetHashCode();
                    hash = (hash * prime) + this.Symbol.GetHashCode();
                    hash = (hash * prime) + this.Color.GetHashCode();
                }

                return hash;
            }
        }
    }
}
