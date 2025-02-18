@REM This script is used to install the required dependencies for the frontend project

@REM Create a new directory for the frontend project with vite
set templateName=react-ts
set projectName=frontend
start /wait cmd /c "npx create-vite@latest %projectName% --template %templateName% && exit"

set projectPath=%cd%

cd %projectName%

@REM Install required dependencies for the frontend project
set dependencies=@reduxjs/toolkit,axios,react-router-dom
set devDependencies=@types/react-router-dom,autoprefixer,cssnano,postcss-nested,postcss-scss

for %%f in (%dependencies%) do (
  start cmd /c "npm install %%f && exit"
)

for %%f in (%devDependencies%) do (
  start cmd /c "npm install --save-dev %%f && exit"
)

@REM Create a .env file
type nul > .env

@REM Create Needed Directories

set templatePath=E:\Udemy\React-Repos\React-TypeScript-Template

set excludeFiles=package.json,package-lock.json,tsconfig.json,tsconfig.app.json,tsconfig.node.json,.gitkeep
set excludeFolders=node_modules

set excludeStr=

@REM Loop through files to exclude
for %%f in (%excludeFiles%) do (
  set excludeStr=%excludeStr% /xf "%%f"
)

@REM Loop through folders to exclude
for %%f in (%excludeFolders%) do (
  set excludeStr=%excludeStr% /xd "%%f"
)

@REM Execute robocopy with the proper exclusions
robocopy "%templatePath%" ./ /e %excludeStr%

cd %projectPath%
