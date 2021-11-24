using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAthorsTest.Test
{
    [TestClass]
    public class FirstLetterUppercaseAttributeTest
    {
        [TestMethod]
        public void FirstLetterLowerCase_ReturnError()
        {
            //Preparation
            var firstLetterUppercase = new FirstLetterUppercaseAttribute();
            var valueTest = "cris";
            var valueContext = new ValidationContext(new { Name = valueTest });

            //Execution
            var resultTest = firstLetterUppercase.GetValidationResult(valueTest, valueContext);

            //Verification
            Assert.AreEqual("La primera letra deber mayuscula", resultTest.ErrorMessage);
        }

        [TestMethod]
        public void NullValue_NoReturnError()
        {
            //Preparation
            var firstLetterUppercase = new FirstLetterUppercaseAttribute();
            string valueTest = null;
            var valueContext = new ValidationContext(new { Name = valueTest });

            //Execution
            var resultTest = firstLetterUppercase.GetValidationResult(valueTest, valueContext);

            //Verification
            Assert.IsNull(resultTest);
        }
        
        [TestMethod]
        public void FirstLetterUpperCase_NoReturnError()
        {
            //Preparation
            var firstLetterUppercase = new FirstLetterUppercaseAttribute();
            string valueTest = "Cris";
            var valueContext = new ValidationContext(new { Name = valueTest });

            //Execution
            var resultTest = firstLetterUppercase.GetValidationResult(valueTest, valueContext);

            //Verification
            Assert.IsNull(resultTest);
        }
    }
}
