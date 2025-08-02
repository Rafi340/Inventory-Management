using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Seeds
{
    public class AccountSeed
    {
        public static Account[] AccountSeeds()
        {
            return [
                new Account {Id=1 , AccountTypeId = 1, AccountNumber = "Cash in Cash", OpeningBalance = 0, 
                    Balance= 0 , CreatedAt=new DateTime(2025, 01, 01) },

                new Account {Id=2 , AccountTypeId = 1, AccountNumber = "SD Cash" , OpeningBalance = 0, 
                    Balance= 0, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id=3 , AccountTypeId = 1, AccountNumber = "TB Cash" , OpeningBalance = 0,
                    Balance= 0, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id=4 , AccountTypeId = 2, AccountNumber = "016372383" , OpeningBalance = 1000,
                    Balance= 1000, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id=5 , AccountTypeId = 2, AccountNumber = "019726323" , OpeningBalance = 100,
                    Balance= 100, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id=6 , AccountTypeId = 2, AccountNumber = "019724343" , OpeningBalance = 100, 
                    Balance= 100, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id=7 , AccountTypeId = 2, AccountNumber = "018342342" , OpeningBalance = 100,
                    Balance= 100, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id=8 , AccountTypeId = 3, AccountNumber = "5446546445", HolderName="Taveer" ,BankName="IBBL",
                    BranchName="Dholaikhal" , OpeningBalance = 100, Balance= 100, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id=9 , AccountTypeId = 3, AccountNumber = "56456444655" , HolderName="khan" ,BankName="IBBL",BranchName="Uttara" ,  OpeningBalance = 100, Balance= 100, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id = 10, AccountTypeId = 3, AccountNumber = "3454365456", HolderName = "Alomgir", BankName = "City Bank", BranchName = "Ghulshan" , OpeningBalance = 100, Balance= 100, CreatedAt=new DateTime(2025, 01, 01) },
                new Account {Id = 11, AccountTypeId = 3, AccountNumber = "4398984343", HolderName = "Billal", BankName = "City Bank", BranchName = "Ghulshan" , OpeningBalance = 100, Balance= 100, CreatedAt=new DateTime(2025, 01, 01) },
            ]; }
    }
}
