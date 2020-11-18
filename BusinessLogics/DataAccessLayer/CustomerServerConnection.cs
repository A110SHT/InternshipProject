using Interfaces;
using Microsoft.Extensions.Options;
using ModelEntities;
using SQLHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.DataAccessLayer
{
    public class CustomerServerConnection : ICustomerRecordsProvider
    {
            public CustomerServerConnection(IOptions<ConnectionStrings> dataoptions)
            {
                SQLHandlerAsync.Connectionconfig = dataoptions.Value.DatabaseConnection;
            }
            public async void ADDorUpdateCustomer(CustomerModel objCustomer)
            {
                SQLExecuteNonQueryAsync SQLH = new SQLExecuteNonQueryAsync();
                List<SQLParam> param = new List<SQLParam>()
                    {
                         new SQLParam("@customerid",objCustomer.CustomerId),
                         new SQLParam("@email",objCustomer.Email),
                         new SQLParam("@customername",objCustomer.CustomerName),
                         new SQLParam("@password",objCustomer.Password),
                         new SQLParam("@roleid",objCustomer.RoleId),
                         new SQLParam("@address",objCustomer.Address),
                         new SQLParam("@contact",objCustomer.Contact),
                         new SQLParam("@purchasedate",objCustomer.PurchaseDate),
                         new SQLParam("@picturename",objCustomer.PictureName)
                    };
                await SQLH.ExecuteNonQueryAsync("u_sp_CustomerAddorEdit", param);
            }

            public async Task<CustomerModel> ListCustomerById(Guid objCustomerid)
            {
                SQLGetAsync SQLH = new SQLGetAsync();
                List<SQLParam> param = new List<SQLParam>()
            {
                new SQLParam("@customerid",objCustomerid),
            };
                return await SQLH.ExecuteAsObjectAsync<CustomerModel>("u_sp_CustomerGetById", param);
            }
            public async Task<IList<CustomerModel>> ListCustomer()
            {
                SQLGetListAsync Sqlh = new SQLGetListAsync();
                return await Sqlh.ExecuteAsListAsync<CustomerModel>("u_sp_CustomerList");           

            }


            public async Task<IList<RoleModel>> AllRoles()
        {
            SQLGetListAsync Sqlh = new SQLGetListAsync();
            return await Sqlh.ExecuteAsListAsync<RoleModel>("u_GetAllRoles");

        }

            public async void RemoveCustomer(CustomerModel objCustomer)
            {
                SQLExecuteNonQueryAsync Sqlh = new SQLExecuteNonQueryAsync();
                List<SQLParam> param = new List<SQLParam>()
                {
                new SQLParam("@customerid",objCustomer.CustomerId)
                };
                await Sqlh.ExecuteNonQueryAsync("u_sp_CustomerRemove", param);
            }

            public async Task<CustomerModel> ListCustomerByEmail(string objCName)
        {
            SQLGetAsync SQLH = new SQLGetAsync();
            List<SQLParam> param = new List<SQLParam>()
            {
                new SQLParam("@email",objCName)
            }; 
            return await SQLH.ExecuteAsObjectAsync<CustomerModel>("u_sp_CustomerByEmail", param);
        }

       
            public async void UpdatePassword(LoginModel objPassword)
        {
            SQLExecuteNonQueryAsync SQLH = new SQLExecuteNonQueryAsync();
            List<SQLParam> param = new List<SQLParam>()
                    {
                         new SQLParam("@newpassword",objPassword.NewPassword),
                         new SQLParam("@password",objPassword.Password)
                    };
            await SQLH.ExecuteNonQueryAsync("u_ChangePassword", param);
        }
    }
}
