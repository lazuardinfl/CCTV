#!/bin/bash

sudo chown vscode:vscode ~/.nuget
dotnet dev-certs https
dotnet restore
cd ClientApp
npm install
