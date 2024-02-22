using EasySaveGUI.model;

namespace EasySaveGUI.Tests {
    [TestClass]
    public class JobLogTests {

        [TestMethod]
        public void TestJobLogProperties() {
            // Arrange
            string name = "TestJob";
            string sourceFile = @"C:\Source\testfile.txt";
            string destinationFile = @"C:\Destination\testfile.txt";
            ulong fileSize = 1024; // 1 KB
            double transferTime = 5.3; // 5.3 seconds

            // Act
            JobLog jobLog = new(name, sourceFile, destinationFile, fileSize, transferTime);

            // Assert
            Assert.AreEqual(name, jobLog.Name);
            Assert.AreEqual(sourceFile, jobLog.SourceFile);
            Assert.AreEqual(destinationFile, jobLog.DestinationFile);
            Assert.AreEqual(fileSize, jobLog.FileSize);
            Assert.AreEqual(transferTime, jobLog.TransferTime);
            // Timestamp is dynamically generated, so it cannot be directly asserted
            Assert.IsNotNull(jobLog.Timestamp);
        }
    }
}