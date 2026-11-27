@echo off
setlocal
rem Zen Browser Portable - alternative launcher (does the same as PortableZen.exe).
rem To use another profile, change profile1 below (for example to profile2).
set "PROFILE=profile1"

set "ZEN_EXE=%~dp0app\win\zen.exe"
set "PROFILE_DIR=%~dp0data\%PROFILE%"

if not exist "%ZEN_EXE%" (
    echo Zen Browser could not be found:
    echo   %ZEN_EXE%
    echo Make sure the app\win folder contains the Zen Browser files.
    pause
    exit /b 1
)

if not exist "%PROFILE_DIR%" mkdir "%PROFILE_DIR%"

rem "start" lets this window close right away instead of waiting for the browser.
start "" "%ZEN_EXE%" -profile "%PROFILE_DIR%" %*
