namespace ThunderFighter.Contracts
{
    public interface IScreen
    {
        bool IsShown { get; }

        void Show();

        void Hide();
    }
}