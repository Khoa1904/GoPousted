@echo off

::set CURPATH=%cd%
::set LinkFileName=Win4POS_Start_Link.lnk

::바로가기가 만들어질 경로와 이름, 확장자 lnk를 합쳐서 변수로 저장
::set lnk=%AppData%\Microsoft\Windows\Start Menu\Programs\Startup\%LinkFileName%

::echo ----------------------------------------------------------------

::if exist "%lnk%" (
::  DEL "%lnk%"
::  echo  %LinkFileName% : Remove at Startup Program : Success!!!
::) else (
::  echo %LinkFileName% : Already Removed at Startup Program : Success!!!
::  echo    "%lnk%"
::)

::echo ----------------------------------------------------------------

::pause


REM ----------------------------------------------------------------------------------
REM //////////////////////////////////////////////////////////////////////////////////
REM ----------------------------------------------------------------------------------

set CURPATH=%cd%
set LinkFileName=Win4POS_Start_Link.lnk
set lnk=%AppData%\Microsoft\Windows\Start Menu\Programs\Startup\%LinkFileName%
echo ----------------------------------------------------------------
Win4POS_Alive.exe REMOVE_STARTPROG_WIN4POS
echo  %LinkFileName% : Removed at Startup Program : Success!!!
echo ----------------------------------------------------------------
pause

