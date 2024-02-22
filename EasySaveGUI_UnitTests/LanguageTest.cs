using EasySaveGUI.model;

namespace EasySaveGUI.Tests {
    [TestClass]
    public class LanguageTests {

        [TestMethod]
        public void TestMissingStringReturnsPlaceholder() {
            // Arrange
            Language language = new Language();
            string expectedString = "[Missing translation]";

            // Act
            string actualString = language.GetString("NonExistentKey");

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void TestSetLanguageSetsCorrectLanguage() {
            // Arrange
            Language language = new Language();
            string expectedLanguageCode = "fr";

            // Act
            language.SetLanguage(expectedLanguageCode);
        }

    }
}
