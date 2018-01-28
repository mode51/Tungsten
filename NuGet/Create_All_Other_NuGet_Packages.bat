@echo Creating All Other NuGet Packages

rem nuget pack Tungsten\Tungsten.nuspec
nuget pack Tungsten.Net\Tungsten.Net.nuspec
nuget pack Tungsten.Net.RPC\Tungsten.Net.RPC.nuspec
nuget pack Tungsten.Domains\Tungsten.Domains.nuspec
nuget pack Tungsten.Firewall\Tungsten.Firewall.nuspec
nuget pack Tungsten.IO.Pipes\Tungsten.IO.Pipes.nuspec
nuget pack Tungsten.LiteDb\Tungsten.LiteDb.nuspec
nuget pack Tungsten.WPF\Tungsten.WPF.nuspec
nuget pack Tungsten.WPF.Metro\Tungsten.WPF.Metro.nuspec

pause