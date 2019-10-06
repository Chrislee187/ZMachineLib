# ZTest

A simple console application that uses the ZPlay.exe to run a ZMachine program and automatically supplying inputs and verifying outputs to the programs standard input and output streams. Verify that the supplied commands produce the supplied output. Input/Output test data is supplied via simple text file format (see below).

NB. The feature tests use a similar technique (with Mocks, instead of redirected stdin/out streams) and the same test text files.


# USAGE

```
ZTest zprogram.zfile zmachine.walkthru [quiet]
```

walkthru files are texts files containing the inputs and outputs to send to the zprogram file.

* blank lines are ignored
* a line starting with '#' is considered a comment and is ignored
* a line starting with '>' is considered a command, the '>' is removed and the command is sent to the Players stdin stream.
* all others lines are considered to be the output expected from the Players stdout stream. Expected text must occur before the next command occurs. 
* if expected text between commands is not found the test has failed and the failure details are reported.
* By default tests run in verbose mode, and all inputs and outputs of the ZMachine are echo'd to the Console. supply ANY third argument to ZTest will activate quiet mode where by the inputs and outputs are NOT echo'd to the Console, and a simple progress display is shown instead. Failure details are, of course, still reported.

Run the simple Powershell script `run_ztests.ps1` to see it in action


# Approach

Reads a set of inputs and expected outputs from the text file,then creates a process with redirected standard input and output to run the ZMachine program (using ZPlay.exe) supplying the inputs and checking that the outputs are as expected.

Exits with -1 if the test fails

