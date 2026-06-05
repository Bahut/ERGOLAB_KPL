using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TUBES_KPL;

namespace ERGOLAB_KPL.Tests
{
    [TestClass]
    public class InputValidatorAndFormatterTests
    {
        private InputValidatorAndFormatter _helper;

        [TestInitialize]
        public void Setup()
        {
            _helper = new InputValidatorAndFormatter();
        }

        [TestMethod]
        public void Test_ValidateNoTelp_Valid()
        {
            Assert.IsTrue(_helper.ValidateNoTelp("081234567890"));
        }

        [TestMethod]
        public void Test_ValidateNIK_Valid()
        {
            Assert.IsTrue(_helper.ValidateNIK("3273012345678901"));
        }

        [TestMethod]
        public void Test_SamarkanNama_EmptyInput_ThrowsException()
        {
            try
            {
                _helper.SamarkanNamaPelapor("");

                Assert.Fail("Harusnya melempar ArgumentException tetapi tidak.");
            }
            catch (ArgumentException)
            {
                return;
            }
        }
    }
}