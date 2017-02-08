#region Copyright
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Avalara.AvaTax.Adapter.Proxies;
using Avalara.AvaTax.Adapter.Proxies.AccountSvcProxy;
using System.Xml.Serialization;

namespace Avalara.AvaTax.Adapter.AccountService
{
    /// <include file='AccountSvc.Doc.xml' path='adapter/AccountSvc/class/*' />
	[Guid("671C83F8-CAFD-4d9e-A82D-18D8C11CA59C")]
	[ComVisible(true)]
	public class AccountSvc : BaseSvc, IDisposable
	{
		private const string SERVICE_NAME = "Account/AccountSvc.asmx/v2";
        //vprivate const string SERVICE_NAME = "Account/AccountSvc.asmx";
        
		private AvaLogger _avaLog;

        /// <include file='AccountSvc.Doc.xml' path='adapter/common/members[@name="Constructor"]/*' />
		public AccountSvc()
		{
			try 
			{
				_avaLog = AvaLogger.GetLogger();
				_avaLog.Debug(string.Format("Instantiating AccountSvc: {0}", base.UniqueId));
				base.ServiceName = SERVICE_NAME;
				ProxyAccountSvc proxy = new ProxyAccountSvc();
				proxy.ProfileValue = new ProxyProfile();
				proxy.Security = new ProxySecurity();
				base.SvcProxy = proxy;

			} catch (Exception ex)
			{
				ExceptionManager.HandleException(ex);
			}
		}

        /// <include file='AccountSvc.Doc.xml' path='adapter/common/members[@name="Ping"]/*' />
        [DispId(30)]
        public PingResult Ping(string message)
        {
            try
            {
                _avaLog.Debug("AccountSvc.Ping");
                ProxyPingResult svcResult = (ProxyPingResult)base.InvokeService(typeof(ProxyAccountSvc), MethodBase.GetCurrentMethod().Name, new object[] { message });

                _avaLog.Debug("Copying result from proxy object");
                PingResult localResult = new PingResult();
                localResult.CopyFrom(svcResult);					//load local object with service results

                return localResult;
            }
            catch (Exception ex)
            {
                return PingResult.CastFromBaseResult(ExceptionManager.HandleException(ex));
            }
        }

        /// <include file='AccountSvc.Doc.xml' path='adapter/common/members[@name="IsAuthorized"]/*' />
		[DispId(31)]
		public IsAuthorizedResult IsAuthorized(string operations)
		{
			try
			{
                _avaLog.Debug("AccountSvc.IsAuthorized");

                ProxyIsAuthorizedResult svcResult = (ProxyIsAuthorizedResult)base.InvokeService(typeof(ProxyAccountSvc), MethodBase.GetCurrentMethod().Name, new object[] { operations });

                _avaLog.Debug("Copying result from proxy object");
                IsAuthorizedResult localResult = new IsAuthorizedResult();	//local copy to hold service results
                localResult.CopyFrom(svcResult);	//load local object with service results

				return localResult;
			} catch (Exception ex)
			{
				return IsAuthorizedResult.CastFromBaseResult(ExceptionManager.HandleException(ex));
			}
		}

        /// <include file='AccountSvc.Doc.xml' path='adapter/AccountSvc/members[@name="CompanyFetch"]/*' />
		[DispId(32)]
        public CompanyFetchResult CompanyFetch(FetchRequest fetchRequest)
		{
			try
			{
                _avaLog.Debug("AccountSvc.CompanyFetch");

                Utilities.VerifyRequestObject(fetchRequest);

                _avaLog.Debug("Copying Company Fetch info into proxy object");
                ProxyFetchRequest proxyRequest = new ProxyFetchRequest();
                fetchRequest.CopyTo(proxyRequest);
                //Record time take for address validation
                Perf monitor = new Perf();
                monitor.Start();

                ProxyCompanyFetchResult svcResult = (ProxyCompanyFetchResult)base.InvokeService(typeof(ProxyAccountSvc), MethodBase.GetCurrentMethod().Name, new object[] { proxyRequest });

                monitor.Stop(this, ref svcResult);
                _avaLog.Debug("Copying Company info from proxy object");
                CompanyFetchResult localResult = new CompanyFetchResult();
                localResult.CopyFrom(svcResult);

				return localResult;
			} 
			catch (Exception ex)
			{
                return CompanyFetchResult.CastFromBaseResult(ExceptionManager.HandleException(ex));
			}
		}

        /// <include file='AccountSvc.Doc.xml' path='adapter/AccountSvc/members[@name="NexusFetch"]/*' />
        [DispId(33)]
        public NexusFetchResult NexusFetch(FetchRequest fetchRequest)
        {
            try
            {
                _avaLog.Debug("AccountSvc.NexusFetch");

                Utilities.VerifyRequestObject(fetchRequest);

                _avaLog.Debug("Copying Nexus Fetch info into proxy object");
                ProxyFetchRequest proxyRequest = new ProxyFetchRequest();
                fetchRequest.CopyTo(proxyRequest);
                //Record time take for address validation
                Perf monitor = new Perf();
                monitor.Start();

                ProxyNexusFetchResult svcResult = (ProxyNexusFetchResult)base.InvokeService(typeof(ProxyAccountSvc), MethodBase.GetCurrentMethod().Name, new object[] { proxyRequest });

                monitor.Stop(this, ref svcResult);
                _avaLog.Debug("Copying Nexues info from proxy object");
                NexusFetchResult localResult = new NexusFetchResult();
                localResult.CopyFrom(svcResult);

                return localResult;
            }
            catch (Exception ex)
            {
                return NexusFetchResult.CastFromBaseResult(ExceptionManager.HandleException(ex));
            }
        }

        /// <include file='AccountSvc.Doc.xml' path='adapter/AccountSvc/members[@name="CompanySave"]/*' />
        [DispId(34)]
        public CompanySaveResult CompanySave(Company CompanyRequest)
        {
            try
            {
                _avaLog.Debug("AccountSvc.CompanySave");
                _avaLog.Debug("Copying company info into proxy object");

                ProxyCompany proxyCompany = new ProxyCompany();
                CompanyRequest.CopyTo(proxyCompany);

                ProxyCompanySaveResult svcResult = (ProxyCompanySaveResult)base.InvokeService(typeof(ProxyAccountSvc), MethodBase.GetCurrentMethod().Name, new object[] { proxyCompany });

                _avaLog.Debug("Copying company info from proxy object");
                CompanySaveResult localResult = new CompanySaveResult();
                localResult.CopyFrom(svcResult);

                return localResult;
            }
            catch (Exception ex)
            {
                return CompanySaveResult.CastFromBaseResult(ExceptionManager.HandleException(ex));
            }
        }

        /// <include file='AccountSvc.Doc.xml' path='adapter/AccountSvc/members[@name="NexusSave"]/*' />
        [DispId(35)]
        public NexusSaveResult NexusSave(Nexus Nexus)
        {
            _avaLog.Debug("AccountSvc.NexusSave");
            _avaLog.Debug("Copying Nexus info into proxy object");

            ProxyNexus proxyNexus = new ProxyNexus();
            Nexus.CopyTo(proxyNexus);

            ProxyNexusSaveResult svcResult = (ProxyNexusSaveResult)base.InvokeService(typeof(ProxyAccountSvc), MethodBase.GetCurrentMethod().Name, new object[] { proxyNexus });

            _avaLog.Debug("Copying Nexues info from proxy object");
            NexusSaveResult localResult = new NexusSaveResult();
            localResult.CopyFrom(svcResult);

            return localResult;

        }

        [DispId(36)]
        public UserFetchResult UserFetch(FetchRequest fetchRequest)
        {
            try
            {
                _avaLog.Debug("AccountSvc.UserFetch");

                Utilities.VerifyRequestObject(fetchRequest);

                _avaLog.Debug("Copying User Fetch info into proxy object");
                ProxyFetchRequest proxyRequest = new ProxyFetchRequest();
                fetchRequest.CopyTo(proxyRequest);
                //Record time take for address validation
                Perf monitor = new Perf();
                monitor.Start();

                ProxyUserFetchResult svcResult = (ProxyUserFetchResult)base.InvokeService(typeof(ProxyAccountSvc), MethodBase.GetCurrentMethod().Name, new object[] { proxyRequest });

                monitor.Stop(this, ref svcResult);
                _avaLog.Debug("Copying User info from proxy object");
                UserFetchResult localResult = new UserFetchResult();
                localResult.CopyFrom(svcResult);

                return localResult;
            }
            catch (Exception ex)
            {
                return UserFetchResult.CastFromBaseResult(ExceptionManager.HandleException(ex));
            }
        }
        [DispId(37)]
        public UserSaveResult UserSave(User _user)
        {
            try
            {
                _avaLog.Debug("AccountSvc.UserSave");
                _avaLog.Debug("Copying User info into proxy object");

                ProxyUser _proxyUser = new ProxyUser();
                _user.CopyTo(_proxyUser);

                ProxyUserSaveResult svcResult = (ProxyUserSaveResult)base.InvokeService(typeof(ProxyAccountSvc), MethodBase.GetCurrentMethod().Name, new object[] { _proxyUser });

                _avaLog.Debug("Copying company info from proxy object");
                UserSaveResult localResult = new UserSaveResult();
                localResult.CopyFrom(svcResult);

                return localResult;
            }
            catch (Exception ex)
            {
                return UserSaveResult.CastFromBaseResult(ExceptionManager.HandleException(ex));
            }
        }
               

        /// <include file='AccountSvc.Doc.xml' path='adapter/common/members[@name="Finalize"]/*' />
        ~AccountSvc()
		{
            _avaLog.Debug(string.Format("Finalizing AccountSvc: {0}", base.UniqueId));
		}
		#region Internal Members

		/// <summary>
		/// Convenient wrapper property that casts the base WebServicesClientProtocol into our strongly typed proxy class.
		/// </summary>
		internal new ProxyAccountSvc SvcProxy
		{
			get
			{
                return (ProxyAccountSvc)base.SvcProxy;
			}
		}
		#endregion

	}
    [XmlType(Namespace = "http://avatax.avalara.com/services", TypeName = "UserSaveResult")]
    public class UserSaveResult : BaseResult
    {
        public UserSaveResult()
        {

        }
        private int userIdField;

