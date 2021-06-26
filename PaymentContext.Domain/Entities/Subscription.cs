using System;
using System.Collections.Generic;
using System.Linq;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities
{
    public class Subscription : Entity
    {
        private readonly IList<Payment> _payments;
        public Subscription(DateTime? expiredate)
        {
            Createdate = DateTime.Now;
            LastUpdateDate = DateTime.Now;
            Expiredate = expiredate;
            Active = true;
            _payments = new List<Payment>();
        }

        public DateTime Createdate { get; private set; } 
        public DateTime LastUpdateDate { get; private set; } 
        public DateTime? Expiredate { get; private set; } 
        public bool Active { get; private set; }
        public IReadOnlyCollection<Payment> Payments { get {return _payments.ToArray();}}

        public void AddPayment(Payment payment) 
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterThan(DateTime.Now, payment.PaidDate, "Subscription.Payments", "A data do pagamento deve ser futura")
            );

            //if(Valid) // Só adiciona se for válido
            _payments.Add(payment);
        }

        public void SetActivate(bool isActivate)
        {
            Active = isActivate;
            LastUpdateDate = DateTime.Now;
        }
    }
}