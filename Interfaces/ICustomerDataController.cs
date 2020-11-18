using ModelEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICustomerDataController
    {
        Task<IList<CustomerModel>> GetAll();
        Task<IList<RoleModel>> GetAllRoles();
        Task<CustomerModel> GetById(Guid Customerid);
        void AddorUpdateCustomer(CustomerModel objCustomer);
        void DeleteCustomerData(CustomerModel objCustomer);
        Task<CustomerModel> GetByEmail(string objName);
        void Passwordupdate(LoginModel objPassWord);
    }
}
