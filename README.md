# ZMachineLib - Original Readme.md content

[![Build status](https://ci.appveyor.com/api/projects/status/ig4abc31j6imrypw?svg=true)](https://ci.appveyor.com/project/BrianPeek/zmachinelib)

An incomplete (ZMachine)[https://en.wikipedia.org/wiki/Z-machine] interpreter written in C#.  This library supports v1-v5, but v5 is not complete.  The code is a bit rough around the edges in places but I will continue working on it.  See the ConsoleZMachine project for info on how to use the library.

# Why This Fork

* I was bored
* As an exercise in refactoring legacy code.
* A way for me to play some of the old Infocom games.. 


# Approach

The core approach was to attempt to refactor lots of pieces of code that grab bytes from the machine "memory" (a `byte[]`), in to clearly named objects that abstracted the byte array handling away as much as possible.

To this end some of the core underlying concepts of the the ZMachine have been abstracted into objects such as the ZDictionary, ZAbbreviations, ZObjectTree available in a ZMachine as well some of a machines core functionality in handling variables and arguments via the Variable and Operand Manager classes.

All of these are held in a root ZMemory object which maintains the byte[] array containing both the code and data of a ZMachine program.

Individual operations (Ops) that the ZMachine supports get and set the values required via the nice OO 'Z' abtractions described above rather than directly manipulating the byte array.

# Architecture

A zmachine program's execution is a simple loop getting the next byte code instruction and executing it. Input and outputs are effectively interrupts and block the execution until the input or output is received/finished.

Whilst this is fine in todays generally multi-core environments, we still all use a single-core, single-thread machine when we use browser scripting. This means to "run" the zmachine in a browser it would run until the zprogram exits (i.e. when the user 'quits' a game) effectively locking the browser UI thread.

To cater for this ZMachine can also be run in a sequential mode where by it will run until the next input from the user is required at which the machine continued with user input passed in.



## ZmachineLib

The core library containing the code to run a ZMachine program from it bytecode.

### Debugging

* I've added some custom debugging supporting in the form of `!!` commands which can be entered whilst playing a game to dump out parts of the ZMachine state. use `!!` to see supported commands
* This is WIP and needs some attention, better handling around the debug command parsing, extended output for object instead of just the name. other commands as required.

## ZDump

Utility to dump useful information about a ZMachine program file.

## ZPlay

Simple console application to run ZMachine programs files.

# Testing

* It's very easy to break something when messing with address and pointers to a virtual memory. mess a pointer up and you won't know until something else later fails and it can be hard to debug. Test and commit often, reverting has saved me many times from a big mess up.
* There is an ongoing effort to get unit-tests written for each operation.
* There are some feature tests that send games commands for a specific game (I've used Zork I) and check that the game responds as expected.
* I found a special command(in Zork I at least, not checked elsewhere yet) called `#rand` that seems to allow me to set a seed for testing so that the play-thru is consistent, by consistent we mean that any random in-game events while happen at the same time/place for a given seed.
* An, almost complete, run-thru (339/350 points) of Zork I is implemented as feature test to exercise the ZMachine V3 support.


# NOTES

* Refactoring an ancient byte-code interpreter to something more "objecty" is not necessarily the best idea but it's a fun excercise in extreme refactoring of legacy code, note the original C# that I forked from isn't that old, but the ZMachine was designed 40 years ago, when every bit and byte counted and much of the design of the operation code set reflects thats.
* Whilst the "ZMachine" does use the same bytecode as any other ZMachine interpreter might, underneath, it's uses OO/SOLID/XP code and abstractions to represent the underlying concepts of the ZMachine and its Operations, such as Objects, Properties and Dictionaries etc.
* I've only concentrated on V3 files initially and whilst there is some of the old, incomplete v5 code still around I'm intending to formalise an approach to handling the version differences once I'm happy the V3 implementation is solid.

# REFERENCES

* Heavy use was made of (The ZMachine Standards document)[http://inform-fiction.org/zmachine/standards/z1point1/index.html], amazing effort over the years on that, great job!
* Forked and refactored from, and therefore heavily based on, Brian Peek's (C# ZMachineLib)[https://github.com/BrianPeek/ZMachineLib]

# ACQUIRING ZPROGRAM FILES

* To avoid any copyright issues, this repo contains no actual games files, there are various sources available for these, a great starting point is here on github at [Historical Source](https://github.com/historicalsource)

The powershell sample below will retrieve Zork I, II & III in V3 ZMachine compatible with both `ZDump` & `ZPlay`

```
Invoke-WebRequest https://github.com/historicalsource/zork1/blob/master/COMPILED/zork1.z3?raw=true -OutFile .\zork1.z3
Invoke-WebRequest https://github.com/historicalsource/zork2/blob/master/COMPILED/zork2.z3?raw=true -OutFile .\zork2.z3
Invoke-WebRequest https://github.com/historicalsource/zork3/blob/master/COMPILED/zork3.z3?raw=true -OutFile .\zork3.z3
```

There are many other V3 programs available in the historical source archive

# Todos

Some things I've been thinking about doing;

* Full Operation logging (via .NET Core logging ofc)
* Better save game handling
	* Can prompt for filename in IUserIO/IFileIO hooks
	* Offer default filename

* Command line options for ZPlay (to control logging amongst other things)
* Command line options for ZDump to allow individual dump sections to be switched on/off etc. Maybe some hex/dec options
* More "tree" like display to ZDump's object tree dump

## Logging

Several types of logging could be useful, includng;

* game play logging, logs all game text and user inputs to a file
* operation logging, logs all Operations executed
* trace/debug/exception logging, typical logging you might see in any program

### Game Play Logging

Hook in to the IUserIO interface, log all Print()'s and the results of Reads() to single file. Prefix user input with '> '

Probably just open our own file in the IUserIO hooks

### Operation Logging (ZMachine debug log)

Logs the inputs and outputs of every operation, giving a debug trace of ZMachine execution

Possibly have terse, verbose modes;

* terse mode - single line of detail
* verbose mode - expanded detail of everything that happened in a operation

### trace/debug/exception logging

Typical logging of other areas of the code as required. 
Use standard ILogger patterns and approaches

