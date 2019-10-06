dotnet build -v=q --nologo

$programFile = ".\zork1.z3"
$walkthru = ".\ZMachineLib.Feature.Tests\zork1.rand0.walkthru"

Function file-exists($path) {
	Test-Path $path -PathType Leaf
}	

if(!(file-exists $programFile)) {
	echo "Downloading $programFile ..."
	Invoke-WebRequest https://github.com/historicalsource/zork1/blob/master/COMPILED/zork1.z3?raw=true -OutFile .\zork1.z3
}

if((file-exists $walkthru)) {
	echo "Running $programFile walkthru : $walkthru ..."
	dotnet run -p ZTest -- $programFile $walkthru
}
else {
	echo "File not found: $walkthru"
}


