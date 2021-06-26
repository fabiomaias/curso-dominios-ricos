using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.ValueObjects
{
    [TestClass]
    public class DocumentTests
    {
        //Red, Green, Refactor
        [TestMethod]
        public void ShouldReturnErrorWHenCNPJIsInvalid()
        {
            var document = new Document("123", EDocumentType.CNPJ);
            Assert.IsFalse(document.IsValid);
        }

        [TestMethod]
        public void ShouldReturnErrorWHenCNPJIsValid()
        {
            var document = new Document("34110468000150", EDocumentType.CNPJ);
            Assert.IsTrue(document.IsValid);
        }

        [TestMethod]
        public void ShouldReturnErrorWHenCPFIsInvalid()
        {
            var document = new Document("123", EDocumentType.CPF);
            Assert.IsFalse(document.IsValid);
        }

        [DataTestMethod]
        [DataRow("53164907036")]
        [DataRow("85878090082")]
        [DataRow("58763740087")]
        [TestMethod]
        public void ShouldReturnErrorWHenCPFIsValid(string cpf)
        {
            var document = new Document(cpf, EDocumentType.CPF);
            Assert.IsTrue(document.IsValid);
        }
        
    }
}