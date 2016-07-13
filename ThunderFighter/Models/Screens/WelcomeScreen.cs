namespace ThunderFighter.Models.Screens
{
    using System;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Controls;    
    using ThunderFighter.Models.Screens.Abstract;

    internal class WelcomeScreen : Screen
    {
        private readonly Engine engine;
        private readonly MessageBox messageBox;

        public WelcomeScreen(Engine engine)
        {
            this.engine = engine;

            this.messageBox = new MessageBox(
                this.engine.Field,
                "Welcome to THUNDER FIGHTER!\nPress ENTER to start or ESC to exit...",
                MessageBoxPositionType.Center,
                MessageBoxTextAlignmentType.Center);
        }

        protected override void ShowOverride()
        {
            this.messageBox.Draw(true);
        }

        protected override void HideOverride()
        {
            this.messageBox.Clear();
        }

        protected override void HandleKeyDown(ConsoleKeyDownEventArgs args)
        {
            if (args.KeyInfo.Key == ConsoleKey.Enter)
            {
                this.Hide();
                this.engine.GameStatus = GameStatusType.Play;
                this.engine.StartTime = DateTime.Now;

                ScreenBuffer.DrawScreen();
            }
            else if (args.KeyInfo.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("\n\nIf you see this it is because you've run the application with a debuger attached! Run the exe file outside Visual Studio.");
                Environment.Exit(0);
            }
        }
    }
}
