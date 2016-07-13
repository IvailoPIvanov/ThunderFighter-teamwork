namespace ThunderFighter.Models.Screens.Abstract
{    
    using ThunderFighter.Contracts;
    using ThunderFighter.Controls;

    public abstract class Screen : IScreen
    {
        private bool isShown;

        public bool IsShown
        {
            get
            {
                return this.isShown;
            }
        }

        public void Show()
        {
            if (!this.isShown)
            {
                ConsoleKeyboardHandler.Instance.KeyDown += this.Instance_KeyDown;

                this.ShowOverride();
                this.isShown = true;
            }
        }

        public void Hide()
        {
            if (this.isShown)
            {
                ConsoleKeyboardHandler.Instance.KeyDown -= this.Instance_KeyDown;

                this.HideOverride();
                this.isShown = false;
            }
        }

        protected abstract void ShowOverride();

        protected abstract void HideOverride();

        protected abstract void HandleKeyDown(ConsoleKeyDownEventArgs args);

        private void Instance_KeyDown(object sender, ConsoleKeyDownEventArgs e)
        {
            this.HandleKeyDown(e);
        }
    }
}