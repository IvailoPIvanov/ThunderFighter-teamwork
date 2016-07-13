namespace ThunderFighter.Controls
{
    using System;

    public class ConsoleKeyDownEventArgs : EventArgs
    {
        private readonly ConsoleKeyInfo keyInfo;

        public ConsoleKeyDownEventArgs(ConsoleKeyInfo keyInfo)
        {
            this.keyInfo = keyInfo;
        }

        public ConsoleKeyInfo KeyInfo
        {
            get
            {
                return this.keyInfo;
            }
        }
    }
}