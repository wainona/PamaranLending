using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using System.IO;
using System.Globalization;

namespace BackgroundTask
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                WriteLog("Generation of receivables started.");
              //  GenerateBill.Generate(DateTime.Now);
                GenerateBillFacade.GenerateReceivable(DateTime.Today);
                WriteLog("Generation of receivables ended.");
            }

            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                WriteLog("Updating status of financial product started.");
                FinancialProduct.UpdateStatus();
                WriteLog("Updating status of financial product ended.");
            }

            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                ///If day difference between lastpaymentdate to today is greater than 3 months, loan account status
                ///must be changed to delinquent
                ///or if 3 months of no payment since loan release date, loan account status must also be changed to 
                /// delinquent
                WriteLog("Updating status of loan account started.");
                LoanAccount.UpdateLoanAccountStatus(DateTime.Today);
                WriteLog("Updating status of loan account ended.");
            }
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
               ///Create First Demand Letter if loan account is delinquent and has no existing 
               ///demand letter for that loan account, also, set the status to Require First Demand Letter
               ///else if demand letter already exists -> if day difference between date today and promise 
               ///to  pay date, change status to Require Final Demand Letter
                WriteLog("Updating status of demand letter started.");
                DemandLetter.UpdateDemandLetterStatus(DateTime.Today);
                WriteLog("Updating status of demand letter ended.");

            }

            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                ///Deliquent -> Active If that customer no longer has an overdue account
                ///Subprime -> Active If customer has no under litigation account && no deliquent account
                ///Subprime -> Deliquent Active If customer has no under litigation account but has deliquent account
                ///Active -> Deliquent if customer has no under litigation account but has deliquent account
                WriteLog("Updating status of customer started.");
                Customer.UpdateCustomerStatus(DateTime.Now);
                WriteLog("Updating status of customer ended.");

            }

      
        }

        private static void WriteLog(string message)
        {
            try
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Lending Application\\";
                if (Directory.Exists(folder) == false)
                    Directory.CreateDirectory(folder);

                string path = folder + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if(File.Exists(path) == false)
                    File.Create(path).Close();

                using (StreamWriter w = File.AppendText(path))
                {
                    w.WriteLine("{0} : {1}", DateTime.Now.ToString(CultureInfo.InvariantCulture), message);
                    w.Flush();
                    w.Close();
                }
            }
            catch
            {
                
            }
        }
    }
}
