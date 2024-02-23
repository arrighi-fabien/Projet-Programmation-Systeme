using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using EasySaveGUI.model;

namespace EasySaveGUI.Tests {
    [TestClass]
    public class ToolTests {
        private Tool tool;

        [TestInitialize]
        public void Setup() {
            tool = Tool.GetInstance();
        }

        [TestMethod]
        public void TestSavedSaveJob_WhenSavejobsExists_ReturnsListOfSaveJobs() {
            // Arrange
            string jsonContent = "[{\"Name\":\"Job1\",\"SourceFolder\":\"C:\\\\Source\",\"DestinationFolder\":\"C:\\\\Destination\",\"type\":\"full\"}]";
            File.WriteAllText("config/savejobs.json", jsonContent);

            // Act
            List<SaveJob> saveJobs = tool.GetSavedSaveJob();

            // Assert
            Assert.IsNotNull(saveJobs);
            Assert.AreEqual(1, saveJobs.Count);
            Assert.AreEqual("Job1", saveJobs[0].Name);
            Assert.AreEqual("C:\\Source", saveJobs[0].SourceFolder);
            Assert.AreEqual("C:\\Destination", saveJobs[0].DestinationFolder);

            // Clean up
            File.Delete("config/savejobs.json");
        }

        [TestMethod]
        public void TestSavedSaveJob_WhenSavejobsDoesNotExist_ReturnsEmptyList() {
            // Act
            List<SaveJob> saveJobs = tool.GetSavedSaveJob();

            // Assert
            Assert.IsNotNull(saveJobs);
            Assert.AreEqual(0, saveJobs.Count);
        }

        [TestMethod]
        public void TestFileSize_WhenFileExists_ReturnsFileSize() {
            // Arrange
            string filePath = "testfile.txt";
            string fileContent = "This is a test file.";
            File.WriteAllText(filePath, fileContent);

            // Act
            ulong fileSize = tool.GetFileSize(filePath);

            // Assert
            Assert.AreEqual((ulong)fileContent.Length, fileSize);

            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void TestFileSize_WhenFileDoesNotExist_ReturnsZero() {
            // Act
            ulong fileSize = tool.GetFileSize("nonexistentfile.txt");

            // Assert
            Assert.AreEqual((ulong)0, fileSize);
        }

        [TestMethod]
        public void TestPathIsValid_WhenPathIsValid_ReturnsTrue() {
            // Act
            bool isValid = tool.PathDirectoryIsValid(@"C:\Valid\Path");

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TestPathIsValid_WhenPathIsInvalid_ReturnsFalse() {
            // Act
            bool isValid = tool.PathDirectoryIsValid(@"Invalid?Path");

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestCleanup]
        public void Cleanup() {
            // Clean up any resources here if needed
        }
    }
}
