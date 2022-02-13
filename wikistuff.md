# Overview
For information on getting started, jump to the Getting Started section below.

Squiggle is two primary things:
* A *very* basic scripting language
* A way to read and run the language at runtime.

## Squiggle "language"
Squiggle has two main concepts - `lines` and `commands`.

### Lines
Squiggle code is broken into lines, with each line holding one command.
```
Speaker: Hello, I'm on a line.
Speaker 2: Hello, I'm on another line!
```
Squiggle code is executed line by line, top to bottom, with each line holding a single command. Note that there are no semicolons to end lines. 

#### Valid Squiggle
The **only** thing Squiggle *really* cares about is that the data you pass in *must* be seperated with `new line` characters. You get this "for free" if you're loading text from a file, but for strings, make sure your lines are seperated (insert `\n` at the end of your line).

### Commands
Each line of Squiggle is considered to be a `command`. Squiggle supports two types of commands out of the box - Dialog and Custom.

Squiggle text can be composed of as many command types as you like, freely mixing them with each other:
```
[playSound chime 1]
Speaker: Hello! Thanks for visiting!
[playSound chime 2]
Another Speaker: There's the bell, time to go!
[playSound chime 3]
[playSound endDialog]
```

#### Dialog Commands
Dialog in Squiggle consists of two components, a "speaker" and their actual dialog. These two components are seperated by a colon (`:`).

Here's an example:
```
Fran: Hello! My name is Fran.
```
When run through Squiggle, Squiggle will extract `Fran` as the "speaker" and `Hello! My name is Fran.` as their dialog. Dialog commands make it easy to get setup with Squiggle and validate that it's working in your game, and also provide a common use case for this type of tool out of the box.

Commands give you a format to pass data from Squiggle source to your game, but what you do with this information is up to you. Squiggle just ensures everything is given to you correctly and at the right time.

#### Custom Commands
While dialog commands are useful for simple back and forth dialog between characters (or your own inner voice), Squiggle also exposes a way to call arbitrary commands in your game, from Squigle code.

Here's an example (this will work out of the box):
```
[debug logState]
```
This command will instruct Squiggle to find the command tagged `debug` and pass it the argument `logState` (more on this later).

Custom commands in Squiggle have two major parts, and the actual command name, followed by arguments to pass to that command.
* Calling a command must start with a `[` and end with a `]`.
* The first part of the command has to be the proper tagged command name (more on this later)
* Arguments are seperated by a space, meaning all arguments have to be a single "word"

Commands can contain as many arguments as you like:
```
[setSound mysoundname play 2]
```
This command will instruct Squiggle to find the command tagged `setSound` and pass it the argument `mysoundname`, `play`, and `2`.

### Squiggle "Source"/"Scripts" Format
A major benefit of Squiggle is that it *does not care* how it recieves text, meaning any collection of text is a possible valid Squiggle file. This means you can feed it text in any way you see fit:
* Load text from file
* Pass it the contents of a `string` in your game
* Link it up to a `Text` component in Unity
* Read from server
* etc.

## Squiggle Runner
Squiggle ships with a "Runner" that will attempt to execute anything you send to it as Squiggle code. Each passed in script will be executed line by line, from top to bottom. 

The "runner" gives you a lot of freedom to decide when processing a line/command is "done", so you can easily block execution of lines in a script if need be. Once you're done, Squiggle will resume. See more on this below in the Getting Started section.

# Guide

## Absolute Basics
Squiggle works primarily in an event-driven, command pattern style execution. Wrapping your head around that may be difficult if you're coming purely from Unity update-loop centric development, but we promise you'll be fine.

With that said, there are only a few Squiggle events that you really need to care about:
* `CommandExecutionComplete`

## Basic Dialog
If you only want to use Squiggle for a dialog system, all you need to do is listen to Squiggle's static `OnNewDialogCommand` action
The most basic implementation of Squiggle in your code requires only one thing: telling 
Working with Squiggle requires really only one thing, which is knowledge of how C# `Actions` work. When a Squiggle command executes, you (the implementer) decides what that execution means by a

## Listening For Commands
Squiggle's runner is an idomatic implementation of the Command Pattern. Squiggle Commands themselves are commands that get executed, and require some specific overrides.

Namely, this comes in in figuring out when a command is "done", that way the Runner can continue execution for the next line. Squiggle gives you a default timer buffer if a command has no callback, but we expect that for any non-trivial use of Squiggle, you'll hook into your own events. Here's how that works.

### Speaker Command Callbacks


### Custom Command Signature

# Why
See the section below on comparisons to other prominent dialog editors for Unity.

For Cantata, we had a few primary needs:
* The game has a built-in editor, and we wanted the ability to author working dialog at runtime
* We needed the ability to load in arbitary scripts from source files
* We needed the ability to inject commands in the dialog flow

The needs of the first bullet there meant that Yarn, Twine, Ink, etc. were all not possible. Even if those were available, we wanted a solution that was a bit more spartan — we didn't want to defer control flow to a tool we didn't know the internals of, and wanted all the internal execution of the code be easy to follow and hook into.

With this, Squiggle was born.

## VS. Twine/Yarn

variables
branching
built-in display
runtime authoring (no compile)
agnostic "runner"

It's worth saying that when this project was started, the new version of Yarn wasn't yet out. In reviewing it recently, it's come very far since the version of Yarn that was previously available and is much close to what Squiggle offers now.

# Event Flow
Runner gets a new command
Runners passes command to function
Runner waits to hear CommandExecutionComplete from command

# Authoring Commands
While you get Speaker commands for free, custom commands will likely be the main source of any code your write to support your Squiggle scripts.

With that in mind, let's write a command!
```
[doSomething setMood happy 3]
```
Commands point to code that you've written in your own game in order to handle the commands. Squiggle doesn't ship with any commands by default. Here's a sample command:
```c#
[SquiggleCommand("doSomething")]
public class DoSomethingCommand : SquiggleCommand
{
    public string FMODEventPath => GetConstPathOrFMODPath(Args[1]);
    public DoSomethingCommand(string[] args) : base(args){}
    public override void Execute()
    {
        //handle your command logic here
        GameManager.AudioManager.PlayOneShotFromFMODPath(FMODEventPath);
        CommandExecutionComplete?.Invoke();
    }
}
```


## What it is
Command parsing with Sprache + Command Pattern + Reflection
