#!/bin/bash

# .env file and minimum one file inside ca directory
# must exist to prevent container build error

# create .env file if not exist
if [ ! -f .env ]; then
    touch .env
fi

# create ca directory and one file if not exist
mkdir -p ca
cat << EOF > ca/README.md
put root CA file here with \`.crt\` extension \\
don't delete this file
EOF

# copy custom root CA from default linux ca directory
\cp /usr/local/share/ca-certificates/* ca/ 2>/dev/null || true

# create cache directory for nuget
mkdir -p ~/.devcache/.nuget/packages

# create cache directory for npm
mkdir -p ~/.devcache/.npm
