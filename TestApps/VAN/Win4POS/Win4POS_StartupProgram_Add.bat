@echo off

::set CURPATH=%cd%

::set LinkFileName=Win4POS_Start_Link.lnk

::바로가기가 만들어질 경로와 이름, 확장자 lnk를 합쳐서 변수로 저장
::set lnk=%AppData%\Microsoft\Windows\Start Menu\Programs\Startup\%LinkFileName%

::바로가기를 만들 대상
::set tgpath=%CURPATH%\Win4POS.exe

::arguments (필요하지 않으면 사용하지 않습니다.)
::set "arg=SKIP"

::바로가기 아이콘(기본 아이콘으로 설정 시 이 변수는 설정이 필요 없습니다.)
::set icon=

::바로가기 시작위치(필요하지 않으면 공백으로 설정해주세요.)
::set startLocation=%CURPATH%

::set command=^
::$objshell = New-object -ComObject WScript.Shell;^
::$lnk = $objshell.CreateShortcut('%lnk%');^
::$lnk.TargetPath = '%tgpath%';^
::$lnk.WorkingDirectory = '%startLocation%';^
::$lnk.Arguments = '%arg%';
::if not "%icon%"=="" set command=%command%$lnk.IconLocation ^= '%icon%';
::set command=%command%$lnk.Save();

::powershell -command "& {%command%}"

::echo ----------------------------------------------------------------
::if exist "%lnk%" (
::  echo %LinkFileName% : Add at Startup Program : Success!!!
::) else (
::  echo Error!, %LinkFileName% Not Find File !~~~
::  echo    "%lnk%"
::)
::echo ----------------------------------------------------------------
::pause


REM ----------------------------------------------------------------------------------
REM //////////////////////////////////////////////////////////////////////////////////
REM ----------------------------------------------------------------------------------

Win4POS_Alive.exe ADD_STARTPROG_WIN4POS

::바로가기가 만들어질 경로와 이름, 확장자 lnk를 합쳐서 변수로 저장
set LinkFileName=Win4POS_Start_Link.lnk
set lnk=%AppData%\Microsoft\Windows\Start Menu\Programs\Startup\%LinkFileName%
echo ----------------------------------------------------------------
echo %LinkFileName% : Add at Startup Program : Success!!!
echo ----------------------------------------------------------------
pause



