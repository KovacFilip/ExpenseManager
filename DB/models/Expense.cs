namespace DB.models
{
    public class Expense
    {
        public int ObjectId { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int PersonId { get; set; }
    }
}
