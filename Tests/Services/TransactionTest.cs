using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Xunit;

namespace Tests.Services
{
   public class TransactionTest
    {

        [Fact]
        public void Test1()
        {
            try
            {
                using (var scope = new TransactionScope())
                {     // We know this one - System.InvalidOperationException:      // TransactionScope必须放在与创建它相同的线程上。     

                }
            }
            catch (Exception e)
            {
                // error handling } 

            }
        }
    }
}
