@echo NetStandard 1.0 + net45
copy "Tungsten.ArrayMethods\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.CallResult\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.EventTemplate\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.Lockable\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.Logging\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.Property\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.Threading\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.Threading.Lockers\bin\Release\*.nupkg" ..\NuGet\


@echo NetStandard 1.3 + net45
copy "Tungsten.As\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.Console\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.Encryption\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.From\bin\Release\*.nupkg" ..\NuGet\

@echo NetStandard 1.4 + net45
copy "Tungsten.IO.Pipes\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.LiteDb\bin\Release\*.nupkg" ..\NuGet\

@echo NetStandard 1.5 + net45
copy "Tungsten.Net\bin\Release\*.nupkg" ..\NuGet\

@echo .Net Framework
copy "Tungsten.Domains\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.Firewall\bin\Release\*.nupkg" ..\NuGet\
copy "Tungsten.InterProcess\bin\Release\*.nupkg" ..\NuGet\
pause