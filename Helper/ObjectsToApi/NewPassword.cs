namespace Helper.ObjectsToApi
{
    public class NewPassword
    {
        public string? OldPasswordHash { get; set; }
        public string? NewPasswordHash { get; set; }
        public int PersonId { get; set; }
    }
}
