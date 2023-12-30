using SAPbobsCOM;
using System;
using System.Collections.Generic;

namespace AutoReconcilationAddon.Services
{
	public class ReconciliationService
	{
		private SAPbobsCOM.Company oCom;
		private SAPbobsCOM.InternalReconciliationsService service;

		public ReconciliationService(SAPbobsCOM.Company oCom)
		{
			this.oCom = oCom;
			this.service = (InternalReconciliationsService)this.oCom.GetCompanyService().GetBusinessService(ServiceTypes.InternalReconciliationsService);
		}

		public InternalReconciliationsService GetService() => this.service;

		public InternalReconciliationOpenTrans GetOpenTransactions(
		  InternalReconciliationOpenTransParams transactionParams)
		{
			return this.service.GetOpenTransactions(transactionParams);
		}

		public InternalReconciliationOpenTransParams GetTransParams(
		  string cardCode,
		  string dateFrom,
		  string dateTo)
		{
			InternalReconciliationOpenTransParams dataInterface = (InternalReconciliationOpenTransParams)this.service.GetDataInterface(InternalReconciliationsServiceDataInterfaces.irsInternalReconciliationOpenTransParams);
			dataInterface.ReconDate = DateTime.Today;
			dataInterface.DateType = ReconSelectDateTypeEnum.rsdtPostDate;
			dataInterface.FromDate = DateTime.ParseExact(dateFrom, "yyyyMMdd", (IFormatProvider)null);
			dataInterface.ToDate = DateTime.ParseExact(dateTo, "yyyyMMdd", (IFormatProvider)null);
			dataInterface.CardOrAccount = CardOrAccountEnum.coaCard;
			dataInterface.InternalReconciliationBPs.Add();
			dataInterface.InternalReconciliationBPs.Item((object)0).BPCode = cardCode;
			return dataInterface;
		}

		public InternalReconciliationOpenTransParams GetLinkedTransParams(
		  string cardCode,
		  string linkedCardCode,
		  string dateFrom,
		  string dateTo)
		{
			InternalReconciliationOpenTransParams dataInterface = (InternalReconciliationOpenTransParams)this.service.GetDataInterface(InternalReconciliationsServiceDataInterfaces.irsInternalReconciliationOpenTransParams);
			dataInterface.ReconDate = DateTime.Today;
			dataInterface.DateType = ReconSelectDateTypeEnum.rsdtPostDate;
			dataInterface.FromDate = DateTime.ParseExact(dateFrom, "yyyyMMdd", (IFormatProvider)null);
			dataInterface.ToDate = DateTime.ParseExact(dateTo, "yyyyMMdd", (IFormatProvider)null);
			dataInterface.CardOrAccount = CardOrAccountEnum.coaCard;
			dataInterface.InternalReconciliationBPs.Add();
			dataInterface.InternalReconciliationBPs.Item((object)0).BPCode = cardCode;
			dataInterface.InternalReconciliationBPs.Add();
			dataInterface.InternalReconciliationBPs.Item((object)1).BPCode = linkedCardCode;
			return dataInterface;
		}

		public InternalReconciliationParams Reconciliate(
		  InternalReconciliationOpenTrans openTransactions,
		  Dictionary<(int, int), double> reconciliatedTransactions)
		{
			bool flag = false;
			foreach (InternalReconciliationOpenTransRow reconciliationOpenTransRow in (IInternalReconciliationOpenTransRows)openTransactions.InternalReconciliationOpenTransRows)
			{
				(int, int) key = (reconciliationOpenTransRow.TransId, reconciliationOpenTransRow.TransRowId);
				if (reconciliatedTransactions.ContainsKey(key))
				{
					flag = true;
					reconciliationOpenTransRow.Selected = BoYesNoEnum.tYES;
					reconciliationOpenTransRow.ReconcileAmount = reconciliatedTransactions[key];
				}
			}
			return !flag ? (InternalReconciliationParams)null : this.service.Add(openTransactions);
		}
	}
}
