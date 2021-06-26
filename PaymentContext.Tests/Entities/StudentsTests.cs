using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {

        private readonly Subscription _subscription;
        private readonly Student _student;
        private readonly Email _email;
        private readonly Document _document;
        private readonly Address _address;
        private readonly Name _name;
        private readonly PayPalPayment _payment;

        public StudentTests()
        {
            _name = new Name("Fábio","Maia");
            _email = new Email("fabiomaias@gmail.com");
            _document = new Document("02503508588", EDocumentType.CPF);
            _student = new Student(_name, _document, _email, _address);
            _address = new Address("Avenida Tal", "21", "Buraquinho", "Lauro de Freitas", "Bahis", "Brasil", "42710400");
            _payment = new PayPalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5),10, 10, "Caio Maia", _document, _address, _email);
            _subscription = new Subscription(null); 
            
        }

        [TestMethod]
        public void ShouldReturnErrorWHenHadActiveSubsciption()
        {
            
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsFalse(_student.IsValid);
        }

        [TestMethod]
        public void ShouldReturnErrorWHenSubsciptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);

            Assert.IsFalse(_student.IsValid);
            
        }

        [TestMethod]
        public void ShouldReturnSuccessHenAddubsciption()
        {
            _subscription.AddPayment(_payment);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.IsValid);
        }
    }
}
