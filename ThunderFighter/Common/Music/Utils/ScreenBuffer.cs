namespace ThunderFighter.Common.Utils
{
    using System;
    using System.Runtime.InteropServices;

    public static class ScreenBuffer
    {
        private const uint STD_OUTPUT_HANDLE = unchecked((uint)(-11));

        private static int cols;
        private static int rows;
        private static CHAR_INFO[] screenBuf; // screen buffer array
        private static CHAR_INFO[] screenBufCopy; // copy of screen buffer array
        private static IntPtr handle;
        private static COORD bufferSize;
        private static COORD bufferCoord;
        private static SMALL_RECT rect;
        private static ConsoleColor defaultForegroundColor;
        private static ConsoleColor defaultBackgroundColor;

        /// <summary>
        /// CHAR_ATTRIBUTES native structure.
        /// </summary>
        [Flags]
        public enum Attr : ushort
        {
            NO_ATTRIBUTES = 0x0000,

            /// <summary>
            /// Text color contains blue.
            /// </summary>
            FOREGROUND_BLUE = 0x0001,

            /// <summary>
            /// Text color contains green.
            /// </summary>
            FOREGROUND_GREEN = 0x0002,

            /// <summary>
            /// Text color contains red.
            /// </summary>
            FOREGROUND_RED = 0x0004,

            /// <summary>
            /// Text color is intensified.
            /// </summary>
            FOREGROUND_INTENSITY = 0x0008,

            /// <summary>
            /// Background color contains blue.
            /// </summary>
            BACKGROUND_BLUE = 0x0010,

            /// <summary>
            /// Background color contains green.
            /// </summary>
            BACKGROUND_GREEN = 0x0020,

            /// <summary>
            /// Background color contains red.
            /// </summary>
            BACKGROUND_RED = 0x0040,

            /// <summary>
            /// Background color is intensified.
            /// </summary>
            BACKGROUND_INTENSITY = 0x0080,

            /// <summary>
            /// Leading byte.
            /// </summary>
            COMMON_LVB_LEADING_BYTE = 0x0100,

            /// <summary>
            /// Trailing byte.
            /// </summary>
            COMMON_LVB_TRAILING_BYTE = 0x0200,

            /// <summary>
            /// Top horizontal
            /// </summary>
            COMMON_LVB_GRID_HORIZONTAL = 0x0400,

            /// <summary>
            /// Left vertical.
            /// </summary>
            COMMON_LVB_GRID_LVERTICAL = 0x0800,

            /// <summary>
            /// Right vertical.
            /// </summary>
            COMMON_LVB_GRID_RVERTICAL = 0x1000,

            /// <summary>
            /// Reverse foreground and background attribute.
            /// </summary>
            COMMON_LVB_REVERSE_VIDEO = 0x4000,

            /// <summary>
            /// Underscore.
            /// </summary>
            COMMON_LVB_UNDERSCORE = 0x8000
        }

        public static void Initialize(
            int cols,
            int rows,
            ConsoleColor defaultForegroundColor,
            ConsoleColor defaultBackgroundColor)
        {
            ScreenBuffer.cols = cols;
            ScreenBuffer.rows = rows;
            ScreenBuffer.defaultForegroundColor = defaultForegroundColor;
            ScreenBuffer.defaultBackgroundColor = defaultBackgroundColor;

            ScreenBuffer.screenBuf = new CHAR_INFO[rows * cols];
            ScreenBuffer.screenBufCopy = new CHAR_INFO[rows * cols];

            ScreenBuffer.bufferSize = new COORD() { X = (short)cols, Y = (short)rows };
            ScreenBuffer.bufferCoord = new COORD() { X = 0, Y = 0 };
            ScreenBuffer.rect = new SMALL_RECT() { Left = 0, Top = 0, Right = (short)cols, Bottom = (short)rows };

            ScreenBuffer.handle = GetStdHandle(STD_OUTPUT_HANDLE);

            ScreenBuffer.ClearScreen();
        }

        public static void Clear(int x, int y, bool clearForced)
        {
            ushort color = (ushort)(((int)ScreenBuffer.defaultBackgroundColor) << 4);

            ScreenBuffer.screenBuf[(y * ScreenBuffer.cols) + x].UnicodeChar = ' ';
            ScreenBuffer.screenBuf[(y * ScreenBuffer.cols) + x].Attributes = color;

            if (clearForced)
            {
                ScreenBuffer.DrawRectangle(x, y, x, y);
            }
        }

        public static void Clear(int x, int y, string text, bool clearForced)
        {
            int posX = x;
            int posY = y;
            int carry;
            int i = 0;
            int j = (y * ScreenBuffer.cols) + x;

            while (i < text.Length && j < ScreenBuffer.screenBuf.Length)
            {
                ScreenBuffer.Clear(x, y, false);

                x = (x + 1) % ScreenBuffer.cols;
                carry = (x + 1) / ScreenBuffer.cols;
                y = y + carry;

                i++;
                j++;
            }

            if (clearForced)
            {
                ScreenBuffer.DrawRectangle(posX, posY, posX + text.Length, posY);
            }
        }

        public static void Draw(int x, int y, char symbol, ConsoleColor foregroundColor, bool drawForced)
        {
            ushort color = ScreenBuffer.CombineColors(foregroundColor, ScreenBuffer.defaultBackgroundColor);

            ScreenBuffer.screenBuf[(y * ScreenBuffer.cols) + x].UnicodeChar = symbol;
            ScreenBuffer.screenBuf[(y * ScreenBuffer.cols) + x].Attributes = color;

            if (drawForced)
            {
                ScreenBuffer.DrawRectangle(x, y, x, y);
            }
        }

        public static void Draw(int x, int y, string text, ConsoleColor foregroundColor, bool drawForced)
        {
            int posX = x;
            int posY = y;
            int carry;
            int i = 0;
            int j = (y * ScreenBuffer.cols) + x;

            while (i < text.Length && j < ScreenBuffer.screenBuf.Length)
            {
                ScreenBuffer.Draw(x, y, text[i], foregroundColor, false);

                x = (x + 1) % ScreenBuffer.cols;
                carry = (x + 1) / ScreenBuffer.cols;
                y = y + carry;

                i++;
                j++;
            }

            if (drawForced)
            {
                ScreenBuffer.DrawRectangle(posX, posY, posX + text.Length, posY);
            }
        }

        public static void ClearScreen()
        {
            // Clear buffer
            for (int y = 0; y < ScreenBuffer.rows; ++y)
            {
                for (int x = 0; x < ScreenBuffer.cols; ++x)
                {
                    ScreenBuffer.Clear(x, y, false);
                }
            }

            ScreenBuffer.DrawScreen();
        }

        public static void DrawScreen()
        {
            if (!AreScreenBufferArraysEqual())
            {
                // note that the screen is NOT cleared at any point as this will simply 
                // overwrite the existing values on screen. Clearing will cause flickering again.
                bool b = WriteConsoleOutput(
                    ScreenBuffer.handle,
                    ScreenBuffer.screenBuf,
                    ScreenBuffer.bufferSize,
                    ScreenBuffer.bufferCoord,
                    ref ScreenBuffer.rect);

                Array.Copy(ScreenBuffer.screenBuf, ScreenBuffer.screenBufCopy, ScreenBuffer.screenBuf.Length);
            }
        }

        public static void DrawRectangle(int left, int top, int right, int bottom)
        {
            if (!AreScreenBufferArraysEqual(left, top, right, bottom))
            {
                int rows = bottom - top + 1;
                int cols = right - left + 1;

                CHAR_INFO[] rectBuf = new CHAR_INFO[rows * cols];

                int i = 0;
                for (int row = top; row <= bottom; row++)
                {
                    for (int col = left; col <= right; col++)
                    {
                        rectBuf[i++] = ScreenBuffer.screenBuf[(row * ScreenBuffer.cols) + col];
                    }
                }

                COORD rectBufSize = new COORD() { X = (short)cols, Y = (short)rows };
                COORD rectBufCoord = new COORD() { X = 0, Y = 0 };

                SMALL_RECT rectangle = new SMALL_RECT() { Left = (short)left, Top = (short)top, Right = (short)right, Bottom = (short)bottom };

                // note that the screen is NOT cleared at any point as this will simply 
                // overwrite the existing values on screen. Clearing will cause flickering again.
                bool b = WriteConsoleOutput(
                    ScreenBuffer.handle,
                    rectBuf,
                    rectBufSize,
                    rectBufCoord,
                    ref rectangle);

                Array.Copy(ScreenBuffer.screenBuf, ScreenBuffer.screenBufCopy, ScreenBuffer.screenBuf.Length);
            }
        }

        private static bool AreScreenBufferArraysEqual()
        {
            return AreScreenBufferArraysEqual(0, 0, ScreenBuffer.cols - 1, ScreenBuffer.rows - 1);
        }

        private static bool AreScreenBufferArraysEqual(int left, int top, int right, int bottom)
        {
            int row;
            int col;
            int index;
            int rows = bottom - top + 1;
            int cols = right - left + 1;

            for (row = top; row <= bottom; row++)
            {
                index = (row * ScreenBuffer.cols) + left;

                for (col = left; col <= right; col++)
                {
                    if (ScreenBuffer.screenBuf[index].UnicodeChar != ScreenBuffer.screenBufCopy[index].UnicodeChar ||
                        ScreenBuffer.screenBuf[index].Attributes != ScreenBuffer.screenBufCopy[index].Attributes)
                    {
                        return false;
                    }

                    index++;
                }
            }

            return true;
        }

        private static ushort CombineColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            return (ushort)((int)foregroundColor + (((int)backgroundColor) << 4));
        }

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetStdHandle(uint type);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "WriteConsoleOutputW")]
        private static extern bool WriteConsoleOutput(
            IntPtr handleConsoleOutput,
            CHAR_INFO[] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }

        /// <summary>
        /// CharSet.Unicode is required for proper marshaling.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct CHAR_INFO
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public char AsciiChar;
            [FieldOffset(2)]
            public ushort Attributes; // public Attr Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
    }
}
