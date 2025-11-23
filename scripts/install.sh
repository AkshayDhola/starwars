#!/usr/bin/env bash

set -e

REPO="AkshayDhola/starwars"
VERSION="v1.0.0"
BINARY_NAME="starwars"
ZIP_NAME="starwars-linux-x64-v1.0.0.tar.gz"

echo "📦 Downloading $BINARY_NAME $VERSION ..."

curl -L -o $ZIP_NAME "https://github.com/$REPO/releases/download/$VERSION/$ZIP_NAME"

echo "📂 Extracting..."
unzip -o $ZIP_NAME -d /tmp/$BINARY_NAME

echo "🚚 Moving binary to /usr/local/bin ..."
sudo mv /tmp/$BINARY_NAME/$BINARY_NAME /usr/local/bin/$BINARY_NAME

echo "🧹 Cleaning up..."
rm -rf /tmp/$BINARY_NAME $ZIP_NAME

chmod +x /usr/local/bin/$BINARY_NAME

echo ""
echo "🎉 Installation complete!"
echo "Run '$BINARY_NAME --help' to get started."
