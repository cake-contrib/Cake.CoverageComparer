using AngleSharp;
using AngleSharp.Dom;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CakeNamespaceImport("Cake.CoverageComparer")]
namespace Cake.CoverageComparer
{
    public static class SimpleCoverageComparer
    {
        private static readonly CultureInfo _enUsCulture = CultureInfo.GetCultureInfo("en-US");

        /// <summary>
        /// Read index.htm reports and calculate diffence
        /// </summary>
        /// <param name="context"></param>
        /// <param name="upstream">index.htm path from upstream branch</param>
        /// <param name="current">index.htm path from current branch</param>
        /// <returns>HTML table in markdown format</returns>
        [CakeMethodAlias]
        public static async Task<string> CompareCoverage(this ICakeContext context, FilePath upstream, FilePath current)
        {
            var fromCurrentTable = await ExtractTableFromHtml(current.FullPath);
            var fromMasterTable = await ExtractTableFromHtml(upstream.FullPath);
            var coveredFiles = ExtractCoveredClasses(fromCurrentTable, isFromMaster: false)
                .Concat(ExtractCoveredClasses(fromMasterTable, isFromMaster: true));

            var coverageResult = from item in coveredFiles.GroupBy(x => x.FullName)
                                 let fromMaster = item.FirstOrDefault(x => x.IsFromMasterBranch)
                                 let fromCurrent = item.FirstOrDefault(x => !x.IsFromMasterBranch)
                                 select new CoveredClassReport(fromCurrent.FullName ?? fromMaster.FullName, fromCurrent.TotalCovered, fromMaster.TotalCovered);

            var sb = new StringBuilder()
                .AppendLine("File | Current branch coverage | Upstream branch coverage | Difference")
                .AppendLine("---- | ----------------------  | ------------------------ | ----------");
            foreach (var item in coverageResult.Where(x => x.CoverageDifference != 0))
                sb.AppendLine($"{item.FullName} | {item.TotalCoveredCurrent:N} | {item.TotalCoveredMaster:N} | {item.CoverageDifference / 100:P} {item.CoverageDifferenceEmoji}");
            var result = sb.ToString();
            return result;
        }

        private static async Task<IElement> ExtractTableFromHtml(string path)
        {
            var ctx = BrowsingContext.New(Configuration.Default);
            var doc = await ctx.OpenAsync(req => req.Content(File.ReadAllText(path)));
            return doc.QuerySelector("table.overview.table-fixed.stripped");
        }

        private static IEnumerable<CoveredClass> ExtractCoveredClasses(IElement table, bool isFromMaster)
            => from tr in table.QuerySelectorAll("tbody tr")
               let fields = tr.QuerySelectorAll("td").ToArray()
               where fields.Length == 12
               select new CoveredClass(fields[0].TextContent.Trim(), decimal.Parse(fields[5].TextContent.Trim().Replace("%", ""), _enUsCulture), isFromMaster);
    }

}
