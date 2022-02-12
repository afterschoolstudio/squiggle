# Squiggle

Squiggle ("**S**uper **Q**uick **G**ame **L**anguage") is a simple scripting language (and runtime parser) built for use inside of any C# game engine or framework (Unity, Monogame, etc.). With minimal setup, you easily start authoring custom dialog and commands for use in your game. Here's an example:

```
[playSound waiterBell 2]
Customer: Hello! May I place an order?
Hostess: Sure, sit over here.
[playAnimation moveHostess]
[setLights dim]
```

If you're familar with tools like Yarn or Twine, Squiggle is like a "lite" version of those, offering a subset of their features in order to be as quick and simple as possible.

Because Squiggle doesn't automatically give you things you get for free in those other tools, Squiggle is *slightly* more advanced, and you will need to be comfortable with code to use it. 

If you're looking for an easy-to-use, plug-and-play dialog system, I highly recommend looking into either Yarn, Twine, or Ink! If you're comfortable with code though and still interested, keep reading for an overview of the tool, how to get started using it, or how it compares to the previously mentioned tools.

# Overview
For information on getting started, jump to the Getting Started section below.

Squiggle is two primary things:
* A *very* basic scripting "language"
* A way to run those Squiggle scripts at runtime

See below for a breakdown of each of these

## Squiggle "language"
Squiggle has two main concepts - `lines` and `commands`.

### Lines
Squiggle code is broken into lines, with each line holding one command.
```
Speaker: Hello, I'm on a line.
Speaker 2: Hello, I'm on another line!
```
The code itself is executed line by line, from top to bottom. Note that there are no semicolons to end lines. If you're loading Squiggle text from file, you get this for free. However if you're building a script in code, you'll need to insert newline characters into your strings so Squiggle knows its a new line/commmand.

### Commands
Each line of Squiggle is considered to be a command. Squiggle supports two types of commands out of the box - Dialog and Custom.

#### Dialog Commands
Dialog in Squiggle consists of two components, a "speaker" and their actual dialog, seperated by a colon (`:`). Here's an example:
```
Fran: Hello! My name is Fran.
```
When run through Squiggle, Squiggle will extract `Fran` as the "speaker", and the text after the `:`, `Hello! My name is Fran.`, as their dialog. Unlike Yarn or Twine, what happens with that information is fully up to you to implement for your own game! Squiggle stops short of being too prescriptive about what you do its information.

#### Custom Commands
While Squiggle would be fine as-is with just the dialog portion, it also has the ability to call arbitrary code from its text. Here's an example:
```
[playSound mysoundname]
```
This command will instruct Squiggle to find the command tagged `playSound` and pass it the parameter `mysoundname` (more on this later).

Commands can contain as many parameters as you like:
```
[setSound mysoundname play]
```
This command will instruct Squiggle to find the command tagged `setSound` and pass it the parameter `mysoundname` and `play`.

Like Dialog commands above, what you do with this information is up to you, Squiggle just ensures everything is given to you properly and at the right time.

### Squiggle "Source"/"Scripts"
A major benefit of Squiggle is that it *does not care* how it recieves text, meaning any collection of "text" is a possible valid Squiggle file. This means you can give it text in any way you see fit:
* Load text from file
* Pass it the contents of a `string` in your game
* Link it up to a `Text` component in Unity
* Read from server
* etc.

Squiggle text can be composed of as many dialog and commands lines as you like, freely mixing them with each other:
```
[playSound chime 1]
Speaker: Hello! Thanks for visiting!
[playSound chime 2]
Another Speaker: There's the bell, time to go!
[playSound chime 3]
[playSound endDialog]
```
Squiggle *is* perscriptive about what it wants from formatting though, so make sure you have all `[]` properly closed, etc.

### Valid Squiggle
The **only** thing Squiggle *really* cares about is that the data you pass must be seperated with new line characters. You get this "for free" if you're loading text from a file, but for strings, make sure your lines are seperated (insert `\n` at the end of your line).


## Squiggle Runner
Squiggle ships with a "Runner" that will execute any valid Squiggle code you pass to it. Each passed in script will be executed line by line, from top to bottom. 

The "runner" gives you a lot of freedom to decide when processing a line/command is "done", so you can easily block execution of lines in a script if need be. Once you're done, Squiggle will resume. See more on this below in the Getting Started section.

# Getting Started
To get started in Unity, you can download the Unity package here:

You'll find three example scenes in the project, each demonstrating a different way to load and execute Squiggle code:
* Input Text Box - Write and execute Squiggle directly in play mode
* Monobehaviour - Write squiggle in a public monobeahvior string field
* Scriptable Object - Author squiggle in an SO
* Streaming Assets - Load a squiggle file from StreamingAssets

The code samples in each of these scenes show you how you can author Squiggle code and run it at runtime, and also provide samples to show you how you can tie into execution of the runner.

## Listening For Commands
Squiggle's runner is an idomatic implementation of the Command Pattern. Squiggle Commands themselves are commands that get executed, and require some specific overrides.

Namely, this comes in in figuring out when a command is "done", that way the Runner can continue execution for the next line. Squiggle gives you a default timer buffer if a command has no callback, but we expect that for any non-trivial use of Squiggle, you'll hook into your own events. Here's how that works.

### Speaker Command Callbacks


### Custom Command Signature


# Use Cases
While Squiggle was primarily designed as a dialog tool, you can think of it a *bit* like a code calling "playbook". From this lens, you can see how it could be used for many other use cases:

* Dialog Systems
* AI Routines
* Unit Testing
* Build Instructions
* etc.

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
