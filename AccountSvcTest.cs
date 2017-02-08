using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Avalara.AvaTax.Adapter.AccountService;
using System.Configuration;
using Avalara.AvaTax.Adapter;

namespace Test.Avalara.AvaTax.Adapter
{
    [TestFixture]
    public class AccountSvcTest
    {
        private AccountSvc _accountSvc;

        public AccountSvcTest()
		{
			//
			// TODO: Add constructor logic here
			//
			
		}

		[SetUp]
		public void Init()
		{
			try
			{
				_accountSvc = new AccountSvc();

                _accountSvc.Configuration.Url = ConfigurationSettings.AppSettings.Get("Url");		//"http://117.195.94.233/avaservice";		

				//fill these only if they haven't been loaded from Avalara.AvaTax.Adapter.dll.config
				//if (_addressSvc.Configuration.Security.Account == null || _addressSvc.Configuration.Security.Account.Length == 0)
				//{
                //_accountSvc.Configuration.Security.Account = ConfigurationSettings.AppSettings.Get("account");
                _accountSvc.Configuration.Security.UserName = "qbdsoho@avalara.com"; //"jitendra.chandwani@avalara.com"; //"intadmin@avalara.com";// ConfigurationSettings.AppSettings.Get("UserName");
				//}
				//if (_addressSvc.Configuration.Security.Key == null || _addressSvc.Configuration.Security.Key.Length == 0)
				//{
				//_accountSvc.Configuration.Security.License = ConfigurationSettings.AppSettings.Get("key");
                _accountSvc.Configuration.Security.Password = "Kennwort!";//"Avalara`1#";// ConfigurationSettings.AppSettings.Get("Password");
				//}

				//_addressSvc.Configuration.Security.Timeout = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("timeout"));
                _accountSvc.Profile.Client = "NUnit AccountSvcTest";
				_accountSvc.Profile.Name = "";

			}
			catch (Exception ex)
			{
                Assert.Fail("AccountSvc failed creation: " + ex.Message + " : " + ex.StackTrace);
			}
		}

        [Test]
        public void AccountSvcPingTest()
        {
            try
            {
                PingResult result = _accountSvc.Ping("");

                foreach (Message message in result.Messages)
                {
                    Console.WriteLine(message.Name + ": " + message.Summary);
                }

                Assert.AreEqual(SeverityLevel.Success, result.ResultCode);
            }
            catch (Exception ex)
            {
                Assert.Fail("AccountSvcPingTest failed: " + ex.Message + " : " + ex.StackTrace);
            }
        }

        [Test]
        public void AccountSvcIsAuthorisedTest()
        {
            try
            {
                IsAuthorizedResult result = _accountSvc.IsAuthorized("*");

                foreach (Message message in result.Messages)
                {
                    Console.WriteLine(message.Name + ": " + message.Summary);
                }

                Assert.AreEqual(SeverityLevel.Success, result.ResultCode);
            }
            catch (Exception ex)
            {
                Assert.Fail("AccountSvcIsAuthorisedTest failed: " + ex.Message + " : " + ex.StackTrace);
            }
        }

        [Test]
        public void CompanyRequestTest()
        {
            try
            {
                FetchRequest fetchrequest = new FetchRequest();
                fetchrequest.Fields = "*";
                fetchrequest.Filters = "'CompanyCode='0b89ba8182584eb88566cca00'";
                CompanyFetchResult result = _accountSvc.CompanyFetch(fetchrequest);

                foreach (Message message in result.Messages)
                {
                    Console.WriteLine(message.Name + ": " + message.Summary);
                }

                Assert.AreEqual(SeverityLevel.Success, result.ResultCode);
            }
            catch (Exception ex)
            {
                Assert.Fail("CompanyRequestTest failed: " + ex.Message + " : " + ex.StackTrace);
            }
        }

        [Test]
        public void CompanyNexusFetchTest()
        {
            try
            {
                //string filtersCNT = " CompanyId = {0} and JurisTypeId in ('{1}') and Country = '{2}'";`
                FetchRequest fetchrequest = new FetchRequest();
                fetchrequest.Fields = "*";
                fetchrequest.Filters = "'CompanyID=196536";
                NexusFetchResult result = _accountSvc.NexusFetch(fetchrequest);

                foreach (Message message in result.Messages)
                {
                    Console.WriteLine(message.Name + ": " + message.Summary);
                }
                Assert.AreEqual(61, result.Nexuses.Count);
                Assert.AreEqual(SeverityLevel.Success, result.ResultCode);
            }
            catch (Exception ex)
            {
                Assert.Fail("CompanyNexusFetchTest failed: " + ex.Message + " : " + ex.StackTrace);
            }
        }

