namespace CreativityReportGenerator.Core.Models
{
    /// <summary>
    /// Creativity report item.
    /// </summary>
    public class CreativityReportItem
    {
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the commit Id.
        /// </summary>
        /// <value>
        /// The commit Id.
        /// </value>
        public string CommitId { get; set; }

        /// <summary>
        /// Gets or sets the comment for commit.
        /// </summary>
        /// <value>
        /// The comment to commit.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the commit's author.
        /// </summary>
        /// <value>
        /// The commit's author.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets time of hours spent on this commit.
        /// </summary>
        /// <value>
        /// Time of hours spent on this commit.
        /// </value>
        public int Hours { get; set; }
    }
}
