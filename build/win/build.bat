:: Windows Build
@echo off

:: Save current directory and move two levels up
pushd .
cd ..\..

:: Set build type
set BUILD_TYPE=win

:: Set optimization flags
set OPTIMIZATIONS=-Os --no-exception-messages --no-debug-info --no-pie --no-stacktrace-data

:: Initialize global variables sets
call build\globalSets.bat

BFLAT build ^
    -r "%SHARED_PATH%\System.CommandLine.dll" ^
    -o "%OUTPUT_PATH%" ^
    %OPTIMIZATIONS%

:: Restore previous directory
popd