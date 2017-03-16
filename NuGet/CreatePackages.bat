@mkdir Tungsten\lib\net45
@mkdir Tungsten\lib\netstandard1.4
@mkdir Tungsten\lib\uap10.0
@mkdir Tungsten\lib\portable-net45+win+wpa81+MonoAndroid10+MonoTouch10+xamarinios10+xamarinmac20
nuget pack Tungsten

@mkdir Tungsten.Net\lib\netstandard1.4
nuget pack Tungsten.Net

pause