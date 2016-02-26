cd "%1"
IF EXIST bin\PJCAdmin2.dll (
  DEL bin\PJCAdmin2.dll
)
IF EXIST bin\PJCAdmin.dll (
  exit /B 1
)
IF EXIST bin\PJCAdmin2.dll (
  exit /B 1
)