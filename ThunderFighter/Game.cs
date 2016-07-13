namespace ThunderFighter
{
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;

    public static class Game
    {
        public static void Start()
        {
            var theme = new Theme(ThemeColorType.Black);
            var field = new Field(theme, 130, 40);
            ScreenBuffer.Initialize(field.Width, field.Height, Theme.Contrast, Theme.BackGround);
            
            Engine engine = new Engine(field);
            engine.Start();
        }
    }
}       
