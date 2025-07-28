@echo off
ECHO Running docker compose...
ECHO.

docker compose up -d

IF %ERRORLEVEL% == 0 (
    ECHO.
    ECHO [+] Containers successfully launched.
) ELSE (
    ECHO.
    ECHO [!] Something went wrong...
)

ECHO.
pause