using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface ICustomerService
    {
        Task CreateCustomer(Customer customer);
        Task DeleteCustomer(long customerId);
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Customer> GetCustomers(long batchId);
        Task<Customer> GetCustomer(long id);
    }
}