        /// <remarks/>
        public int UserId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        internal static UserSaveResult CastFromBaseResult(BaseResult baseResult)
        {
            UserSaveResult result = new UserSaveResult();
            result.CopyFrom(baseResult);
            return result;
        }
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-B54E-F28333C4DCAC")]
    [ComVisible(true)]
    public partial class NexusSaveResult : BaseResult
    {

        private int nexusIdField;

        /// <remarks/>
        public int NexusId
        {
            get
            {
                return this.nexusIdField;
            }
            set
            {
                this.nexusIdField = value;
            }
        }

        internal void CopyFrom(ProxyNexusSaveResult svcResult)
        {
            base.CopyFrom(svcResult);
            svcResult.NexusId = nexusIdField;
        }

        internal static NexusSaveResult CastFromBaseResult(BaseResult baseResult)
        {
            NexusSaveResult result = new NexusSaveResult();
            result.CopyFrom(baseResult);
            return result;
        }
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-B55E-F28333C5DCAC")]
    [ComVisible(true)]
    public class CompanySaveResult : BaseResult
    {
        private int companyIdField;
        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        internal void CopyFrom(ProxyCompanySaveResult svcResult)
        {
            base.CopyFrom(svcResult);
            companyIdField = svcResult.CompanyId;
        }

        internal static CompanySaveResult CastFromBaseResult(BaseResult baseResult)
        {
            CompanySaveResult result = new CompanySaveResult();
            result.CopyFrom(baseResult);
            return result;
        }
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-B55E-F28332C5DCAC")]
    [ComVisible(true)]
    public class FetchRequest : BaseRequest
    {

        private string fieldsField;

        private string filtersField;

        private string sortField;

        private int maxCountField;

        private int pageIndexField;

        private int pageSizeField;

        private int recordCountField;

        /// <remarks/>
        public string Fields
        {
            get
            {
                return this.fieldsField;
            }
            set
            {
                this.fieldsField = value;
            }
        }

        /// <remarks/>
        public string Filters
        {
            get
            {
                return this.filtersField;
            }
            set
            {
                this.filtersField = value;
            }
        }

        /// <remarks/>
        public string Sort
        {
            get
            {
                return this.sortField;
            }
            set
            {
                this.sortField = value;
            }
        }

        /// <remarks/>
        public int MaxCount
        {
            get
            {
                return this.maxCountField;
            }
            set
            {
                this.maxCountField = value;
            }
        }

        /// <remarks/>
        public int PageIndex
        {
            get
            {
                return this.pageIndexField;
            }
            set
            {
                this.pageIndexField = value;
            }
        }

        /// <remarks/>
        public int PageSize
        {
            get
            {
                return this.pageSizeField;
            }
            set
            {
                this.pageSizeField = value;
            }
        }

        /// <remarks/>
        public int RecordCount
        {
            get
            {
                return this.recordCountField;
            }
            set
            {
                this.recordCountField = value;
            }
        }

        internal void CopyTo(ProxyFetchRequest proxyFetchRequest)
        {
             proxyFetchRequest.Fields = fieldsField;
             proxyFetchRequest.Filters = filtersField;
             proxyFetchRequest.Sort = sortField;
             proxyFetchRequest.MaxCount = maxCountField;
             proxyFetchRequest.PageIndex = pageIndexField;
             proxyFetchRequest.PageSize = pageSizeField;
             proxyFetchRequest.RecordCount = recordCountField;
        }

        internal override bool IsValid(out string message)
        {
            message = "";
            return true;
        }
    }

    /// <remarks/>
    [Guid("29931F0B-BFC8-42c4-B55E-F28332C5DCAC")]
    [ComVisible(true)]
    public class CompanyFetchResult : BaseResult
    {

        private Companies companiesField;

        private int recordCountField;

        /// <remarks/>
        public Companies Companies
        {
            get
            {
                return this.companiesField;
            }
            set
            {
                this.companiesField = value;
            }
        }

        /// <remarks/>
        public int RecordCount
        {
            get
            {
                return this.recordCountField;
            }
            set
            {
                this.recordCountField = value;
            }
        }

        internal void CopyFrom(ProxyCompanyFetchResult SvcResult)
        {
            base.CopyFrom(SvcResult);

            //iterate through addresses returned by the web service and move them into
            //    a local address object and local arraylist
            if (SvcResult.Companies != null)
            {
                companiesField = new Companies();
                for (int Index = 0; Index < SvcResult.Companies.Length; Index++)
                {
                    ProxyCompany SvcAddress = SvcResult.Companies[Index];
                    Company localCompany = new Company();
                    localCompany.CopyFrom(SvcAddress);
                    companiesField.Add(localCompany);
                }
            }
        }

        internal static CompanyFetchResult CastFromBaseResult(BaseResult baseResult)
        {
            CompanyFetchResult result = new CompanyFetchResult();
            result.CopyFrom(baseResult);
            return result;
        }
    }

    /// <remarks/>
    /// <remarks/>
    [Guid("29931F9B-BFC8-42c4-B55E-F28332C5DCAC")]
    [ComVisible(true)]
    public class NexusFetchResult : BaseResult
    {

        private Nexuses nexusesField;

        private int recordCountField;

        /// <remarks/>
        public Nexuses Nexuses
        {
            get
            {
                return this.nexusesField;
            }
            set
            {
                this.nexusesField = value;
            }
        }

        /// <remarks/>
        public int RecordCount
        {
            get
            {
                return this.recordCountField;
            }
            set
            {
                this.recordCountField = value;
            }
        }

        internal void CopyFrom(ProxyNexusFetchResult SvcResult)
        {
            base.CopyFrom(SvcResult);
            recordCountField = SvcResult.RecordCount;
            //iterate through addresses returned by the web service and move them into
            //    a local address object and local arraylist
            if (SvcResult.Nexuses != null)
            {
                nexusesField = new Nexuses();
                for (int Index = 0; Index < SvcResult.Nexuses.Length; Index++)
                {
                    ProxyNexus SvcNexus = SvcResult.Nexuses[Index];
                    Nexus locaNexus = new Nexus();
                    locaNexus.CopyFrom(SvcNexus);
                    nexusesField.Add(locaNexus);
                }
            }
        }

        internal static NexusFetchResult CastFromBaseResult(BaseResult baseResult)
        {
            NexusFetchResult result = new NexusFetchResult();
            result.CopyFrom(baseResult);
            return result;
        }
    }

    /// <include file='AccountSvc.Doc.xml' path='adapter/Certificates/class/*' />
    [Guid("29931F0B-BFC7-42c4-B55E-F28332C5DDAC")]
    [ComVisible(true)]
    public class Company
    {
        private int accountIdField;

        private int companyIdField;

        private string companyCodeField;

        private string companyNameField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private int entityNoField;

        private bool hasProfileField;

        private bool isActiveField;

        private bool isDefaultField;

        private bool isReportingEntityField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private int parentIdField;

        private System.DateTime sSTEffDateField;

        private string sSTPIDField;

        private string tINField;

        private string regalBankIdField;

        private string defaultCountryField;

        private string baseCurrencyCodeField;

        private RoundingLevelId roundingLevelIdField;

        private bool cashBasisAccountingEnabledField;

        private Companies childrenField = new Companies();

        private CompanyContacts contactsField = new CompanyContacts();

        private Item[] itemsField;

        private Nexuses nexusesField = new Nexuses();

        private Company parentField;

        private TaxCode[] taxCodesField;

        private TaxRule[] taxRulesField;

        private CompanyReturn[] filingCalendarsField;

        private bool warningsEnabledField;

        private bool isTestField;

        private TaxDependencyLevelId taxDependencyLevelIdField;

        private bool inProgressField;

        private int defaultLocationIdField;

        private string businessIdentificationNoField;

