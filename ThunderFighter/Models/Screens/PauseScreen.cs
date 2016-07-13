namespace ThunderFighter.Models.Screens
{
    using System;    
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Controls;
    using ThunderFighter.Models.Screens.Abstract;

    internal class PauseScreen : Screen
    {
        private readonly Engine engine;
        private readonly MessageBox messageBox;
        private DateTime timeBeforePause;

        public PauseScreen(Engine engine)
        {
            this.engine = engine;

            this.messageBox = new MessageBox(
                this.engine.Field,
                "PAUSE!\nPress any key to continue...",
                MessageBoxPositionType.Center,
                MessageBoxTextAlignmentType.Center);
        }

        protected override void ShowOverride()
        {
            this.timeBeforePause = DateTime.Now;
            this.messageBox.Draw(true);
        }

        protected override void HideOverride()
        {
            this.messageBox.Clear();
        }

        protected override void HandleKeyDown(ConsoleKeyDownEventArgs args)
        {
            this.Hide();
            this.engine.GameStatus = GameStatusType.Play;
            this.engine.StartTime += DateTime.Now - this.timeBeforePause;

            ScreenBuffer.DrawScreen();
        }
    }
}
