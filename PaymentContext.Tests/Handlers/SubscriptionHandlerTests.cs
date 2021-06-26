using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Fábio";
            command.LastName = "Maia";
            command.Document = "99999999999";
            command.Email = "maia@maia.com";
            command.BarCode = "42143213451235";
            command.BoletoNumber = "12525452341235";
            command.PaymentNumber = "23453246";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Fábio Maia";
            command.PayerDocument = "09876567654";
            command.PayerDocumentType = EDocumentType.CPF;   
            command.PayerEmail = "fabiomaia@intranext.com.br";
            command.Street = "gwer";
            command.Number = "23";
            command.Neighborhood = "eggsgf";
            command.City = "sdfgsd";
            command.State = "sdfgsd";
            command.Country = "sfgsg";
            command.ZipCode = "56356776";

            handler.Handle(command);
            Assert.AreEqual(false, handler.IsValid);
        }
    }
}