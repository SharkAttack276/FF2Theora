:: Windows Build (IL)
@echo off

:: Save current directory and move two levels up
pushd .
cd ..\..

:: Set build type
set BUILD_TYPE=win

:: Set optimization flags
set OPTIMIZATIONS=--no-debug-info

:: Initialize global variables sets
call build\globalSets.bat

BFLAT build-il ^
    -r "%SHARED_PATH%\System.CommandLine.dll" ^
    -o "%OUTPUT_PATH%" ^
    %OPTIMIZATIONS% ^
    @"%SOURCE_PATH%"

:: Restore previous directory
popd