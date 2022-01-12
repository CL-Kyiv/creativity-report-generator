
using System;
using System.Diagnostics.CodeAnalysis;

namespace CreativityReportGenerator.Core.Models
{
    public class Author : IEquatable<Author>
    {
        public string Name { get; set; }
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
