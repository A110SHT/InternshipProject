using BusinessLogics.Common;
using Interfaces;
using ModelEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.LogicalLayer
{
    public class CustomerDataController : ICustomerDataController
    {
        private ICustomerRecordsProvider _CustomerData;
        public CustomerDataController(ICustomerRecordsProvider customerData)
        {
            _CustomerData = customerData;
        }

        public void AddorUpdateCustomer(CustomerModel objCustomer)
        {
            var encrypt = CommonMethods.ConvertToEncrypt(objCustomer.Password);
            objCustomer.PictureName = string.Join(",", objCustomer.Pictures);
            objCustomer.Password = encrypt;
            _CustomerData.ADDorUpdateCustomer(objCustomer);
        }

        public void DeleteCustomerData(CustomerModel objCustomer)
        {
            _CustomerData.RemoveCustomer(objCustomer);
        }

        public async Task<IList<CustomerModel>> GetAll()
        {
            IList<CustomerModel> data = await _CustomerData.ListCustomer();
            foreach (CustomerModel item in data)
            {
                item.Pictures = item.PictureName.Split(',');
                item.Password = CommonMethods.ConvertToDecrypt(item.Password);
            }
            return data;
        }


        public async Task<IList<RoleModel>> GetAllRoles()
        {
            IList<RoleModel> data = await _CustomerData.AllRoles();
            return data;
        }
        

        public async Task<CustomerModel> GetById(Guid Customerid)
        {
            CustomerModel data = await _CustomerData.ListCustomerById(Customerid);
            data.Pictures = data.PictureName.Split(',');
            data.Password = CommonMethods.ConvertToDecrypt(data.Password);
            return data;
        }

        public async Task<CustomerModel> GetByEmail(string objName)
        {
            CustomerModel data = await _CustomerData.ListCustomerByEmail(objName);
            data.Password = CommonMethods.ConvertToDecrypt(data.Password);
            return data;
        }

        public void Passwordupdate(LoginModel objPassWord)
        {
            var oldencrypt = CommonMethods.ConvertToEncrypt(objPassWord.Password);
            var newencrypt = CommonMethods.ConvertToEncrypt(objPassWord.NewPassword);
            objPassWord.Password = oldencrypt;
            objPassWord.NewPassword = newencrypt;
            _CustomerData.UpdatePassword(objPassWord);
        }
        
    }
}
