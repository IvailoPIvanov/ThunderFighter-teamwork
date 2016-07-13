namespace ThunderFighter
{
    using System;
    using System.Runtime.InteropServices;
    using ThunderFighter.Common.Constants;
    using ThunderFighter.Models.Common;
    
    public class Field
    {
        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;

        private int width;
        private int height;

        private int playWidth;
        private int playHeight;

        private int menuWidth;
        private int menuHeight;

        public Field(Theme theme, int windowWidth = 60, int windowHeight = 60)
        {
            this.Theme = theme;

            this.Width = Math.Min(Console.LargestWindowWidth - Constants.MenuWidth, windowWidth);
            this.Height = Math.Min(Console.LargestWindowHeight, windowHeight);

            this.MenuHeight = this.Height;
            this.MenuWidth = Constants.MenuWidth;

            this.PlayWidth = this.Width - this.MenuWidth;
            this.PlayHeight = this.Height;

            // The code moves the console window to the top left of your screen
            SetWindowPos(GetConsoleWindow(), 0, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOSIZE | SWP_NOZORDER);
            
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            
            Console.WindowWidth = this.Width;
            Console.WindowHeight = this.Height;
            Console.BufferWidth = this.Width;
            Console.BufferHeight = this.Height;

            Console.Title = "ThunderFighter by Team Hestia";
            Console.BackgroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Console.Clear();
        }

        public Theme Theme { get; set; }

        public int Width
        {
            get
            {
                return this.width;
            }

            private set
            {
                this.width = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }

            private set
            {
                this.height = value;
            }
        }

        public int PlayWidth
        {
            get
            {
                return this.playWidth;
            }

            private set
            {
                this.playWidth = value;
            }
        }

        public int PlayHeight
        {
            get
            {
                return this.playHeight;
            }

            private set
            {
                this.playHeight = value;
            }
        }

        public int MenuWidth
        {
            get
            {
                return this.menuWidth;
            }

            private set
            {
                this.menuWidth = value;
            }
        }

        public int MenuHeight
        {
            get
            {
                return this.menuHeight;
            }

            private set
            {
                this.menuHeight = value;
            }
        }

        public Point2D Center
        {
            get
            {
                return new Point2D(this.PlayWidth / 2, this.PlayHeight / 2);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(
            IntPtr hWnd,
            int handleWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint wFlags);
    }
}
