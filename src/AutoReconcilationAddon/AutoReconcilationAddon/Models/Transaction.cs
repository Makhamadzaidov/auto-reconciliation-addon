namespace AutoReconcilationAddon.Models
{
	public class Transaction
	{
		public int transID;
		public int transRowID;
		public bool creditOrDebit;
		public double reconcileAmount;
		public double calculatedReconcileAmount;
		public bool isNegative;

		public Transaction(int transID, int transRowID, bool creditOrDebit, double reconcileAmount)
		{
			this.transID = transID;
			this.transRowID = transRowID;
			this.creditOrDebit = creditOrDebit;
			this.reconcileAmount = reconcileAmount;
			if (this.reconcileAmount >= 0.0)
				return;
			this.isNegative = true;
			this.creditOrDebit = !this.creditOrDebit;
			this.reconcileAmount *= -1.0;
		}
		public void AddReconcileAmount(double amount)
		{
			if (this.isNegative)
				this.calculatedReconcileAmount -= amount;
			else
				this.calculatedReconcileAmount += amount;
		}
	}
}
