@echo off
ECHO Running docker compose...
ECHO.

docker compose -f docker-compose.yml -f docker-compose.monitoring.yml up

IF %ERRORLEVEL% == 0 (
    ECHO.
    ECHO [+] Containers successfully launched.
) ELSE (
    ECHO.
    ECHO [!] Something went wrong...
)

ECHO.
pause