namespace ThunderFighter
{    
    using System;
    using ThunderFighter.Common.Enums;

    public class Theme
    {
        public Theme(ThemeColorType color)
        {
            this.Color = color;
            if (color == ThemeColorType.White)
            {
                Theme.BackGround = ConsoleColor.White;
                Theme.Contrast = ConsoleColor.Black;
                Theme.Light = ConsoleColor.Gray;
                Theme.Red = ConsoleColor.DarkRed;
                Theme.Green = ConsoleColor.DarkGreen;
                Theme.Blue = ConsoleColor.Blue;
                Theme.Magenta = ConsoleColor.DarkMagenta;
            }
            else if (color == ThemeColorType.Black)
            {
                Theme.BackGround = ConsoleColor.Black;
                Theme.Contrast = ConsoleColor.White;
                Theme.Light = ConsoleColor.DarkGray;
                Theme.Red = ConsoleColor.Red;
                Theme.Green = ConsoleColor.Green;
                Theme.Blue = ConsoleColor.Cyan;
                Theme.Magenta = ConsoleColor.DarkMagenta;
            }
            else if (color == ThemeColorType.Blue)
            {
                Theme.BackGround = ConsoleColor.DarkBlue;
                Theme.Contrast = ConsoleColor.Yellow;
                Theme.Light = ConsoleColor.DarkGray;
                Theme.Red = ConsoleColor.Red;
                Theme.Green = ConsoleColor.White;
                Theme.Blue = ConsoleColor.Magenta;
                Theme.Magenta = ConsoleColor.Cyan;
            }
        }

        public static ConsoleColor Magenta { get; set; }

        public static ConsoleColor BackGround { get; set; }

        public static ConsoleColor Contrast { get; set; }

        public static ConsoleColor Light { get; set; }

        public static ConsoleColor Red { get; set; }

        public static ConsoleColor Green { get; set; }

        public static ConsoleColor Blue { get; set; }

        public ThemeColorType Color { get; set; }
    }
}
