namespace CreativityReportGenerator.Core.Models
{
    public class CreativityReportItem
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ProjectName { get; set; }
        public string CommitId { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public int Hours { get; set; }
    }
}
