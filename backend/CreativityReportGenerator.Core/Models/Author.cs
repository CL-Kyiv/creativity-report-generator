using System;

namespace CreativityReportGenerator.Core.Models
{
    /// <summary>
    /// Author.
    /// </summary>
    public class Author : IEquatable<Author>
    {
        /// <summary>
        /// Gets or sets the author name.
        /// </summary>
        /// <value>
        /// The author name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the author Email.
        /// </summary>
        /// <value>
        /// The author Email.
        /// </value>
        public string Email { get; set; }

        public bool Equals(Author other)
        {
            if (Name == other.Name && Email == other.Email)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hashFirstName = Name == null ? 0 : Name.GetHashCode();
            int hashLastName = Email == null ? 0 : Email.GetHashCode();

            return hashFirstName ^ hashLastName;
        }
    }
}
