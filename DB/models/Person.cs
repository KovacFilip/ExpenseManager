namespace DB.models
{
    public class Person
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Roles Role { get; set; }
    }
}
