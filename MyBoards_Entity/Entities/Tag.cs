namespace MyBoards_Entity.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public virtual List<WorkItem> WorkItems { get; set; }
        public string Category { get; set; }
    }
}
