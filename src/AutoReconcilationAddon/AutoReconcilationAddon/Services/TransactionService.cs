using AutoReconcilationAddon.Models;
using System.Collections.Generic;

namespace AutoReconcilationAddon.Services
{
	public class TransactionService
	{
		private List<Transaction> transactions;
		private List<Transaction> reconciliatedTransactions;
		private Queue<Transaction> debitTransactions;
		private Queue<Transaction> creditTransactions;

		public TransactionService()
		{
			this.transactions = new List<Transaction>();
			this.reconciliatedTransactions = new List<Transaction>();
			this.debitTransactions = new Queue<Transaction>();
			this.creditTransactions = new Queue<Transaction>();
		}
		public void AddTransaction(Transaction t) => transactions.Add(t);

		public void RemoveTransaction(Transaction t) => this.transactions.Remove(t);

		public void ClearTransactions() => this.transactions.Clear();

		public void AutoReconciliate()
		{
			this.debitTransactions.Clear();
			this.creditTransactions.Clear();
			this.reconciliatedTransactions.Clear();

			foreach (Transaction transaction in this.transactions)
			{
				if (transaction.creditOrDebit)
					this.debitTransactions.Enqueue(transaction);
				else
					this.creditTransactions.Enqueue(transaction);
			}

			if (this.debitTransactions.Count == 0 || this.creditTransactions.Count == 0)
				return;

			Transaction transaction1 = this.debitTransactions.Dequeue();
			Transaction transaction2 = this.creditTransactions.Dequeue();

			while (true)
			{
				double amount1 = transaction1.reconcileAmount - transaction1.calculatedReconcileAmount;
				double amount2 = transaction2.reconcileAmount - transaction2.calculatedReconcileAmount;
				if (amount1 > amount2)
				{
					transaction1.AddReconcileAmount(amount2);
					transaction2.AddReconcileAmount(amount2);
					this.reconciliatedTransactions.Add(transaction2);
					if (this.creditTransactions.Count != 0)
						transaction2 = this.creditTransactions.Dequeue();
					else
						break;
				}
				else if (amount1 < amount2)
				{
					transaction2.AddReconcileAmount(amount1);
					transaction1.AddReconcileAmount(amount1);
					this.reconciliatedTransactions.Add(transaction1);
					if (this.debitTransactions.Count != 0)
						transaction1 = this.debitTransactions.Dequeue();
					else
						goto label_17;
				}
				else
				{
					transaction1.AddReconcileAmount(amount1);
					transaction2.AddReconcileAmount(amount2);
					this.reconciliatedTransactions.Add(transaction1);
					this.reconciliatedTransactions.Add(transaction2);
					if (this.debitTransactions.Count != 0 && this.creditTransactions.Count != 0)
					{
						transaction1 = this.debitTransactions.Dequeue();
						transaction2 = this.creditTransactions.Dequeue();
					}
					else
						goto label_9;
				}
			}
			this.reconciliatedTransactions.Add(transaction1);
			return;
		label_17:
			this.reconciliatedTransactions.Add(transaction2);
			return;
		label_9:;
		}
		public Dictionary<(int, int), double> GetReconciliatedTransactions()
		{
			Dictionary<(int, int), double> reconciliatedTransactions = new Dictionary<(int, int), double>();
			foreach (Transaction reconciliatedTransaction in this.reconciliatedTransactions)
				reconciliatedTransactions.Add((reconciliatedTransaction.transID, reconciliatedTransaction.transRowID), reconciliatedTransaction.calculatedReconcileAmount);
			return reconciliatedTransactions;
		}
	}
}
