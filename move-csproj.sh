#!/bin/bash
set -eux

# tarball csproj files, sln files, and NuGet.config
find . \( -name "*.csproj" -o -name "*.sln" -o -name "NuGet.config" \) -print0 \
	    | tar -cvf projectfiles.tar --null -T -
