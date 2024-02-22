using EasySaveGUI.model;

namespace EasySaveGUI.Tests {
    [TestClass]
    public class JobStateTests {

        [TestMethod]
        public void TestJobStateProperties() {
            // Arrange
            string name = "TestJob";
            string sourceFile = @"C:\Source\";
            string destinationFile = @"C:\Destination\";
            string state = "In Progress";
            uint totalFiles = 10;

            // Act
            JobState jobState = new JobState(name, sourceFile, destinationFile, state, totalFiles);

            // Assert
            Assert.AreEqual<string>(name, jobState.Name);
            Assert.AreEqual<string>(sourceFile, jobState.SourceFile);
            Assert.AreEqual<string>(destinationFile, jobState.DestinationFile);
            Assert.AreEqual<string>(state, jobState.State);
            Assert.AreEqual<uint>(totalFiles, jobState.TotalFiles);
            Assert.AreEqual<uint>(totalFiles, jobState.FilesLeft); // FilesLeft should be equal to TotalFiles initially
            Assert.AreEqual<ulong>(0, jobState.TotalFilesSize);
            Assert.AreEqual<ulong>(0, jobState.FilesSizeLeft);
            Assert.AreEqual<int>(0, jobState.Progression);
        }

        [TestMethod]
        public void TestFinishJobState() {
            // Arrange
            JobState jobState = new JobState();

            // Act
            jobState.FinishJobState();

            // Assert
            Assert.AreEqual<string>("END", jobState.State);
            Assert.AreEqual<uint>(0, jobState.TotalFiles);
            Assert.AreEqual<ulong>(0ul, jobState.TotalFilesSize);
            Assert.AreEqual<uint>(0, jobState.FilesLeft);
            Assert.AreEqual<ulong>(0ul, jobState.FilesSizeLeft);
            Assert.AreEqual<int>(0, jobState.Progression);
            Assert.AreEqual<string>("", jobState.SourceFile);
            Assert.AreEqual<string>("", jobState.DestinationFile);
        }
    }
}