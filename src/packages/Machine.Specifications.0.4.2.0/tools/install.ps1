param($installPath, $toolsPath, $package, $project)
$project.Object.References | Where-Object { $_.Name -eq 'Machine.Specifications.TDNetRunner' } | ForEach-Object { $_.Remove() }