using EasySaveGUI.model;

namespace EasySaveGUI.Tests {
    [TestClass]
    public class FullSaveTests {

        [TestMethod]
        public void TestIsToSave_AlwaysReturnsTrue() {
            // Arrange
            string name = "TestJob";
            string sourceFolder = @"C:\Source\";
            string destinationFolder = @"C:\Destination\";

            // Create a new instance of FullSave
            FullSave fullSave = new FullSave(name, sourceFolder, destinationFolder);

            // Act
            bool result = fullSave.IsToSave("anyfilepath.txt");

            // Assert
            Assert.IsTrue(result);
        }
    }
}