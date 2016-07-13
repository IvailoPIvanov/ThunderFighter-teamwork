namespace ThunderFighter.Models.Screens
{    
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Controls;
    using ThunderFighter.Models.Screens.Abstract;

    internal class HighScoreScreen : Screen
    {
        private readonly Engine engine;
        private readonly MessageBox messageBox;

        public HighScoreScreen(Engine engine)
        {
            this.engine = engine;
            this.messageBox = new MessageBox(
                this.engine.Field,
                "CONGRATULATIONS!\nYou hit a new high score: " + this.engine.Scores.HighestScore,
                MessageBoxPositionType.Lower,
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
        }
    }
}