namespace ThunderFighter.Controls
{
    using System;

    public class ConsoleKeyboardHandler
    {
        private static readonly ConsoleKeyboardHandler instance;

        static ConsoleKeyboardHandler()
        {
            ConsoleKeyboardHandler.instance = new ConsoleKeyboardHandler();
        }

        public ConsoleKeyboardHandler()
        {
        }

        public event EventHandler<ConsoleKeyDownEventArgs> KeyDown;

        public static ConsoleKeyboardHandler Instance
        {
            get
            {
                return ConsoleKeyboardHandler.instance;
            }
        }

        public void HandleKeys()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                this.OnKeyDown(new ConsoleKeyDownEventArgs(keyInfo));
            }
        }

        protected virtual void OnKeyDown(ConsoleKeyDownEventArgs args)
        {
            this.KeyDown.Invoke(this, args);
        }
    }
}