using EasySaveGUI.model;

namespace EasySaveGUI.Tests {
    [TestClass]
    public class LogSystemTests {

        [TestMethod]
        public void LogSystem_ConstructsCorrectly() {
            // Arrange
            string name = "TestJob";
            string sourceFile = "C:\\Source\\test.txt";
            string destinationFile = "D:\\Destination\\test.txt";

            // Act
            LogSystem logSystem = new TestLogSystem(name, sourceFile, destinationFile);

            // Assert
            Assert.AreEqual(name, logSystem.Name);
            Assert.AreEqual(sourceFile, logSystem.SourceFile);
            Assert.AreEqual(destinationFile, logSystem.DestinationFile);
        }

        // Mock class for testing LogSystem
        public class TestLogSystem : LogSystem {
            public TestLogSystem(string name, string sourceFile, string destinationFile) : base(name, sourceFile, destinationFile) {
            }
        }
    }
}