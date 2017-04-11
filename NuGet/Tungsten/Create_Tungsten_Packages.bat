@echo Creating All Tungsten Packages
cd ..
nuget pack Tungsten\Tungsten.Classic.nuspec
nuget pack Tungsten\Tungsten.nuspec

nuget pack Tungsten\Tungsten.Universal.nuspec
nuget pack Tungsten\Tungsten.Portable.nuspec
nuget pack Tungsten\Tungsten.Standard.nuspec

pause