using System;
using Xunit;
using FluentAssertions;
using LibGit2Sharp;
using Moq;
using System.Collections.Generic;

namespace CreativityReportGenerator.Services.Tests
{
    public class CreativityReportGeneratorServiceTests
    {
        private const int StartWorkingHours = 22;

        private const int EndWorkingHours = 10;

        [Fact]
        public void CalculateCreativeTime_IfMergeCommit_ReturnZero()
        {
            var expectedResult = 0;
            var firstCommitMock = new Mock<Commit>();
            var secondCommitMock = new Mock<Commit>();
            var commits = new List<Commit>() { firstCommitMock.Object, secondCommitMock.Object };

            var commitMock = new Mock<Commit>();
            commitMock.Setup(m => m.Parents).Returns(commits); 
            
            var tc = CreateTestCandidate();

            var result = tc.CalculateCreativeTime(commitMock.Object, null, StartWorkingHours, EndWorkingHours);

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void CalculateCreativeTime_IfPreviousCommitIsNull_ReturnHalfWorkingHours()
        {
            var expectedResult = 6;

            var firstCommitMock = new Mock<Commit>();
            var commits = new List<Commit>() { firstCommitMock.Object };

            var commitMock = new Mock<Commit>();
            commitMock.Setup(m => m.Parents).Returns(commits);

            var tc = CreateTestCandidate();

            var result = tc.CalculateCreativeTime(commitMock.Object, null, StartWorkingHours, EndWorkingHours);

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void CalculateCreativeTime_IfPreviousCommitExists_ReturnTimeDifferenceBetweenCommits()
        {
            var expectedResult = 3;

            const string name = "name";
            const string email = "email";

            var previousCommitTime = new DateTime(2022, 2, 7, 23, 0, 0);
            var fakeConfigForPreviousCommit = new Mock<Configuration>();
            fakeConfigForPreviousCommit.Setup(c => c.BuildSignature(previousCommitTime))
                      .Returns<DateTimeOffset>(t => new Signature(name, email, t));
            var signaturePreviousCommitMock = fakeConfigForPreviousCommit.Object.BuildSignature(previousCommitTime);
            var previousCommitMock = new Mock<Commit>();
            previousCommitMock.Setup(m => m.Author).Returns(signaturePreviousCommitMock);
            var commits = new List<Commit>() { previousCommitMock.Object };

            var currentCommitTime = new DateTime(2022, 2, 8, 5, 0, 0);
            var fakeConfigForCurrentCommit = new Mock<Configuration>();
            fakeConfigForCurrentCommit.Setup(c => c.BuildSignature(currentCommitTime))
                      .Returns<DateTimeOffset>(t => new Signature(name, email, t));
            var signatureCurrentCommitMock = fakeConfigForCurrentCommit.Object.BuildSignature(currentCommitTime);
            var currentCommitMock = new Mock<Commit>();
            currentCommitMock.Setup(m => m.Parents).Returns(commits);
            currentCommitMock.Setup(m => m.Author).Returns(signatureCurrentCommitMock);

            var tc = CreateTestCandidate();

            var result = tc.CalculateCreativeTime(currentCommitMock.Object, previousCommitMock.Object, StartWorkingHours, EndWorkingHours);

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void CalculateCreativeTime_IfPreviousCommitExistsAndNotAllTimeBetweenCommitsWasWorkingHours_ReturnTimeDifferenceBetweenCommits()
        {
            var expectedResult = 7;

            const string name = "name";
            const string email = "email";

            var previousCommitTime = new DateTime(2022, 2, 7, 21, 0, 0);
            var fakeConfigForPreviousCommit = new Mock<Configuration>();
            fakeConfigForPreviousCommit.Setup(c => c.BuildSignature(previousCommitTime))
                      .Returns<DateTimeOffset>(t => new Signature(name, email, t));
            var signaturePreviousCommitMock = fakeConfigForPreviousCommit.Object.BuildSignature(previousCommitTime);
            var previousCommitMock = new Mock<Commit>();
            previousCommitMock.Setup(m => m.Author).Returns(signaturePreviousCommitMock);
            var commits = new List<Commit>() { previousCommitMock.Object };

            var currentCommitTime = new DateTime(2022, 2, 8, 23, 0, 0);
            var fakeConfigForCurrentCommit = new Mock<Configuration>();
            fakeConfigForCurrentCommit.Setup(c => c.BuildSignature(currentCommitTime))
                      .Returns<DateTimeOffset>(t => new Signature(name, email, t));
            var signatureCurrentCommitMock = fakeConfigForCurrentCommit.Object.BuildSignature(currentCommitTime);
            var currentCommitMock = new Mock<Commit>();
            currentCommitMock.Setup(m => m.Parents).Returns(commits);
            currentCommitMock.Setup(m => m.Author).Returns(signatureCurrentCommitMock);

            var tc = CreateTestCandidate();

            var result = tc.CalculateCreativeTime(currentCommitMock.Object, previousCommitMock.Object, StartWorkingHours, EndWorkingHours);

            result.Should().Be(expectedResult);
        }

        private LocalCreativityReportGenaratorService CreateTestCandidate() =>
            new LocalCreativityReportGenaratorService();
    }
}
