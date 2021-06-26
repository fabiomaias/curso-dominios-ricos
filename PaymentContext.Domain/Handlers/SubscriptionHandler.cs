using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable<Notification>, IHandler<CreateBoletoSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if(!command.IsValid){
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            } 

            // Verificar se o Documento já existe
            if(_repository.DocumentExixts(command.Document))
                AddNotification("Document", "Já existe uma conta cadastrada com este CPF");

            // Verificar se o E-mail já existe
            if(_repository.EmailExists(command.Email))
                AddNotification("Email", "Já existe conta criada com este e-mail");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street,command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Gerar as Entidades
            var student = new Student(name, document, email, address);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(command.BarCode, command.BoletoNumber, command.PaidDate, command.ExpireDate, command.Total, command.TotalPaid, command.Payer, new Document(command.PayerDocument, command.PayerDocumentType), address, email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar Validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            //checar noificações
            if(!IsValid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            // Salvar Informações
            _repository.CreateSubscription(student);

            // Enviar E-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Sua conta", "Seja bem vindo! Sua assinatura foi gerada.");

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}