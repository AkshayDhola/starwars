#!/bin/bash

echo "📦 Downloading StarWars v1.0.0 for Linux..."

URL="https://github.com/AkshayDhola/starwars/releases/download/v1.0.0/starwars-linux-x64-v1.0.0.tar.gz"
TMP_DIR="/tmp/starwars-download"
INSTALL_DIR="/opt/starwars"
BIN="/usr/local/bin/starwars"

mkdir -p "$TMP_DIR"
cd "$TMP_DIR" || exit

curl -L -o starwars.tar.gz "$URL"

echo "📂 Extracting..."
tar -xzf starwars.tar.gz

echo "🚀 Installing..."
sudo mkdir -p "$INSTALL_DIR"
sudo cp -r ./* "$INSTALL_DIR"
sudo ln -sf "$INSTALL_DIR/starwars" "$BIN"

echo "🎉 Installation complete! Run: starwars"
