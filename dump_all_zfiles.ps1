dotnet build -v=q --nologo

function dumpFiles {
	param($files)

	foreach ($file in $files) 
	{
		$dump = $file.Name + ".zdump"
		echo "Performing ZDUMP: $dump"
		dotnet run -p ZDump -- $file >$dump

		$dump = $file.Name + ".infodump"
		echo "Performing INFODUMP: $dump"
		infodump -f $file >$dump
	}
}

$zFolder ="\\NAS\nas\Vault\Infocom Files\zFiles\"

$z2files = @(Get-ChildItem ($zFolder + "*.z2"))
$z3files = @(Get-ChildItem ($zFolder + "*.z3"))
$z4files = @(Get-ChildItem ($zFolder + "*.z4"))
$z5files = @(Get-ChildItem ($zFolder + "*.z5"))

#dumpFiles $z2files
dumpFiles $z3files
#dumpFiles $z4files
#dumpFiles $z5files