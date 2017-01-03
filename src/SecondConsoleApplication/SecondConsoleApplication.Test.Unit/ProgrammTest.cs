using NUnit.Framework;

namespace SecondConsoleApplication.Test.Unit
{

    [TestFixture]
    public class ProgrammTest
    {
        [Test]
        public void GetName_NameIsNotEmpty()
        {
            Assert.IsNotNullOrEmpty(Program.GetName());
        }
    }
}
