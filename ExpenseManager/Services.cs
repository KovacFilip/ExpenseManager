namespace ExpenseManager
{
    using DB.models;

    public class AuthService
    {
        private bool _isLoggedIn;
        public Person? Person { get; set; }

        public event Action? OnAuthStateChanged;

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnAuthStateChanged?.Invoke();
    }
}