        public static Company CreateCompany(AccountSvc  _accountSvc,  int accountId, bool isSST = false)
        {
            var companyContact = new CompanyContact
            {
                CompanyContactCode = "001",
                FirstName = "testfirstname",
                LastName = "testlastname",
                Line1 = "700 Pike St",
                City = "Seattle",
                Region = "WA",
                PostalCode = "98101",
                Phone = "1-877-780-4848",
                Email = "a@b.com",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            var company = new Company
            {
                AccountId = accountId,
                CompanyCode = Guid.NewGuid().ToString("N").Substring(0, 25),
                CompanyName = "TEST::" + Guid.NewGuid().ToString("N"),
                CompanyId = 0,
                IsActive = true,
                HasProfile = true,
                IsReportingEntity = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                RoundingLevelId = RoundingLevelId.Document,
                DefaultCountry = "US",
                IsTest = false,
                TaxDependencyLevelId = TaxDependencyLevelId.Document,
                IsDefault = false,
                TIN = "123457777"
            };

            if (isSST)
            {
                company.SSTEffDate = DateTime.UtcNow.AddYears(-10);
                company.SSTPID = "Test";
            }
            company.Contacts.Add(companyContact);

            CompanySaveResult result = _accountSvc.CompanySave(company);

            if (result.ResultCode != SeverityLevel.Success)
            {
                foreach (Message message in result.Messages)
                {
                    Console.WriteLine(message.Summary);
                }

                return null;
            }

            company.CompanyId = result.CompanyId;
            return company;
        }

        [Test]
        public void CreateCompanyMJ()
        {
            Company companyid = CreateCompany(_accountSvc, Convert.ToInt32(ConfigurationSettings.AppSettings.Get("account")));
        }

        private CompanyContact CreateContact()
        {
            CompanyContact contact = new CompanyContact();
            contact.CompanyContactCode = "PRIMARY";
            contact.Line1 = "900 Winslow Way";
            contact.Line2 = "Ste 130";
            contact.City = "Bainbridge Island";
            contact.Region = "WA";
            contact.Country = "US";
            contact.PostalCode = "98110";
            contact.FirstName = "Ramkrishna";
            contact.LastName = "Shenkar";
            contact.Email = "ramkrishna.shenkar@avalara.com";
            contact.Phone = "12235455";
            return contact;
        }

        private Nexus CreateNexusCountry()
        {
            Nexus nexus = new Nexus();

            nexus.CompanyId = 414689; 
            nexus.EffDate = DateTime.Today.AddYears(-2);
            nexus.EndDate = DateTime.Today.AddYears(2);
            nexus.Country = "US";
            nexus.State = "WA";
            nexus.NexusId = 0;
            nexus.JurisCode = "USA";
            nexus.ShortName = "US";
            nexus.JurisName = "UNITED STATES";
            nexus.JurisTypeId = JurisTypeId.CNT;
            nexus.LocalNexusTypeId = LocalNexusTypeId.All;
            nexus.TaxId = null;
            nexus.AccountingMethodId = 1;
            nexus.NexusTypeId = NexusTypeId.NonVolunteer;
            return nexus;
        }

        private Nexus CreateNexusState()
        {
            Nexus nexus = new Nexus();

            nexus.CompanyId = 414689;
            nexus.EffDate = DateTime.Today.AddYears(-2);
            nexus.EndDate = DateTime.Today.AddYears(2);
            nexus.Country = "US";
            nexus.State = "WA";
            nexus.NexusId = 0;
            nexus.JurisCode = "WA";
            nexus.ShortName = "WA";
            nexus.JurisName = "WASHINGTON";
            nexus.JurisTypeId = JurisTypeId.STA;
            nexus.LocalNexusTypeId = LocalNexusTypeId.All;
            nexus.TaxId = null;
            nexus.AccountingMethodId = 1;
            nexus.NexusTypeId = NexusTypeId.NonVolunteer;
            return nexus;
        }

        [Test]
        public void NexusCreateTest()
        {
            try
            {

                //string filtersCNT = " CompanyId = {0} and JurisTypeId in ('{1}') and Country = '{2}'";
                //FetchRequest fetchrequest = new FetchRequest();
                //fetchrequest.Fields = "*";
                //fetchrequest.Filters = "'CompanyID=196536";
                //NexusFetchResult result = _accountSvc.NexusFetch(fetchrequest);

                Nexus createNexus = CreateNexusState();

                NexusSaveResult result = _accountSvc.NexusSave(createNexus);

                foreach (Message message in result.Messages)
                {
                    Console.WriteLine(message.Name + ": " + message.Summary);
                }

                Assert.AreEqual(SeverityLevel.Success, result.ResultCode);
            }
            catch (Exception ex)
            {
                Assert.Fail("CompanyRequestTest failed: " + ex.Message + " : " + ex.StackTrace);
            }
        }

        [Test]
        public void UserFetchTest()
        {
            try
            {
                FetchRequest fetchrequest = new FetchRequest();
                fetchrequest.Fields = "*";
                fetchrequest.PageSize = 1;
                fetchrequest.RecordCount = 99;
                fetchrequest.MaxCount = 99;
                fetchrequest.Filters = "UserName='qbdsoho@avalara.com'";
                UserFetchResult result = _accountSvc.UserFetch(fetchrequest);
                //User u = result.Users[0];
                //u.PasswordStatusId = PasswordStatusId.UserMustChange;
                //_accountSvc.UserSave(result.Users[0]);

                foreach (Message message in result.Messages)
                {
                    Console.WriteLine(message.Name + ": " + message.Summary);
                }
                //Assert.AreEqual(61, result.Nexuses.Count);
                //Assert.AreEqual(SeverityLevel.Success, result.ResultCode);

                //Assert.AreEqual(SeverityLevel.Success, result.ResultCode);
            }
            catch (Exception ex)
            {
                Assert.Fail("CompanyRequestTest failed: " + ex.Message + " : " + ex.StackTrace);
            }
        }



    }
}

