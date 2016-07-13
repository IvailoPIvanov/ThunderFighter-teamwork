namespace ThunderFighter.Models.Screens
{
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Controls;    
    using ThunderFighter.Models.Screens.Abstract;

    internal class GameOverScreen : Screen
    {
        private readonly Engine engine;
        private readonly MessageBox messageBox;

        public GameOverScreen(Engine engine)
        {
            this.engine = engine;

            this.messageBox = new MessageBox(
                this.engine.Field,
                "GAME OVER!\nPress any key to continue...",
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
            this.Hide();
            this.engine.GameStatus = GameStatusType.Welcome;

            ScreenBuffer.DrawScreen();
        }
    }
}