﻿#r "FakeLib.dll"
open Fake
open System

// -------------------------------------------------------------------------------------------------------------------------------------------------

let targetDir = "target/"
let buildDir = targetDir @@ "build/"
let testBuildDir = targetDir @@ "test/"

let testProjectFile = "SecondConsoleApplication.Test.Unit/SecondConsoleApplication.Test.Unit.csproj"

// -------------------------------------------------------------------------------------------------------------------------------------------------

let projectFile = "SecondConsoleApplication/SecondConsoleApplication.csproj"

// -------------------------------------------------------------------------------------------------------------------------------------------------

let PrintYellow infoMessage =
    let curColor = Console.ForegroundColor
    Console.ForegroundColor <- ConsoleColor.Yellow
    printf "%s" infoMessage
    Console.ForegroundColor <- curColor

// -------------------------------------------------------------------------------------------------------------------------------------------------

Description "Clean the target directory"
Target "Clean" (fun _ -> 
    CleanDir targetDir
)

// -------------------------------------------------------------------------------------------------------------------------------------------------

Description "Compiles the project file"
Target "Build" (fun _ ->

    MSBuildHelper.build(fun x -> 
        { x  with 
            Verbosity = Some Minimal
            Properties =["OutputPath", "../" @@ buildDir; "Configuration", getBuildParamOrDefault "buildMode" "Debug"]
            NodeReuse = false}) projectFile
    |> ignore    
)

// -------------------------------------------------------------------------------------------------------------------------------------------------
Description "Run unit tests without category 'LongRunning'"
Target "Test" (fun _ ->
    MSBuildHelper.build(fun x -> 
        { x  with 
            Verbosity = Some Minimal
            Properties =["OutputPath", "../" @@ testBuildDir; "Configuration", getBuildParamOrDefault "buildMode" "Debug"]
            NodeReuse = false}) testProjectFile
    |> ignore                
    
    !!(testBuildDir @@ "**/*.Test.Unit.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             ExcludeCategory = "SkipInFake|LongRunning"
             ShowLabels = false
             OutputFile = testBuildDir @@ "UnitTestResults_SecondeConsoleApplication.xml" })
)

// -------------------------------------------------------------------------------------------------------------------------------------------------
/// Prints all available targets.
let listTargets() =
    let curColor = Console.ForegroundColor
    Console.ForegroundColor <- ConsoleColor.Green
    printf "Available targets:\n"
    TargetDict.Values
      |> Seq.iter (fun target -> 
            Console.ForegroundColor <- ConsoleColor.Yellow
            printf " %s\n" target.Name
            Console.ForegroundColor <- ConsoleColor.Gray
            printf " %s\n" (if target.Description <> null then target.Description + "\n" else "")            
            )
    Console.ForegroundColor <- curColor

// -------------------------------------------------------------------------------------------------------------------------------------------------
// Default target
Description "Display all available targets"
Target "help" (fun _ ->
    printfn "Run \"fake <TargetName>\" to run a target.\n\n"

    listTargets()
)

// -------------------------------------------------------------------------------------------------------------------------------------------------

"Clean"
==> "Test"
==> "Build"

// -------------------------------------------------------------------------------------------------------------------------------------------------
RunTargetOrDefault "help"
