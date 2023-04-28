namespace DB.models
{
    public class PasswordHash
    {
        public int PersonId { get; set; }
        public string Hash { get; set; }
    }
}
