dotnet build -v=q --nologo

$programFile = ".\zork1.z3"
$walkthru = ".\ZMachineLib.Feature.Tests\zork1.rand0.ztest"

Function file-exists($path) {
	Test-Path $path -PathType Leaf
}	

if(!(file-exists $programFile)) {
	echo "Downloading $programFile ..."
	Invoke-WebRequest https://raw.githubusercontent.com/historicalsource/zork1/master/COMPILED/zork1.z3 -OutFile .\zork1.z3
}

if((file-exists $walkthru)) {
	echo "Running ZTest against $programFile using test file : $walkthru ..."
	dotnet run -p ZTest -- $programFile $walkthru q
}
else {
	echo "File not found: $walkthru"
}


