namespace Cake.CoverageComparer
{
    internal struct CoveredClassReport
    {
        internal CoveredClassReport(string fullName, decimal? totalCoveredCurrent, decimal? totalCoveredMaster)
        {
            FullName = fullName;
            TotalCoveredCurrent = totalCoveredCurrent;
            TotalCoveredMaster = totalCoveredMaster;
            CoverageDifference = totalCoveredMaster.GetValueOrDefault() - totalCoveredCurrent.GetValueOrDefault();
        }

        internal string FullName { get; }
        internal decimal? TotalCoveredCurrent { get; }
        internal decimal? TotalCoveredMaster { get; }
        internal decimal CoverageDifference { get; }
        internal string CoverageDifferenceEmoji => CoverageDifference switch
        {
            var value when value > 0 => "✅",
            var value when value < 0 => "❌",
            _ => "👁‍"
        };


        public override string ToString()
            => $"{FullName} => current: {TotalCoveredCurrent} - master: {TotalCoveredMaster}";
    }
}
