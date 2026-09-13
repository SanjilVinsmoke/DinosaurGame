using DinosaurGame.Core;
using NUnit.Framework;

namespace DinosaurGame.Tests
{
    public class AppRuntimeTests
    {
        [Test]
        public void TargetFrameRate_IsMobileBaseline60()
        {
            Assert.AreEqual(60, AppRuntime.TargetFrameRate);
        }
    }
}
