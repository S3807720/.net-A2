namespace MCBA_Models.Utilities
{

    public static class ConstantVals
    {
        public static readonly int Minimum_Checking = 200;
        public static readonly int Minimum_Savings = 0;
        public static readonly int Open_Checking = 500;
        public static readonly int Open_Savings = 100;
        public static readonly decimal Withdraw_Fee = 0.10m;
        public static readonly decimal Transfer_Fee = 0.20m;
        public static readonly int Max_Free_Transfers = 4;
        public static readonly string Failed = "Insufficient funds";
        public static readonly string Paid = "Paid";
        public static readonly string Due = "Due Soon";
        public static readonly string Finished = "Finished";
        public static readonly string Blocked = "Blocked";
    }
}