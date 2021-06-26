using System.Collections.Generic;
using System.Linq;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;
using Flunt.Validations;
using Flunt.Notifications;

namespace PaymentContext.Domain.Entities
{
    public class Student : Entity
    {
        private IList<Subscription> _subscriptions;
        public Student(Name name, Document document, Email email, Address address)
        {
            Name = name;
            Document = document;
            Email = email;
            Address = address;
            _subscriptions = new List<Subscription>();

            AddNotifications(name, document, email);
        }

        public Name Name { get; set; }
        public Document Document { get; private set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<Subscription> Subscriptions { get { return _subscriptions.ToArray(); } }

        public void AddSubscription(Subscription subscription) {
            // foreach(var sub in Subscriptions)
            //     subscription.SetActivate(false);
            
            var hasSubscriptionActive = false;
            foreach (var item in _subscriptions)
            {
                if(item.Active)
                    hasSubscriptionActive = true;
            }

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsFalse(hasSubscriptionActive, "Student.Subscriptions", "Você já possui uma assinatura ativa")
                .AreNotEquals(0, subscription.Payments.Count, "Student.Subscription.Payments", "Esta assinatura não possui pagamentos")
            );
            
            _subscriptions.Add(subscription);

            //Alternativa
            // if(hasSubscriptionActive)
            //     AddNotification("Student.Subscriptions", "Você já possui uma assinatura ativa");
        }

    }
}