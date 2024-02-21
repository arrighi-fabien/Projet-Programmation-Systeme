using EasySaveGUI.model;
using System.IO;

namespace EasySaveGUI.Tests {
    [TestClass]
    public class SaveJobTests {
        [TestMethod]
        public void GetFileList_ReturnsCorrectFileCount() {
            // Arrange
            string testFolderPath = "C:\\TestFolder";
            Directory.CreateDirectory(testFolderPath);
            File.Create(Path.Combine(testFolderPath, "testfile1.txt")).Close();
            File.Create(Path.Combine(testFolderPath, "testfile2.txt")).Close();
            File.Create(Path.Combine(testFolderPath, "testfile3.txt")).Close();
            Directory.CreateDirectory(Path.Combine(testFolderPath, "Subfolder"));
            File.Create(Path.Combine(testFolderPath, "Subfolder", "testfile4.txt")).Close();

            SaveJob saveJob = new MockSaveJob();

            // Act
            List<string> files = new List<string>();
            saveJob.GetFileList(testFolderPath, files);

            // Assert
            // Expected count should be 4 because we have 3 files and 1 subfolder
            Assert.AreEqual(4, files.Count);

            // Clean up
            Directory.Delete(testFolderPath, true);
        }

        // Mock class to test abstract methods of SaveJob
        public class MockSaveJob : SaveJob {
            public MockSaveJob() : base("", "", "") { }

            public override bool IsToSave(string path) {
                return true;
            }
        }
    }
}
