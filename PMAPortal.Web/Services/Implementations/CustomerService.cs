using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class CustomerService: ICustomerService
    {
        private readonly IRepository<Customer> customerRepo;
        private readonly ILoggerService<CustomerService> logger;
        private readonly IHttpContextAccessor accessor;

        public CustomerService(IRepository<Customer> customerRepo,
            ILoggerService<CustomerService> logger,
            IHttpContextAccessor accessor)
        {
            this.customerRepo = customerRepo;
            this.logger = logger;
            this.accessor = accessor;
        }

        // add Customer
        public async Task CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new AppException("Customer object cannot be null");
            }

            if (await customerRepo.AnyAsync(c=> c.AccountNumber == customer.AccountNumber || c.Email==customer.Email || c.PhoneNumber == customer.PhoneNumber))
            {
                throw new AppException($"A customer with account number '{customer.AccountNumber}', email '{customer.Email}' or phone number '{customer.PhoneNumber}' already exist");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            customer.CreatedBy = currentUser.Id;
            customer.CreatedDate = DateTimeOffset.Now;

            await customerRepo.Insert(customer, false);

            //log action
            await logger.LogActivity(ActivityActionType.CREATE_CUSTOMER, currentUser.Email, $"Created customer with name {customer.CustomerName}");
        }

        // delete Customer
        public async Task DeleteCustomer(long customerId)
        {
            var customer = await customerRepo.GetById(customerId);
            if (customer == null)
            {
                throw new AppException($"Invalid Customer id {customerId}");
            }
            else
            {
                if(customer.Surveys.Count() > 0 || customer.Installations.Count() > 0)
                {
                    throw new AppException($"Customer cannot be deleted as customer is been processed");
                }

                var _customer = customer.Clone<Customer>();
                await customerRepo.Delete(customerId, false);

                var currentUser = accessor.HttpContext.GetUserSession();
                // log activity
                await logger.LogActivity(ActivityActionType.DELETE_CUSTOMER, currentUser.Email, customerRepo.TableName, _customer, new Customer(),
                    $"Deleted customer with name {_customer.CustomerName}");
            }
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return customerRepo.GetAll().OrderBy(c => c.CustomerName);
        }
        public IEnumerable<Customer> GetCustomers(long batchId)
        {
            return customerRepo.GetWhere(c=>c.BatchId==batchId).OrderBy(c => c.CustomerName);
        }

        public async Task<Customer> GetCustomer(long id)
        {
            return await customerRepo.GetById(id);
        }

    }
}
