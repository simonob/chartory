@echo off

@echo *******************************************
@echo * BUILDING SOLUTION IN RELEASE			*
@echo *******************************************

C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release src\Chartory\Chartory\Chartory.csproj

pushd Tools\NuGet

@echo *******************************************
@echo * COPYING BINARIES						*
@echo *******************************************
mkdir .\Chartory\lib\winrt45
mkdir .\Chartory\lib\winrt45\Chartory
mkdir .\Chartory\lib\winrt45\Chartory\Themes
copy ..\..\src\Chartory\Chartory\bin\release\Chartory.dll .\Chartory\lib\winrt45\
copy ..\..\src\Chartory\Chartory\bin\release\Chartory.pri .\Chartory\lib\winrt45\
copy ..\..\src\Chartory\Chartory\bin\release\themes\generic.xaml .\Chartory\lib\winrt45\Chartory\Themes

@echo *******************************************
@echo * BUILDING NUGET PAKCAGE					*
@echo *******************************************
nuget pack Chartory.nuspec -o .\

@echo *******************************************
@echo * DONE BUILDING NUGET - 					*
@echo * DON'T FORGET TO PUBLISH					*
@echo *******************************************

popd

pause