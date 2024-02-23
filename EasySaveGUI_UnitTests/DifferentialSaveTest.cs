using EasySaveGUI.model;
using System.IO;

namespace EasySaveGUI.Tests {
    [TestClass]
    public class DifferentialSaveTests {

        private string sourceFolder = @"C:\Source\";
        private string destinationFolder = @"C:\Destination\";
        private string filePath = "testFile.txt";

        [TestInitialize]
        public void Init() {
            // Arrange 
            Directory.CreateDirectory(sourceFolder);
            Directory.CreateDirectory(destinationFolder);
            File.WriteAllText(Path.Combine(sourceFolder, filePath), "Content");
        }

        [TestCleanup]
        public void Clean() {
            // Clean up
            File.Delete(Path.Combine(sourceFolder, filePath));
            Directory.Delete(sourceFolder);
            Directory.Delete(destinationFolder, true);
        }

        [TestMethod]
        public void TestNonExistentDestinationFile() {
            // Arrange
            string name = "TestJob";
            DifferentialSave save = new(name, sourceFolder, destinationFolder);

            // Act
            bool result = save.IsToSave(filePath);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDifferentFileSize() {
            // Arrange
            string name = "TestJob";
            string destinationFilePath = Path.Combine(destinationFolder, filePath);
            File.WriteAllText(destinationFilePath, "DifferentContent");
            DifferentialSave save = new(name, sourceFolder, destinationFolder);

            // Act
            bool result = save.IsToSave(filePath);

            // Assert
            Assert.IsTrue(result);

            // Clean up
            File.Delete(destinationFilePath);
        }

        [TestMethod]
        public void TestSameFileSize() {
            // Arrange
            string name = "TestJob";
            string destinationFilePath = Path.Combine(destinationFolder, filePath);
            File.Copy(Path.Combine(sourceFolder, filePath), destinationFilePath);
            DifferentialSave save = new(name, sourceFolder, destinationFolder);

            // Act
            bool result = save.IsToSave(filePath);

            // Assert
            Assert.IsFalse(result);

            // Clean up
            File.Delete(destinationFilePath);
        }
    }
}
