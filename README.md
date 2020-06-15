# Cake.CoverageComparer

A simple coverage comparer that compares opecover html report

# Examples

```csharp
Task("CompareCoverage")
    .Does(async () =>
{
    //                                   main branch coverage       current branch coverage
    var result = await CompareCoverage("./coverage_upstream.html", "./coverageOutput/index.html");
    Information(result);
});
```

if the coverage from main branch is inside a zip file, then you can do something like that

```csharp
Task("CompareCoverage")
    .Does(async () =>
{
    using(var file = System.IO.Compression.ZipFile.OpenRead(@"C:\temp\coverage.zip"))
    using(var reader = new StreamReader(file.GetEntry("index.html").Open()))
    using(var writer = new StreamWriter("./coverage_upstream.html"))
        writer.Write(reader.ReadToEnd());
    
    var result = await CompareCoverage("./coverage_upstream.html", "./coverageOutput/index.html");
    Information(result);
});
```

The result from CompareCoverage is a html markdown table

```
File | Current branch coverage | Upstream branch coverage | Difference
---- | ----------------------  | ------------------------ | ----------
MyProject.Models.AccessGroup | 80,00 | 81,00 | 1,00% ✅
MyProject.Models.AccessGroup2 | 81,00 | 0,00 | -81,00% ❌
MyProject.Models.AccessGroupRole | 55,50 | 54,50 | -1,00% ❌
MyProject.Models.AccessGroupRole2 | 0,00 | 54,50 | 54,50% ✅
```

File | Current branch coverage | Upstream branch coverage | Difference
---- | ----------------------  | ------------------------ | ----------
MyProject.Models.AccessGroup | 80,00 | 81,00 | 1,00% ✅
MyProject.Models.AccessGroup2 | 81,00 | 0,00 | -81,00% ❌
MyProject.Models.AccessGroupRole | 55,50 | 54,50 | -1,00% ❌
MyProject.Models.AccessGroupRole2 | 0,00 | 54,50 | 54,50% ✅
