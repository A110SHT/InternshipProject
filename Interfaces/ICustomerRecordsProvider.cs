using ModelEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICustomerRecordsProvider
    {
        Task<CustomerModel> ListCustomerById(Guid objCustomerid);
        Task<IList<CustomerModel>> ListCustomer();
        Task<IList<RoleModel>> AllRoles();
        void ADDorUpdateCustomer(CustomerModel objCustomer);
        void RemoveCustomer(CustomerModel objCustomer);
        Task<CustomerModel> ListCustomerByEmail(string objCName);
        void UpdatePassword(LoginModel objPassword);
    }
}
