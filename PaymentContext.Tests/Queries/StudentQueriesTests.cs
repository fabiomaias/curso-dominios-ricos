using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Queries;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.Queries
{
    [TestClass]
    public class StudentQueriesTests
    {
        List<Student> _students;

        public StudentQueriesTests()
        {
            _students = new List<Student>();
           for (int i = 0; i <= 10; i++)
            {
                var student = new Student(
                    new Name("Aluno", "numero" + i.ToString()),
                    new Document("1111111111" + i.ToString(), EDocumentType.CPF),
                    new Email(i.ToString() + "@intranext.com.br"),
                    new Address("hggjh", "43", "kjhgkg", "kjhkjhh", "ihhhghoh", "iugggg", "45444678")
                );
                _students.Add(student);
            }
        }
        
        [TestMethod]
        public void ShouldRenurnNullWHenDocumentNotExists()
        {
            var exp = StudentQueries.GetStudentInfo("12345678911");
            var studn = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreEqual(null, studn);
        }

        [TestMethod]
        public void ShouldRenurnStudentWHenDocumentExists()
        {
            var exp = StudentQueries.GetStudentInfo("11111111111");
            var studn = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreNotEqual(null, studn);
        }
    }
}