namespace Cake.CoverageComparer
{
    internal struct CoveredClass
    {
        internal CoveredClass(string fullName, decimal totalCovered, bool isFromMasterBranch)
        {
            FullName = fullName;
            TotalCovered = totalCovered;
            IsFromMasterBranch = isFromMasterBranch;
        }

        internal string FullName { get; }
        internal decimal TotalCovered { get; }
        internal bool IsFromMasterBranch { get; }
    }

}
