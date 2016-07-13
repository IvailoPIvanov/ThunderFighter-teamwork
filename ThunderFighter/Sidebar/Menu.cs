namespace ThunderFighter.Sidebar
{
    using System;
    using ThunderFighter.Common.Constants;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Models.Common;

    internal class Menu
    {
        private Engine engine;
        private Point2D position;

        public Menu(Field field, Engine engine)
        {
            this.Position = new Point2D(field.PlayWidth + 2, 0);
            this.Engine = engine;
        }

        public Engine Engine
        {
            get
            {
                return this.engine;
            }

            private set
            {
                this.engine = value;
            }
        }

        public Point2D Position
        {
            get
            {
                return this.position;
            }

            private set
            {
                this.position = value;
            }
        }

        public void CreateBase()
        {
            // Heading
            ScreenBuffer.Draw(this.Position.X, 1, new string('*', Constants.MenuWidth - 5), Theme.Light, false);
            ScreenBuffer.Draw(this.Position.X, 2, string.Format("*{0}*", new string(' ', Constants.MenuWidth - 7)), Theme.Light, false);
            ScreenBuffer.Draw(this.Position.X + 4, 2, "THUNDER FIGHTER", Theme.Blue, false);
            ScreenBuffer.Draw(this.Position.X, 3, new string('*', Constants.MenuWidth - 5), Theme.Light, false);
            ScreenBuffer.Draw(this.Position.X, 4, "by Team - Hestia", ConsoleColor.DarkGray, false);

            // Game info
            ScreenBuffer.Draw(this.Position.X, 7, "Game info:", Theme.Blue, false);
            for (int k = 9; k < 14; k++)
            {
                ScreenBuffer.Draw(this.Position.X, k, "*", Theme.Light, false);
                ScreenBuffer.Draw(this.Position.X + Constants.MenuWidth - 6, k, "*", Theme.Light, false);
            }

            ScreenBuffer.Draw(this.Position.X, 8, new string('*', Constants.MenuWidth - 5), Theme.Light, false);
            ScreenBuffer.Draw(this.Position.X + 5, 9, "Name:", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 11, "Lives:", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 12, "Score:", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 13, "Level:", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X, 14, new string('*', Constants.MenuWidth - 5), Theme.Light, false);

            // Controls block
            ScreenBuffer.Draw(this.Position.X, 16, "Controls:", Theme.Blue, false);
            for (int k = 18; k < 26; k++)
            {
                ScreenBuffer.Draw(this.Position.X, k, "*", Theme.Light, false);
                ScreenBuffer.Draw(this.Position.X + Constants.MenuWidth - 6, k, "*", Theme.Light, false);
            }

            ScreenBuffer.Draw(this.Position.X, 17, new string('*', Constants.MenuWidth - 5), Theme.Light, false);
            ScreenBuffer.Draw(this.Position.X + 4, 18, "<     - Left", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 19, ">     - Right", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 20, "^     - Up", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 21, "v     - Down", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 2, 22, "Space   - Shoot", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 23, "B     - Bomb", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 24, "P     - Pause", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 4, 25, "ESC   - Exit", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X, 26, new string('*', Constants.MenuWidth - 5), Theme.Light, false);

            // Statistics block
            ScreenBuffer.Draw(this.Position.X, 28, "Statistics:", Theme.Blue, false);
            for (int k = 30; k < 38; k++)
            {
                ScreenBuffer.Draw(this.Position.X, k, "*", Theme.Light, false);
                ScreenBuffer.Draw(this.Position.X + Constants.MenuWidth - 6, k, "*", Theme.Light, false);
            }

            ScreenBuffer.Draw(this.Position.X, 29, new string('*', Constants.MenuWidth - 5), Theme.Light, false);
            ScreenBuffer.Draw(this.Position.X + 2, 30, "Games played:", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 2, 31, "Total time:", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 2, 32, "High scores:", Theme.Contrast, false);
            ScreenBuffer.Draw(this.Position.X + 2, 33, "------------", Theme.Light, false);
            ScreenBuffer.Draw(this.Position.X, 38, new string('*', Constants.MenuWidth - 5), Theme.Light, false);

            ScreenBuffer.DrawScreen();
        }

        public void DrawInfo()
        {
            this.DrawInfo(
                this.Engine.Scores.Lives,
                this.Engine.Scores.Score,
                this.Engine.GameLevel,
                this.Engine.GameCounter,
                this.Engine.Scores.HighestScore,
                this.Engine.Timer);
        }

        private void DrawInfo(int lives, int score, GameLevelType gameLevel, int gamesCounter, int highestScore, TimeSpan timer)
        {
            ScreenBuffer.Draw(this.Position.X + 11, 9, string.Format("{0}", this.Engine.Player.Name), Theme.Green, true);
            ScreenBuffer.Draw(this.Position.X + 16, 11, string.Format("{0,3}", lives), ConsoleColor.Red, true);
            ScreenBuffer.Draw(this.Position.X + 12, 12, string.Format("{0,7}", score), ConsoleColor.Red, true);
            ScreenBuffer.Draw(this.Position.X + 12, 13, string.Format("{0,7}", gameLevel), ConsoleColor.DarkGreen, true);
            ScreenBuffer.Draw(this.Position.X + 16, 30, string.Format("{0,5}", gamesCounter), ConsoleColor.DarkGreen, true);
            ScreenBuffer.Draw(this.Position.X + 16, 31, string.Format("{0,2}:{1:00}", timer.Minutes, timer.Seconds), ConsoleColor.DarkGreen, true);
            ScreenBuffer.Draw(this.Position.X + 2, 34, string.Format("{0}", this.Engine.Player.Name), ConsoleColor.DarkGreen, true);
            ScreenBuffer.Draw(this.Position.X + 15, 34, string.Format("{0,6}", highestScore), ConsoleColor.DarkGreen, true);
        }
    }
}