        /// <remarks/>
        public int AccountId
        {
            get
            {
                return this.accountIdField;
            }
            set
            {
                this.accountIdField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public string CompanyCode
        {
            get
            {
                return this.companyCodeField;
            }
            set
            {
                this.companyCodeField = value;
            }
        }

        /// <remarks/>
        public string CompanyName
        {
            get
            {
                return this.companyNameField;
            }
            set
            {
                this.companyNameField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public int EntityNo
        {
            get
            {
                return this.entityNoField;
            }
            set
            {
                this.entityNoField = value;
            }
        }

        /// <remarks/>
        public bool HasProfile
        {
            get
            {
                return this.hasProfileField;
            }
            set
            {
                this.hasProfileField = value;
            }
        }

        /// <remarks/>
        public bool IsActive
        {
            get
            {
                return this.isActiveField;
            }
            set
            {
                this.isActiveField = value;
            }
        }

        /// <remarks/>
        public bool IsDefault
        {
            get
            {
                return this.isDefaultField;
            }
            set
            {
                this.isDefaultField = value;
            }
        }

        /// <remarks/>
        public bool IsReportingEntity
        {
            get
            {
                return this.isReportingEntityField;
            }
            set
            {
                this.isReportingEntityField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public int ParentId
        {
            get
            {
                return this.parentIdField;
            }
            set
            {
                this.parentIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime SSTEffDate
        {
            get
            {
                return this.sSTEffDateField;
            }
            set
            {
                this.sSTEffDateField = value;
            }
        }

        /// <remarks/>
        public string SSTPID
        {
            get
            {
                return this.sSTPIDField;
            }
            set
            {
                this.sSTPIDField = value;
            }
        }

        /// <remarks/>
        public string TIN
        {
            get
            {
                return this.tINField;
            }
            set
            {
                this.tINField = value;
            }
        }

        /// <remarks/>
        public string RegalBankId
        {
            get
            {
                return this.regalBankIdField;
            }
            set
            {
                this.regalBankIdField = value;
            }
        }

        /// <remarks/>
        public string DefaultCountry
        {
            get
            {
                return this.defaultCountryField;
            }
            set
            {
                this.defaultCountryField = value;
            }
        }

        /// <remarks/>
        public string BaseCurrencyCode
        {
            get
            {
                return this.baseCurrencyCodeField;
            }
            set
            {
                this.baseCurrencyCodeField = value;
            }
        }

        /// <remarks/>
        public RoundingLevelId RoundingLevelId
        {
            get
            {
                return this.roundingLevelIdField;
            }
            set
            {
                this.roundingLevelIdField = value;
            }
        }

        /// <remarks/>
        public bool CashBasisAccountingEnabled
        {
            get
            {
                return this.cashBasisAccountingEnabledField;
            }
            set
            {
                this.cashBasisAccountingEnabledField = value;
            }
        }

        /// <remarks/>
        public Companies Children
        {
            get
            {
                return this.childrenField;
            }
            set
            {
                this.childrenField = value;
            }
        }

        /// <remarks/>
        public CompanyContacts Contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
            }
        }

        /// <remarks/>
        public Item[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        public Nexuses Nexuses
        {
            get
            {
                return this.nexusesField;
            }
            set
            {
                this.nexusesField = value;
            }
        }

        /// <remarks/>
        public Company Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        /// <remarks/>
        public TaxCode[] TaxCodes
        {
            get
            {
                return this.taxCodesField;
            }
            set
            {
                this.taxCodesField = value;
            }
        }

        /// <remarks/>
        public TaxRule[] TaxRules
        {
            get
            {
                return this.taxRulesField;
            }
            set
            {
                this.taxRulesField = value;
            }
        }

        /// <remarks/>
        public CompanyReturn[] FilingCalendars
        {
            get
            {
                return this.filingCalendarsField;
            }
            set
            {
                this.filingCalendarsField = value;
            }
        }

        /// <remarks/>
        public bool WarningsEnabled
        {
            get
            {
                return this.warningsEnabledField;
            }
            set
            {
                this.warningsEnabledField = value;
            }
        }

        /// <remarks/>
        public bool IsTest
        {
            get
            {
                return this.isTestField;
            }
            set
            {
                this.isTestField = value;
            }
        }

        /// <remarks/>
        public TaxDependencyLevelId TaxDependencyLevelId
        {
            get
            {
                return this.taxDependencyLevelIdField;
            }
            set
            {
                this.taxDependencyLevelIdField = value;
            }
        }

        /// <remarks/>
        public bool InProgress
        {
            get
            {
                return this.inProgressField;
            }
            set
            {
                this.inProgressField = value;
            }
        }

        /// <remarks/>
        public int DefaultLocationId
        {
            get
            {
                return this.defaultLocationIdField;
            }
            set
            {
                this.defaultLocationIdField = value;
            }
        }

        /// <remarks/>
        public string BusinessIdentificationNo
        {
            get
            {
                return this.businessIdentificationNoField;
            }
            set
            {
                this.businessIdentificationNoField = value;
            }
        }

        internal void CopyFrom(ProxyCompany SvcResult)
        {
            accountIdField = SvcResult.AccountId;
            companyIdField = SvcResult.CompanyId;
            companyCodeField = SvcResult.CompanyCode;
            companyNameField = SvcResult.CompanyName;
            createdDateField = SvcResult.CreatedDate;
            createdUserIdField = SvcResult.CreatedUserId;
            entityNoField = SvcResult.EntityNo;
            hasProfileField = SvcResult.HasProfile;
            isActiveField = SvcResult.IsActive;
            isDefaultField = SvcResult.IsDefault;
            isReportingEntityField = SvcResult.IsReportingEntity;
            modifiedDateField = SvcResult.ModifiedDate;
            modifiedUserIdField = SvcResult.ModifiedUserId;
            parentIdField = SvcResult.ParentId;
            sSTEffDateField = SvcResult.SSTEffDate;
            sSTPIDField = SvcResult.SSTPID;
            tINField = SvcResult.TIN;
            regalBankIdField = SvcResult.RegalBankId;
            defaultCountryField = SvcResult.DefaultCountry;
            baseCurrencyCodeField = SvcResult.BaseCurrencyCode;
            roundingLevelIdField = (RoundingLevelId)SvcResult.RoundingLevelId;
            cashBasisAccountingEnabledField = SvcResult.CashBasisAccountingEnabled;

          //for (int Index = 0; Index < SvcResult.Children.Length; Index++)
          //{
          //    ProxyCompany Svcchildren = SvcResult.Children[Index];
          //    Company localCompany = new Company();
          //    localCompany.CopyFrom(Svcchildren);
          //    childrenField.Add(localCompany);
          //}

          //ram - pending to implement
          //for (int Index = 0; Index < SvcResult.Contacts.Length; Index++)
          //{
          //    ProxyCompanyContact SvccContacts = SvcResult.Contacts[Index];
          //    CompanyContact localCompanyContact = new CompanyContact();
          //    localCompanyContact.CopyFrom(SvccContacts);
          //    contactsField.Add(localCompanyContact);
          //}

            //itemsField = (Item[])SvcResult.Items;
            //nexusesField = SvcResult.Nexuses;
            //parentField = (Company)SvcResult.Parent;
            //taxCodesField = SvcResult.TaxCodes;
            //taxRulesField = SvcResult.TaxRules;
            //filingCalendarsField = SvcResult.FilingCalendars;

          warningsEnabledField = SvcResult.WarningsEnabled;
          isTestField = SvcResult.IsTest;
          taxDependencyLevelIdField = (TaxDependencyLevelId)SvcResult.TaxDependencyLevelId;
          inProgressField = SvcResult.InProgress;
          defaultLocationIdField = SvcResult.DefaultLocationId;
          businessIdentificationNoField = SvcResult.BusinessIdentificationNo;
        }

        internal void CopyTo(ProxyCompany proxyCompany)
        {
            proxyCompany.AccountId = accountIdField;
            proxyCompany.CompanyId =  companyIdField;
            proxyCompany.CompanyCode =  companyCodeField;
            proxyCompany.CompanyName =  companyNameField;
            proxyCompany.CreatedDate =  createdDateField;
            proxyCompany.CreatedUserId =  createdUserIdField;
            proxyCompany.EntityNo =  entityNoField;
            proxyCompany.HasProfile =  hasProfileField;
            proxyCompany.IsActive =  isActiveField; 
            proxyCompany.IsDefault =  isDefaultField;
            proxyCompany.IsReportingEntity =  isReportingEntityField;
            proxyCompany.ModifiedDate =  modifiedDateField;
            proxyCompany.ModifiedUserId =  modifiedUserIdField;
            proxyCompany.ParentId =  parentIdField;
            proxyCompany.SSTEffDate =  sSTEffDateField;
            proxyCompany.SSTPID =  sSTPIDField;
            proxyCompany.TIN =  tINField;
            proxyCompany.RegalBankId =  regalBankIdField;
            proxyCompany.DefaultCountry =  defaultCountryField;
            proxyCompany.BaseCurrencyCode =  baseCurrencyCodeField;
            proxyCompany.RoundingLevelId =  (Proxies.AccountSvcProxy.RoundingLevelId) roundingLevelIdField;
            proxyCompany.CashBasisAccountingEnabled =  cashBasisAccountingEnabledField;

            //for (int Index = 0; Index < SvcResult.Children.Length; Index++)
            //{
            //    ProxyCompany Svcchildren = SvcResult.Children[Index];
            //    Company localCompany = new Company();
            //    localCompany.CopyFrom(Svcchildren);
            //    childrenField.Add(localCompany);
            //}

            int idx = 0;
            proxyCompany.Contacts = new ProxyCompanyContact[Contacts.Count];
            foreach (CompanyContact contact in Contacts)
            {
                ProxyCompanyContact svcContact = new ProxyCompanyContact();
                contact.CopyTo(svcContact);
                proxyCompany.Contacts[idx++] = svcContact;
            }

            idx = 0;
            if (Nexuses != null)
            {
                if (Nexuses.Count > 0)
                {
                    proxyCompany.Nexuses = new ProxyNexus[Nexuses.Count];
                    foreach (Nexus nexus in Nexuses)
                    {
                        ProxyNexus svcNexus = new ProxyNexus();
                        nexus.CopyTo(svcNexus);
                        proxyCompany.Nexuses[idx++] = svcNexus;
                    }
                }
            }

            //items =  (Item[])SvcResult.Items;
            //parent =  (Company)SvcResult.Parent;
            //taxCodes =  SvcResult.TaxCodes;
            //taxRules =  SvcResult.TaxRules;
            //filingCalendars =  SvcResult.FilingCalendars;

            proxyCompany.WarningsEnabled = warningsEnabledField;
            proxyCompany.IsTest = isTestField;
            proxyCompany.TaxDependencyLevelId = (Proxies.AccountSvcProxy.TaxDependencyLevelId)taxDependencyLevelIdField;
            proxyCompany.InProgress = inProgressField;
            proxyCompany.DefaultLocationId = defaultLocationIdField;
            proxyCompany.BusinessIdentificationNo = businessIdentificationNoField;
        }
    }

    /// <include file='AccountSvc.Doc.xml' path='adapter/Certificates/class/*' />
    [Guid("29931F0B-BFC7-42c4-B55E-F28332C5DCAC")]
    [ComVisible(true)]
    public class Companies : ReadOnlyArrayList
    {
        /// <include file='AvaCert2Svc.Doc.xml' path='adapter/common/members[@name="InternalConstructor"]/*' />
        internal Companies() { }

        /// <include file='AvaCert2Svc.Doc.xml' path='adapter/collection/members[@name="this"]/*' />
        [DispId(0)]
        public new Company this[int index]
        {
            get
            {
                return (Company)base[index];
            }
        }
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-B55E-F28338C5DCAC")]
    [ComVisible(true)]
    public enum RoundingLevelId
    {

        /// <remarks/>
        Line,

        /// <remarks/>
        Document,
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-B55E-F28332C5DC1C")]
    [ComVisible(true)]
    public class CompanyContact
    {

        private string cityField;

        private int companyIdField;

        private string companyContactCodeField;

        private int companyContactIdField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private string emailField;

        private string faxField;

        private string firstNameField;

        private string lastNameField;

        private string countryField;

        private string line1Field;

        private string line2Field;

        private string line3Field;

        private string middleNameField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private string phoneField;

        private string phone2Field;

        private string postalCodeField;

        private string regionField;

        private string titleField;

        private Company companyField;

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public string CompanyContactCode
        {
            get
            {
                return this.companyContactCodeField;
            }
            set
            {
                this.companyContactCodeField = value;
            }
        }

        /// <remarks/>
        public int CompanyContactId
        {
            get
            {
                return this.companyContactIdField;
            }
            set
            {
                this.companyContactIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string Fax
        {
            get
            {
                return this.faxField;
            }
            set
            {
                this.faxField = value;
            }
        }

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public string Line1
        {
            get
            {
                return this.line1Field;
            }
            set
            {
                this.line1Field = value;
            }
        }

        /// <remarks/>
        public string Line2
        {
            get
            {
                return this.line2Field;
            }
            set
            {
                this.line2Field = value;
            }
        }

        /// <remarks/>
        public string Line3
        {
            get
            {
                return this.line3Field;
            }
            set
            {
                this.line3Field = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public string Phone2
        {
            get
            {
                return this.phone2Field;
            }
            set
            {
                this.phone2Field = value;
            }
        }

        /// <remarks/>
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        public string Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public Company Company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
            }
        }

        internal void CopyTo(ProxyCompanyContact SvccContacts)
        {
            SvccContacts.City = cityField;
            SvccContacts.CompanyId = companyIdField;
            SvccContacts.CompanyContactCode = companyContactCodeField;
            SvccContacts.CompanyContactId = companyContactIdField;
            SvccContacts.CreatedDate = createdDateField;
            SvccContacts.CreatedUserId = createdUserIdField;
            SvccContacts.Email = emailField;
            SvccContacts.Fax = faxField;
            SvccContacts.FirstName = firstNameField;
            SvccContacts.LastName = lastNameField;
            SvccContacts.Country = countryField;
            SvccContacts.Line1 = line1Field;
            SvccContacts.Line2 = line2Field;
            SvccContacts.Line3 = line3Field;
            SvccContacts.MiddleName = middleNameField;
            SvccContacts.ModifiedDate = modifiedDateField;
            SvccContacts.ModifiedUserId = modifiedUserIdField;
            SvccContacts.Phone = phoneField;
            SvccContacts.Phone2 = phone2Field;
            SvccContacts.PostalCode = postalCodeField;
            SvccContacts.Region = regionField;
            SvccContacts.Title = titleField;
            //Ram : following code will go into recursion - need to discuss
            //companyField.CopyTo(SvccContacts.ProxyCompany);
        }
       
    }

    /// <include file='TaxSvc.Doc.xml' path='adapter/Lines/class/*' />
    [Guid("70911C94-A1DF-49d7-984D-972B4528E803")]
    [ComVisible(true)]
    public class CompanyContacts : BaseArrayList
    {
        /// <include file='TaxSvc.Doc.xml' path='adapter/common/members[@name="InternalConstructor"]/*' />
        internal CompanyContacts() { }

        /// <include file='TaxSvc.Doc.xml' path='adapter/Lines/members[@name="Add"]/*' />
        [DispId(0)]
        public void Add(CompanyContact contact)
        {
            //we hide the base member so that we can strongly type our parameter
            try
            {
                base.Add(contact);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("CompanyContact", "Cannot add a null CompanyContact to the collection.");
            }
        }

        /// <include file='TaxSvc.Doc.xml' path='adapter/Lines/members[@name="this"]/*' />
        [DispId(1)]
        public new CompanyContact this[int index]
        {
            get
            {
                //we hide the base member so that we can strongly type our returned object
                return (CompanyContact)base[index];
            }
        }


    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-B56E-F28332C5DC1C")]
    [ComVisible(true)]
    public class Item
    {

        private long itemIdField;

        private string itemCodeField;

        private int companyIdField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private int taxCodeIdField;

        private string descriptionField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private Company companyField;

        private TaxCode taxCodeField;

        /// <remarks/>
        public long ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public string ItemCode
        {
            get
            {
                return this.itemCodeField;
            }
            set
            {
                this.itemCodeField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public int TaxCodeId
        {
            get
            {
                return this.taxCodeIdField;
            }
            set
            {
                this.taxCodeIdField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public Company Company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
            }
        }

        /// <remarks/>
        public TaxCode TaxCode
        {
            get
            {
                return this.taxCodeField;
            }
            set
            {
                this.taxCodeField = value;
            }
        }
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-B55D-F28332C5DC1C")]
    [ComVisible(true)]
    public class TaxCode
    {

        private int taxCodeIdField;

        private string taxCodeValueField;

        private string taxCodeTypeIdField;

        private int companyIdField;

        private string descriptionField;

        private bool isPhysicalField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private string parentTaxCodeField;

        private bool isActiveField;

        private bool isSstCertifiedField;

        /// <remarks/>
        public int TaxCodeId
        {
            get
            {
                return this.taxCodeIdField;
            }
            set
            {
                this.taxCodeIdField = value;
            }
        }

        /// <remarks/>
        public string TaxCodeValue
        {
            get
            {
                return this.taxCodeValueField;
            }
            set
            {
                this.taxCodeValueField = value;
            }
        }

        /// <remarks/>
        public string TaxCodeTypeId
        {
            get
            {
                return this.taxCodeTypeIdField;
            }
            set
            {
                this.taxCodeTypeIdField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public bool IsPhysical
        {
            get
            {
                return this.isPhysicalField;
            }
            set
            {
                this.isPhysicalField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public string ParentTaxCode
        {
            get
            {
                return this.parentTaxCodeField;
            }
            set
            {
                this.parentTaxCodeField = value;
            }
        }

        /// <remarks/>
        public bool IsActive
        {
            get
            {
                return this.isActiveField;
            }
            set
            {
                this.isActiveField = value;
            }
        }

        /// <remarks/>
        public bool IsSstCertified
        {
            get
            {
                return this.isSstCertifiedField;
            }
            set
            {
                this.isSstCertifiedField = value;
            }
        }
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-B55D-F28332C5EC1C")]
    [ComVisible(true)]
    public class Nexus
    {

        private int nexusIdField;

        private int companyIdField;

        private string countryField;

        private string stateField;

        private JurisTypeId jurisTypeIdField;

        private string jurisCodeField;

        private string jurisNameField;

        private string shortNameField;

        private System.DateTime effDateField;

        private System.DateTime endDateField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private NexusTypeId nexusTypeIdField;

        private int accountingMethodIdField;

        private bool hasLocalNexusField;

        private string sourcingField;

        private LocalNexusTypeId localNexusTypeIdField;

        private string taxId;

        /// <remarks/>
        public int NexusId
        {
            get
            {
                return this.nexusIdField;
            }
            set
            {
                this.nexusIdField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public JurisTypeId JurisTypeId
        {
            get
            {
                return this.jurisTypeIdField;
            }
            set
            {
                this.jurisTypeIdField = value;
            }
        }

        /// <remarks/>
        public string JurisCode
        {
            get
            {
                return this.jurisCodeField;
            }
            set
            {
                this.jurisCodeField = value;
            }
        }

        /// <remarks/>
        public string JurisName
        {
            get
            {
                return this.jurisNameField;
            }
            set
            {
                this.jurisNameField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EffDate
        {
            get
            {
                return this.effDateField;
            }
            set
            {
                this.effDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public NexusTypeId NexusTypeId
        {
            get
            {
                return this.nexusTypeIdField;
            }
            set
            {
                this.nexusTypeIdField = value;
            }
        }

        /// <remarks/>
        public int AccountingMethodId
        {
            get
            {
                return this.accountingMethodIdField;
            }
            set
            {
                this.accountingMethodIdField = value;
            }
        }

        /// <remarks/>
        public bool HasLocalNexus
        {
            get
            {
                return this.hasLocalNexusField;
            }
            set
            {
                this.hasLocalNexusField = value;
            }
        }

        /// <remarks/>
        public string Sourcing
        {
            get
            {
                return this.sourcingField;
            }
            set
            {
                this.sourcingField = value;
            }
        }

        /// <remarks/>
        public LocalNexusTypeId LocalNexusTypeId
        {
            get
            {
                return this.localNexusTypeIdField;
            }
            set
            {
                this.localNexusTypeIdField = value;
            }
        }

        /// <remarks/>
        public string  TaxId
        {
            get
            {
                return this.taxId;
            }
            set
            {
                this.taxId = value;
            }
        }

        internal void CopyFrom(ProxyNexus SvcResult)
        {
            nexusIdField = SvcResult.NexusId;
            companyIdField = SvcResult.CompanyId;
            countryField = SvcResult.Country;
            stateField = SvcResult.State;
            jurisTypeIdField = (JurisTypeId)SvcResult.JurisTypeId;
            jurisCodeField = SvcResult.JurisCode;
            jurisNameField = SvcResult.JurisName;
            shortNameField = SvcResult.ShortName;
            effDateField = SvcResult.EffDate;
            endDateField = SvcResult.EndDate;
            createdDateField = SvcResult.CreatedDate;
            createdUserIdField = SvcResult.CreatedUserId;
            modifiedDateField = SvcResult.ModifiedDate;
            modifiedUserIdField = SvcResult.ModifiedUserId;
            nexusTypeIdField = (NexusTypeId)SvcResult.NexusTypeId;
            accountingMethodIdField = SvcResult.AccountingMethodId;
            hasLocalNexusField = SvcResult.HasLocalNexus;
            sourcingField = SvcResult.Sourcing;
            taxId = SvcResult.TaxId;
            localNexusTypeIdField = (LocalNexusTypeId)SvcResult.LocalNexusTypeId;
        }


        internal void CopyTo(ProxyNexus proxyNexus)
        {
            proxyNexus.NexusId = nexusIdField;
            proxyNexus.CompanyId  = companyIdField;
            proxyNexus.Country =countryField;
            proxyNexus.State = stateField;
            proxyNexus.JurisTypeId = (ProxyJurisTypeId)jurisTypeIdField;
            proxyNexus.JurisCode =  jurisCodeField;
            proxyNexus.JurisName = jurisNameField;
            proxyNexus.ShortName = shortNameField;
            proxyNexus.EffDate = effDateField;
            proxyNexus.EndDate = endDateField;
            proxyNexus.CreatedDate = createdDateField;
            proxyNexus.CreatedUserId = createdUserIdField;
            proxyNexus.ModifiedDate = modifiedDateField;
            proxyNexus.ModifiedUserId = modifiedUserIdField;
            proxyNexus.NexusTypeId = (ProxyNexusTypeId)nexusTypeIdField;
            proxyNexus.AccountingMethodId = accountingMethodIdField;
            proxyNexus.HasLocalNexus = hasLocalNexusField;
            proxyNexus.Sourcing = sourcingField;
            proxyNexus.LocalNexusTypeId = (ProxyLocalNexusTypeId)localNexusTypeIdField;
            proxyNexus.TaxId =  taxId;
    }
    }

    /// <include file='AccountSvc.Doc.xml' path='adapter/Certificates/class/*' />
    [Guid("29931F0B-BFC7-42c4-B55E-F23682C5DCAC")]
    [ComVisible(true)]
    public class Nexuses : ReadOnlyArrayList
    {
        /// <include file='AvaCert2Svc.Doc.xml' path='adapter/common/members[@name="InternalConstructor"]/*' />
        internal Nexuses() { }

        /// <include file='AvaCert2Svc.Doc.xml' path='adapter/collection/members[@name="this"]/*' />
        [DispId(0)]
        public new Nexus this[int index]
        {
            get
            {
                return (Nexus)base[index];
            }
        }

        /// <include file='TaxSvc.Doc.xml' path='adapter/Lines/members[@name="Add"]/*' />
        [DispId(30)]
        public void Add(Nexus nexus)
        {
            //we hide the base member so that we can strongly type our parameter
            try
            {
                base.Add(nexus);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("nexus", "Cannot add a null nexus to the collection.");
            }

        }
        
    }

    /// <include file='AccountSvc.Doc.xml' path='adapter/Certificates/class/*' />
    [Guid("1F0C648E-10DE-42D6-B716-82F6301FA567")]
    [ComVisible(true)]
    public class Users : ReadOnlyArrayList
    {
        /// <include file='AvaCert2Svc.Doc.xml' path='adapter/common/members[@name="InternalConstructor"]/*' />
        internal Users() { }

        /// <include file='AvaCert2Svc.Doc.xml' path='adapter/collection/members[@name="this"]/*' />
        [DispId(0)]
        public new User this[int index]
        {
            get
            {
                return (User)base[index];
            }
        }

        /// <include file='TaxSvc.Doc.xml' path='adapter/Lines/members[@name="Add"]/*' />
        [DispId(30)]
        public void Add(User user)
        {
            //we hide the base member so that we can strongly type our parameter
            try
            {
                base.Add(user);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("nexus", "Cannot add a null nexus to the collection.");
            }

        }

    }
    [Guid("D103D42C-49E4-4EA9-A4FF-3C9BDBA5EA60")]
    [ComVisible(true)]
    public class User
    {
        public User()
        {

        }
        private int userIdField;

        private int accountIdField;

        private string userNameField;

        private string firstNameField;

        private string lastNameField;

        private string emailField;

        private string postalCodeField;

        private string passwordField;

        private PasswordStatusId passwordStatusIdField;

        private SecurityRoleId securityRoleIdField;

        private bool isActiveField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private int failedLoginAttemptsField;

        private Account accountField;

        private SecurityRole[] securityRolesField;

        private Permission[] permissionsField;

        private int companyIdField;

        /// <remarks/>
        public int UserId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        /// <remarks/>
        public int AccountId
        {
            get
            {
                return this.accountIdField;
            }
            set
            {
                this.accountIdField = value;
            }
        }

        /// <remarks/>
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }

        /// <remarks/>
        public PasswordStatusId PasswordStatusId
        {
            get
            {
                return this.passwordStatusIdField;
            }
            set
            {
                this.passwordStatusIdField = value;
            }
        }

        /// <remarks/>
        internal SecurityRoleId SecurityRoleId
        {
            get
            {
                return this.securityRoleIdField;
            }
            set
            {
                this.securityRoleIdField = value;
            }
        }

        /// <remarks/>
        public bool IsActive
        {
            get
            {
                return this.isActiveField;
            }
            set
            {
                this.isActiveField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public int FailedLoginAttempts
        {
            get
            {
                return this.failedLoginAttemptsField;
            }
            set
            {
                this.failedLoginAttemptsField = value;
            }
        }

        /// <remarks/>
        public Account Account
        {
            get
            {
                return this.accountField;
            }
            set
            {
                this.accountField = value;
            }
        }

        /// <remarks/>
        internal SecurityRole[] SecurityRoles
        {
            get
            {
                return this.securityRolesField;
            }
            set
            {
                this.securityRolesField = value;
            }
        }

        /// <remarks/>
        internal Permission[] Permissions
        {
            get
            {
                return this.permissionsField;
            }
            set
            {
                this.permissionsField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        internal void CopyFrom(ProxyUser SvcResult)
        {
            userIdField = SvcResult.UserId;
            companyIdField = SvcResult.CompanyId;
            accountIdField = SvcResult.AccountId;
            userNameField = SvcResult.UserName;
            firstNameField = SvcResult.FirstName;
            lastNameField = SvcResult.LastName;
            emailField = SvcResult.Email;
            postalCodeField = SvcResult.PostalCode;
            passwordField = SvcResult.Password;
            isActiveField = SvcResult.IsActive;
            createdDateField = SvcResult.CreatedDate;
            createdUserIdField = SvcResult.CreatedUserId;
            modifiedDateField = SvcResult.ModifiedDate;
            modifiedUserIdField = SvcResult.ModifiedUserId;
            failedLoginAttemptsField = SvcResult.FailedLoginAttempts;
            if (SvcResult.Account!=null)
            {
                accountField = new Account();
            accountField.CopyFrom(SvcResult.Account);
            }
            
            securityRolesField = SvcResult.SecurityRoles;
            permissionsField = SvcResult.Permissions;
            passwordStatusIdField = SvcResult.PasswordStatusId;
            securityRoleIdField = SvcResult.SecurityRoleId;
        }


        internal void CopyTo(ProxyUser _proxyUser)
        {
            _proxyUser.UserId = userIdField;
            _proxyUser.CompanyId = companyIdField;
            _proxyUser.AccountId = accountIdField;
            _proxyUser.UserName = userNameField;
            _proxyUser.FirstName = firstNameField;
            _proxyUser.LastName = lastNameField;
            _proxyUser.Email = emailField;
            _proxyUser.PostalCode = postalCodeField;
            _proxyUser.Password = passwordField;
            _proxyUser.IsActive = isActiveField;
            _proxyUser.CreatedDate = createdDateField;
            _proxyUser.CreatedUserId = createdUserIdField;
            _proxyUser.ModifiedDate = modifiedDateField;
            _proxyUser.ModifiedUserId = modifiedUserIdField;
            _proxyUser.FailedLoginAttempts = failedLoginAttemptsField;
            if (accountField !=null)
            {
                _proxyUser.Account = new ProxyAccount();
                accountField.CopyTo(_proxyUser.Account);
            }
            
            _proxyUser.SecurityRoleId = securityRoleIdField;
            _proxyUser.PasswordStatusId = passwordStatusIdField;
            _proxyUser.Permissions = permissionsField;
            _proxyUser.SecurityRoles = securityRolesField;
        }
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public enum AccessLevel
    {

        /// <remarks/>
        None,

        /// <remarks/>
        System,

        /// <remarks/>
        Site,

        /// <remarks/>
        Account,

        /// <remarks/>
        Company,
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public partial class SecurityRole
    {

        private SecurityRoleId securityRoleIdField;

        private string descriptionField;

        private SecurityRoleId inheritsRoleIdField;

        private AccessLevel accessLevelIdField;

        /// <remarks/>
        public SecurityRoleId SecurityRoleId
        {
            get
            {
                return this.securityRoleIdField;
            }
            set
            {
                this.securityRoleIdField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public SecurityRoleId InheritsRoleId
        {
            get
            {
                return this.inheritsRoleIdField;
            }
            set
            {
                this.inheritsRoleIdField = value;
            }
        }

        /// <remarks/>
        public AccessLevel AccessLevelId
        {
            get
            {
                return this.accessLevelIdField;
            }
            set
            {
                this.accessLevelIdField = value;
            }
        }
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public partial class Permission
    {

        private int permissionIdField;

        private string serviceField;

        private string operationField;

        /// <remarks/>
        public int PermissionId
        {
            get
            {
                return this.permissionIdField;
            }
            set
            {
                this.permissionIdField = value;
            }
        }

        /// <remarks/>
        public string Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
            }
        }

        /// <remarks/>
        public string Operation
        {
            get
            {
                return this.operationField;
            }
            set
            {
                this.operationField = value;
            }
        }
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public enum SecurityRoleId
    {

        /// <remarks/>
        NoAccess,

        /// <remarks/>
        SiteAdmin,

        /// <remarks/>
        AccountOperator,

        /// <remarks/>
        AccountAdmin,

        /// <remarks/>
        AccountUser,

        /// <remarks/>
        SystemAdmin,

        /// <remarks/>
        Registrar,

        /// <remarks/>
        CSPTester,

        /// <remarks/>
        CSPAdmin,

        /// <remarks/>
        SystemOperator,

        /// <remarks/>
        TechnicalSupportUser,

        /// <remarks/>
        TechnicalSupportAdmin,

        /// <remarks/>
        TreasuryUser,

        /// <remarks/>
        TreasuryAdmin,

        /// <remarks/>
        ComplianceUser,

        /// <remarks/>
        ComplianceAdmin,

        /// <remarks/>
        ProStoresOperator,

        /// <remarks/>
        CompanyUser,

        /// <remarks/>
        CompanyAdmin,

        /// <remarks/>
        ComplianceTempUser,

        /// <remarks/>
        ComplianceRootUser,

        /// <remarks/>
        ComplianceOperator,

        /// <remarks/>
        SSTAdmin,
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public enum PasswordStatusId
    {

        /// <remarks/>
        UserCannotChange,

        /// <remarks/>
        UserCanChange,

        /// <remarks/>
        UserMustChange,
    }

    /// <remarks/>
    [Guid("5B97BCEA-0967-4DC0-AA64-67112988BC56")]
    [ComVisible(true)]
    public class Account
    {

        private int accountIdField;

        private int siteIdField;

        private string accountNameField;

        private AccountStatusId accountStatusIdField;

        private System.DateTime effDateField;

        private System.DateTime endDateField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private Service[] servicesField;

        private Site siteField;

        private AddressServiceConfig addressServiceConfigField;

        private FormsServiceConfig formsServiceConfigField;

        private TaxServiceConfig taxServiceConfigField;

        private Company[] companiesField;

        private User[] usersField;

        /// <remarks/>
        public int AccountId
        {
            get
            {
                return this.accountIdField;
            }
            set
            {
                this.accountIdField = value;
            }
        }

        /// <remarks/>
        public int SiteId
        {
            get
            {
                return this.siteIdField;
            }
            set
            {
                this.siteIdField = value;
            }
        }

        /// <remarks/>
        public string AccountName
        {
            get
            {
                return this.accountNameField;
            }
            set
            {
                this.accountNameField = value;
            }
        }

        /// <remarks/>
        internal AccountStatusId AccountStatusId
        {
            get
            {
                return this.accountStatusIdField;
            }
            set
            {
                this.accountStatusIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EffDate
        {
            get
            {
                return this.effDateField;
            }
            set
            {
                this.effDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public Service[] Services
        {
            get
            {
                return this.servicesField;
            }
            set
            {
                this.servicesField = value;
            }
        }

        /// <remarks/>
        public Site Site
        {
            get
            {
                return this.siteField;
            }
            set
            {
                this.siteField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AddressServiceConfig AddressServiceConfig
        {
            get
            {
                return this.addressServiceConfigField;
            }
            set
            {
                this.addressServiceConfigField = value;
            }
        }

        public FormsServiceConfig FormsServiceConfig
        {
            get
            {
                return this.formsServiceConfigField;
            }
            set
            {
                this.formsServiceConfigField = value;
            }
        }

        /// <remarks/>
        public TaxServiceConfig TaxServiceConfig
        {
            get
            {
                return this.taxServiceConfigField;
            }
            set
            {
                this.taxServiceConfigField = value;
            }
        }

        /// <remarks/>
        public Company[] Companies
        {
            get
            {
                return this.companiesField;
            }
            set
            {
                this.companiesField = value;
            }
        }

        /// <remarks/>
        public User[] Users
        {
            get
            {
                return this.usersField;
            }
            set
            {
                this.usersField = value;
            }
        }

        internal void CopyFrom(ProxyAccount SvcResult)
        {
            accountIdField = SvcResult.AccountId;
            siteIdField = SvcResult.SiteId;
            accountNameField = SvcResult.AccountName;
            effDateField = SvcResult.EffDate;
            endDateField = SvcResult.EndDate;            
            createdDateField = SvcResult.CreatedDate;
            createdUserIdField = SvcResult.CreatedUserId;
            modifiedDateField = SvcResult.ModifiedDate;
            modifiedUserIdField = SvcResult.ModifiedUserId;
            accountStatusIdField = SvcResult.AccountStatusId;
            servicesField = SvcResult.Services;
            siteField = SvcResult.Site;
            addressServiceConfigField = SvcResult.AddressServiceConfig;
            formsServiceConfigField = SvcResult.FormsServiceConfig;
            taxServiceConfigField = SvcResult.TaxServiceConfig;
            //company and users pending
        }


        internal void CopyTo(ProxyAccount proxyAccount)
        {
            proxyAccount.AccountId = accountIdField;
            proxyAccount.SiteId = siteIdField;
            proxyAccount.AccountName = accountNameField;
            proxyAccount.EffDate = effDateField;
            proxyAccount.EndDate = endDateField;
            proxyAccount.CreatedDate = createdDateField;
            proxyAccount.CreatedUserId = createdUserIdField;
            proxyAccount.ModifiedDate = modifiedDateField;
            proxyAccount.ModifiedUserId = modifiedUserIdField;

            proxyAccount.AccountStatusId = accountStatusIdField;
            proxyAccount.Services = servicesField;
            proxyAccount.Site = siteField;
            proxyAccount.AddressServiceConfig = addressServiceConfigField;
            proxyAccount.FormsServiceConfig = formsServiceConfigField;
            proxyAccount.TaxServiceConfig = taxServiceConfigField;
            //company and users pending
        }

    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public enum ServiceTypeId
    {

        /// <remarks/>
        None,

        /// <remarks/>
        AvaTaxST,

        /// <remarks/>
        AvaTaxPro,

        /// <remarks/>
        AvaTaxGlobal,

        /// <remarks/>
        AutoAddress,

        /// <remarks/>
        AutoReturns,

        /// <remarks/>
        TaxSolver,

        /// <remarks/>
        AvaTaxCsp,

        /// <remarks/>
        Twe,

        /// <remarks/>
        Mrs,

        /// <remarks/>
        AvaCert,

        /// <remarks/>
        AuthorizationPartner,

        /// <remarks/>
        CertCapture,

        /// <remarks/>
        AvaUpc,

        /// <remarks/>
        AvaCUT,
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public partial class Site
    {

        private int siteIdField;

        private string siteNameField;

        private string cspIdField;

        private System.DateTime effDateField;

        private System.DateTime endDateField;

        /// <remarks/>
        public int SiteId
        {
            get
            {
                return this.siteIdField;
            }
            set
            {
                this.siteIdField = value;
            }
        }

        /// <remarks/>
        public string SiteName
        {
            get
            {
                return this.siteNameField;
            }
            set
            {
                this.siteNameField = value;
            }
        }

        /// <remarks/>
        public string CspId
        {
            get
            {
                return this.cspIdField;
            }
            set
            {
                this.cspIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EffDate
        {
            get
            {
                return this.effDateField;
            }
            set
            {
                this.effDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public enum EcmsCertUseId
    {

        /// <remarks/>
        Ignored,

        /// <remarks/>
        Optional,

        /// <remarks/>
        Required,
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public partial class TaxServiceConfig
    {

        private bool requireMappedItemCodeField;

        private bool requireOriginAddressField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private bool ecmsEnabledField;

        private EcmsCertUseId ecmsCertUseUsField;

        private EcmsCertUseId ecmsCertUseCaField;

        private bool ecmsCompleteCertsRequiredField;

        private string ecmsOverrideCodeField;

        private bool ecmsSstCertsRequiredField;

        private int maxLinesField;

        private bool isJaasDisabledField;

        private System.DateTime sstPolicyOverrideDateField;

        private System.DateTime itemDescPolicyOverrideDateField;

        /// <remarks/>
        public bool RequireMappedItemCode
        {
            get
            {
                return this.requireMappedItemCodeField;
            }
            set
            {
                this.requireMappedItemCodeField = value;
            }
        }

        /// <remarks/>
        public bool RequireOriginAddress
        {
            get
            {
                return this.requireOriginAddressField;
            }
            set
            {
                this.requireOriginAddressField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public bool EcmsEnabled
        {
            get
            {
                return this.ecmsEnabledField;
            }
            set
            {
                this.ecmsEnabledField = value;
            }
        }

        /// <remarks/>
        public EcmsCertUseId EcmsCertUseUs
        {
            get
            {
                return this.ecmsCertUseUsField;
            }
            set
            {
                this.ecmsCertUseUsField = value;
            }
        }

        /// <remarks/>
        public EcmsCertUseId EcmsCertUseCa
        {
            get
            {
                return this.ecmsCertUseCaField;
            }
            set
            {
                this.ecmsCertUseCaField = value;
            }
        }

        /// <remarks/>
        public bool EcmsCompleteCertsRequired
        {
            get
            {
                return this.ecmsCompleteCertsRequiredField;
            }
            set
            {
                this.ecmsCompleteCertsRequiredField = value;
            }
        }

        /// <remarks/>
        public string EcmsOverrideCode
        {
            get
            {
                return this.ecmsOverrideCodeField;
            }
            set
            {
                this.ecmsOverrideCodeField = value;
            }
        }

        /// <remarks/>
        public bool EcmsSstCertsRequired
        {
            get
            {
                return this.ecmsSstCertsRequiredField;
            }
            set
            {
                this.ecmsSstCertsRequiredField = value;
            }
        }

        /// <remarks/>
        public int MaxLines
        {
            get
            {
                return this.maxLinesField;
            }
            set
            {
                this.maxLinesField = value;
            }
        }

        /// <remarks/>
        public bool IsJaasDisabled
        {
            get
            {
                return this.isJaasDisabledField;
            }
            set
            {
                this.isJaasDisabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime SstPolicyOverrideDate
        {
            get
            {
                return this.sstPolicyOverrideDateField;
            }
            set
            {
                this.sstPolicyOverrideDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ItemDescPolicyOverrideDate
        {
            get
            {
                return this.itemDescPolicyOverrideDateField;
            }
            set
            {
                this.itemDescPolicyOverrideDateField = value;
            }
        }
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public partial class FormsServiceConfig
    {

        private int accountIdField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private byte reviewActionDefaultField;

        private byte zeroDollarActionDefaultField;

        private byte worksheetDayField;

        /// <remarks/>
        public int AccountId
        {
            get
            {
                return this.accountIdField;
            }
            set
            {
                this.accountIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public byte ReviewActionDefault
        {
            get
            {
                return this.reviewActionDefaultField;
            }
            set
            {
                this.reviewActionDefaultField = value;
            }
        }

        /// <remarks/>
        public byte ZeroDollarActionDefault
        {
            get
            {
                return this.zeroDollarActionDefaultField;
            }
            set
            {
                this.zeroDollarActionDefaultField = value;
            }
        }

        /// <remarks/>
        public byte WorksheetDay
        {
            get
            {
                return this.worksheetDayField;
            }
            set
            {
                this.worksheetDayField = value;
            }
        }
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public partial class AddressServiceConfig
    {

        private bool isUpperCaseField;

        private bool isJaasDisabledField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        /// <remarks/>
        public bool IsUpperCase
        {
            get
            {
                return this.isUpperCaseField;
            }
            set
            {
                this.isUpperCaseField = value;
            }
        }

        /// <remarks/>
        public bool IsJaasDisabled
        {
            get
            {
                return this.isJaasDisabledField;
            }
            set
            {
                this.isJaasDisabledField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public partial class Service
    {

        private int serviceIdField;

        private int accountIdField;

        private ServiceTypeId serviceTypeIdField;

        private int quantityField;

        private int companyIdField;

        private System.DateTime effDateField;

        private System.DateTime endDateField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        /// <remarks/>
        public int ServiceId
        {
            get
            {
                return this.serviceIdField;
            }
            set
            {
                this.serviceIdField = value;
            }
        }

        /// <remarks/>
        public int AccountId
        {
            get
            {
                return this.accountIdField;
            }
            set
            {
                this.accountIdField = value;
            }
        }

        /// <remarks/>
        public ServiceTypeId ServiceTypeId
        {
            get
            {
                return this.serviceTypeIdField;
            }
            set
            {
                this.serviceTypeIdField = value;
            }
        }

        /// <remarks/>
        public int Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EffDate
        {
            get
            {
                return this.effDateField;
            }
            set
            {
                this.effDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }
    }
    [XmlType(Namespace = "http://avatax.avalara.com/services")]
    public enum AccountStatusId
    {

        /// <remarks/>
        Inactive,

        /// <remarks/>
        Active,

        /// <remarks/>
        Test,

        /// <remarks/>
        New,
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42c4-C55D-F28332C5DC1C")]
    [ComVisible(true)]
    public enum NexusTypeId
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Volunteer,

        /// <remarks/>
        NonVolunteer,

        /// <remarks/>
        SSTVolunteer,

        /// <remarks/>
        SSTNonVolunteer,

        /// <remarks/>
        Collect,

        /// <remarks/>
        Legal,
    }

    /// <remarks/>
    [Guid("29931F1B-BFC8-42D4-B55D-F28332C5DC1C")]
    [ComVisible(true)]
    public enum LocalNexusTypeId
    {

        /// <remarks/>
        Selected,

        /// <remarks/>
        StateAdministered,

        /// <remarks/>
        All,
    }

    /// <remarks/>
    [Guid("20931F1B-BFC8-42D4-B55D-F28332C6DC1C")]
    [ComVisible(true)]
    public enum JurisTypeId
    {

        /// <remarks/>
        STA,

        /// <remarks/>
        CTY,

        /// <remarks/>
        CIT,

        /// <remarks/>
        STJ,

        /// <remarks/>
        CNT,
    }

    /// <remarks/>
    [Guid("20031F1B-BFC8-42c4-B55D-F28332C5DC0C")]
    [ComVisible(true)]
    public enum TaxDependencyLevelId
    {

        /// <remarks/>
        Document,

        /// <remarks/>
        State,

        /// <remarks/>
        TaxRegion,

        /// <remarks/>
        Address,
    }

    /// <remarks/>
    [Guid("20931D1B-BFC8-42c4-B55D-F28332D5DC1C")]
    [ComVisible(true)]
    public class CompanyReturn
    {

        private int yearStartField;

        private long companyReturnIdField;

        private CompanySupportingReturn[] companySupportingReturnField;

        private int companyIdField;

        private string companyNameField;

        private string returnNameField;

        private FilingFrequencyId filingFrequencyIdField;

        private short monthsField;

        private string registrationIdField;

        private string einField;

        private string line1Field;

        private string line2Field;

        private string cityField;

        private string regionField;

        private string postalCodeField;

        private string countryField;

        private string phoneField;

        private string descriptionField;

        private string legalEntityNameField;

        private System.DateTime effDateField;

        private System.DateTime endDateField;

        private int createdUserIdField;

        private System.DateTime createdDateField;

        private int modifiedUserIdField;

        private System.DateTime modifiedDateField;

        private int filingCalendarIdField;

        private FilingTypeId filingTypeIdField;

        private string efilePasswordField;

        private byte prepayPercentageField;

        private string taxTypeIdField;

        private string noteField;

        private string alSignOnField;

        private string alAccessCodeField;

        private string meBusinessCodeField;

        private string iaBenField;

        private string ctRegField;

        private string other1NameField;

        private string other1ValueField;

        private string other2NameField;

        private string other2ValueField;

        private string other3NameField;

        private string other3ValueField;

        private string locationCodeField;

        private OutletTypeId outletTypeIdField;

        /// <remarks/>
        public int YearStart
        {
            get
            {
                return this.yearStartField;
            }
            set
            {
                this.yearStartField = value;
            }
        }

        /// <remarks/>
        public long CompanyReturnId
        {
            get
            {
                return this.companyReturnIdField;
            }
            set
            {
                this.companyReturnIdField = value;
            }
        }

        /// <remarks/>
        public CompanySupportingReturn[] CompanySupportingReturn
        {
            get
            {
                return this.companySupportingReturnField;
            }
            set
            {
                this.companySupportingReturnField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public string CompanyName
        {
            get
            {
                return this.companyNameField;
            }
            set
            {
                this.companyNameField = value;
            }
        }

        /// <remarks/>
        public string ReturnName
        {
            get
            {
                return this.returnNameField;
            }
            set
            {
                this.returnNameField = value;
            }
        }

        /// <remarks/>
        public FilingFrequencyId FilingFrequencyId
        {
            get
            {
                return this.filingFrequencyIdField;
            }
            set
            {
                this.filingFrequencyIdField = value;
            }
        }

        /// <remarks/>
        public short Months
        {
            get
            {
                return this.monthsField;
            }
            set
            {
                this.monthsField = value;
            }
        }

        /// <remarks/>
        public string RegistrationId
        {
            get
            {
                return this.registrationIdField;
            }
            set
            {
                this.registrationIdField = value;
            }
        }

        /// <remarks/>
        public string Ein
        {
            get
            {
                return this.einField;
            }
            set
            {
                this.einField = value;
            }
        }

        /// <remarks/>
        public string Line1
        {
            get
            {
                return this.line1Field;
            }
            set
            {
                this.line1Field = value;
            }
        }

        /// <remarks/>
        public string Line2
        {
            get
            {
                return this.line2Field;
            }
            set
            {
                this.line2Field = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string LegalEntityName
        {
            get
            {
                return this.legalEntityNameField;
            }
            set
            {
                this.legalEntityNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EffDate
        {
            get
            {
                return this.effDateField;
            }
            set
            {
                this.effDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int FilingCalendarId
        {
            get
            {
                return this.filingCalendarIdField;
            }
            set
            {
                this.filingCalendarIdField = value;
            }
        }

        /// <remarks/>
        public FilingTypeId FilingTypeId
        {
            get
            {
                return this.filingTypeIdField;
            }
            set
            {
                this.filingTypeIdField = value;
            }
        }

        /// <remarks/>
        public string EfilePassword
        {
            get
            {
                return this.efilePasswordField;
            }
            set
            {
                this.efilePasswordField = value;
            }
        }

        /// <remarks/>
        public byte PrepayPercentage
        {
            get
            {
                return this.prepayPercentageField;
            }
            set
            {
                this.prepayPercentageField = value;
            }
        }

        /// <remarks/>
        public string TaxTypeId
        {
            get
            {
                return this.taxTypeIdField;
            }
            set
            {
                this.taxTypeIdField = value;
            }
        }

        /// <remarks/>
        public string Note
        {
            get
            {
                return this.noteField;
            }
            set
            {
                this.noteField = value;
            }
        }

        /// <remarks/>
        public string AlSignOn
        {
            get
            {
                return this.alSignOnField;
            }
            set
            {
                this.alSignOnField = value;
            }
        }

        /// <remarks/>
        public string AlAccessCode
        {
            get
            {
                return this.alAccessCodeField;
            }
            set
            {
                this.alAccessCodeField = value;
            }
        }

        /// <remarks/>
        public string MeBusinessCode
        {
            get
            {
                return this.meBusinessCodeField;
            }
            set
            {
                this.meBusinessCodeField = value;
            }
        }

        /// <remarks/>
        public string IaBen
        {
            get
            {
                return this.iaBenField;
            }
            set
            {
                this.iaBenField = value;
            }
        }

        /// <remarks/>
        public string CtReg
        {
            get
            {
                return this.ctRegField;
            }
            set
            {
                this.ctRegField = value;
            }
        }

        /// <remarks/>
        public string Other1Name
        {
            get
            {
                return this.other1NameField;
            }
            set
            {
                this.other1NameField = value;
            }
        }

        /// <remarks/>
        public string Other1Value
        {
            get
            {
                return this.other1ValueField;
            }
            set
            {
                this.other1ValueField = value;
            }
        }

        /// <remarks/>
        public string Other2Name
        {
            get
            {
                return this.other2NameField;
            }
            set
            {
                this.other2NameField = value;
            }
        }

        /// <remarks/>
        public string Other2Value
        {
            get
            {
                return this.other2ValueField;
            }
            set
            {
                this.other2ValueField = value;
            }
        }

        /// <remarks/>
        public string Other3Name
        {
            get
            {
                return this.other3NameField;
            }
            set
            {
                this.other3NameField = value;
            }
        }

        /// <remarks/>
        public string Other3Value
        {
            get
            {
                return this.other3ValueField;
            }
            set
            {
                this.other3ValueField = value;
            }
        }

        /// <remarks/>
        public string LocationCode
        {
            get
            {
                return this.locationCodeField;
            }
            set
            {
                this.locationCodeField = value;
            }
        }

        /// <remarks/>
        public OutletTypeId OutletTypeId
        {
            get
            {
                return this.outletTypeIdField;
            }
            set
            {
                this.outletTypeIdField = value;
            }
        }
    }

    /// <remarks/>
    [Guid("20931D1B-BFC8-49c4-B55D-F28332D5DC1C")]
    [ComVisible(true)]
    public enum FilingFrequencyId
    {

        /// <remarks/>
        Monthly,

        /// <remarks/>
        Quarterly,

        /// <remarks/>
        SemiAnnually,

        /// <remarks/>
        Annually,

        /// <remarks/>
        Bimonthly,

        /// <remarks/>
        Occasional,
    }

    /// <remarks/>
    [Guid("20931F1B-BFC8-42D4-B55D-F28332C5DC1C")]
    [ComVisible(true)]
    public enum FilingTypeId
    {

        /// <remarks/>
        PaperReturn,

        /// <remarks/>
        ElectronicReturn,

        /// <remarks/>
        SER,

        /// <remarks/>
        EFTPaper,

        /// <remarks/>
        PhonePaper,

        /// <remarks/>
        SignatureReady,

        /// <remarks/>
        EfileCheck,
    }

    /// <remarks/>
    [Guid("20991F1B-BFC8-42c4-B55D-F28332C5DC1C")]
    [ComVisible(true)]
    public enum OutletTypeId
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Schedule,

        /// <remarks/>
        Duplicate,
    }

    /// <remarks/>
    [Guid("20931F1D-BFC8-42c4-B55D-F28332C5DC1C")]
    [ComVisible(true)]
    public enum TaxTypeId
    {

        /// <remarks/>
        B,

        /// <remarks/>
        C,

        /// <remarks/>
        S,

        /// <remarks/>
        U,

        /// <remarks/>
        O,

        /// <remarks/>
        I,

        /// <remarks/>
        N,

        /// <remarks/>
        R,

        /// <remarks/>
        F,

        /// <remarks/>
        E,
    }

    /// <remarks/>
    [Guid("20931F1B-BFD8-42c4-B55D-F28332C5DC1C")]
    [ComVisible(true)]
    public enum TaxRuleTypeId
    {

        /// <remarks/>
        RateRule,

        /// <remarks/>
        RateOverrideRule,

        /// <remarks/>
        BaseRule,

        /// <remarks/>
        ExemptEntityRule,

        /// <remarks/>
        ProductTaxabilityRule,

        /// <remarks/>
        NexusRule,
    }

    /// <remarks/>
    [Guid("20931F1B-BFC8-42c4-B65D-F28332C5DC1C")]
    [ComVisible(true)]
    public class TaxRule
    {

        private int taxRuleIdField;

        private int companyIdField;

        private string stateField;

        private string stateCodeField;

        private string countyCodeField;

        private JurisTypeId jurisTypeIdField;

        private string jurisCodeField;

        private string jurisNameField;

        private int taxCodeIdField;

        private string customerUsageTypeField;

        private TaxTypeId taxTypeIdField;

        private TaxRuleTypeId taxRuleTypeIdField;

        private bool isAllJurisField;

        private decimal valueField;

        private decimal capField;

        private decimal thresholdField;

        private string optionsField;

        private System.DateTime effDateField;

        private System.DateTime endDateField;

        private string descriptionField;

        private System.DateTime createdDateField;

        private int createdUserIdField;

        private System.DateTime modifiedDateField;

        private int modifiedUserIdField;

        private bool isStProField;

        private string rateTypeIdField;

        private Company companyField;

        private TaxCode taxCodeField;

        private string countryField;

        private string sourcingField;

        /// <remarks/>
        public int TaxRuleId
        {
            get
            {
                return this.taxRuleIdField;
            }
            set
            {
                this.taxRuleIdField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string StateCode
        {
            get
            {
                return this.stateCodeField;
            }
            set
            {
                this.stateCodeField = value;
            }
        }

        /// <remarks/>
        public string CountyCode
        {
            get
            {
                return this.countyCodeField;
            }
            set
            {
                this.countyCodeField = value;
            }
        }

        /// <remarks/>
        public JurisTypeId JurisTypeId
        {
            get
            {
                return this.jurisTypeIdField;
            }
            set
            {
                this.jurisTypeIdField = value;
            }
        }

        /// <remarks/>
        public string JurisCode
        {
            get
            {
                return this.jurisCodeField;
            }
            set
            {
                this.jurisCodeField = value;
            }
        }

        /// <remarks/>
        public string JurisName
        {
            get
            {
                return this.jurisNameField;
            }
            set
            {
                this.jurisNameField = value;
            }
        }

        /// <remarks/>
        public int TaxCodeId
        {
            get
            {
                return this.taxCodeIdField;
            }
            set
            {
                this.taxCodeIdField = value;
            }
        }

        /// <remarks/>
        public string CustomerUsageType
        {
            get
            {
                return this.customerUsageTypeField;
            }
            set
            {
                this.customerUsageTypeField = value;
            }
        }

        /// <remarks/>
        public TaxTypeId TaxTypeId
        {
            get
            {
                return this.taxTypeIdField;
            }
            set
            {
                this.taxTypeIdField = value;
            }
        }

        /// <remarks/>
        public TaxRuleTypeId TaxRuleTypeId
        {
            get
            {
                return this.taxRuleTypeIdField;
            }
            set
            {
                this.taxRuleTypeIdField = value;
            }
        }

        /// <remarks/>
        public bool IsAllJuris
        {
            get
            {
                return this.isAllJurisField;
            }
            set
            {
                this.isAllJurisField = value;
            }
        }

        /// <remarks/>
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public decimal Cap
        {
            get
            {
                return this.capField;
            }
            set
            {
                this.capField = value;
            }
        }

        /// <remarks/>
        public decimal Threshold
        {
            get
            {
                return this.thresholdField;
            }
            set
            {
                this.thresholdField = value;
            }
        }

        /// <remarks/>
        public string Options
        {
            get
            {
                return this.optionsField;
            }
            set
            {
                this.optionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EffDate
        {
            get
            {
                return this.effDateField;
            }
            set
            {
                this.effDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public int CreatedUserId
        {
            get
            {
                return this.createdUserIdField;
            }
            set
            {
                this.createdUserIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ModifiedDate
        {
            get
            {
                return this.modifiedDateField;
            }
            set
            {
                this.modifiedDateField = value;
            }
        }

        /// <remarks/>
        public int ModifiedUserId
        {
            get
            {
                return this.modifiedUserIdField;
            }
            set
            {
                this.modifiedUserIdField = value;
            }
        }

        /// <remarks/>
        public bool IsStPro
        {
            get
            {
                return this.isStProField;
            }
            set
            {
                this.isStProField = value;
            }
        }

        /// <remarks/>
        public string RateTypeId
        {
            get
            {
                return this.rateTypeIdField;
            }
            set
            {
                this.rateTypeIdField = value;
            }
        }

        /// <remarks/>
        public Company Company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
            }
        }

        /// <remarks/>
        public TaxCode TaxCode
        {
            get
            {
                return this.taxCodeField;
            }
            set
            {
                this.taxCodeField = value;
            }
        }

        /// <remarks/>
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public string Sourcing
        {
            get
            {
                return this.sourcingField;
            }
            set
            {
                this.sourcingField = value;
            }
        }
    }

    /// <remarks/>
    [Guid("20931F1B-BFC8-42c4-B55D-F29332C5DC1C")]
    [ComVisible(true)]
    public class CompanySupportingReturn
    {

        private long companyReturnIdField;

        private int companySupportingReturnIdField;

        private string returnNameField;

        /// <remarks/>
        public long CompanyReturnId
        {
            get
            {
                return this.companyReturnIdField;
            }
            set
            {
                this.companyReturnIdField = value;
            }
        }

        /// <remarks/>
        public int CompanySupportingReturnId
        {
            get
            {
                return this.companySupportingReturnIdField;
            }
            set
            {
                this.companySupportingReturnIdField = value;
            }
        }

        /// <remarks/>
        public string ReturnName
        {
            get
            {
                return this.returnNameField;
            }
            set
            {
                this.returnNameField = value;
            }
        }
    }
 
    /// <remarks/>
    [Guid("20931F1B-BFC8-42c4-B55D-F28332C5DC1C")]
    [ComVisible(true)]
    public class CompanySetting
    {

        private int companySettingIdField;

        private int companyIdField;

        private string setField;

        private string nameField;

        private string valueField;

        /// <remarks/>
        public int CompanySettingId
        {
            get
            {
                return this.companySettingIdField;
            }
            set
            {
                this.companySettingIdField = value;
            }
        }

        /// <remarks/>
        public int CompanyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public string Set
        {
            get
            {
                return this.setField;
            }
            set
            {
                this.setField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [Guid("7B82AF45-2901-4DB2-8A1F-F0BE83F80FA7")]
    [ComVisible(true)]
    public class UserFetchResult : BaseResult
    {

        private Users usersField;

        private int recordCountField;

        /// <remarks/>
        public Users Users
        {
            get
            {
                return this.usersField;
            }
            set
            {
                this.usersField = value;
            }
        }

        /// <remarks/>
        public int RecordCount
        {
            get
            {
                return this.recordCountField;
            }
            set
            {
                this.recordCountField = value;
            }
        }

        internal void CopyFrom(ProxyUserFetchResult SvcResult)
        {
            base.CopyFrom(SvcResult);
            recordCountField = SvcResult.RecordCount;
            //iterate through addresses returned by the web service and move them into
            //    a local address object and local arraylist
            if (SvcResult.Users != null)
            {
                usersField = new Users();
                for (int Index = 0; Index < SvcResult.Users.Length; Index++)
                {
                    ProxyUser SvcUser = SvcResult.Users[Index];
                    User locaUser = new User();
                    locaUser.CopyFrom(SvcUser);
                    usersField.Add(locaUser);
                }
            }
        }

        internal static UserFetchResult CastFromBaseResult(BaseResult baseResult)
        {
            UserFetchResult result = new UserFetchResult();
            result.CopyFrom(baseResult);
            return result;
        }

    }

}
