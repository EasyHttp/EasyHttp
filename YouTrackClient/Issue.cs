namespace YouTrackClient
{
    public class Issue
    {
        public string Project { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Assignee { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string Subsystem { get; set; }
        public string State { get; set; }
    }
}