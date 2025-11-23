#!/bin/bash
# install.sh for Linux/macOS

echo "Starting Star Wars game installation..."

# Check for root privileges
if [ "$(id -u)" != "0" ]; then
   echo "This script must be run as root (use sudo ./install.sh)" 1>&2
   exit 1
fi

INSTALL_DIR="/opt/starwars"
BIN_DIR="/usr/local/bin"
EXECUTABLE_NAME="starwars" # Ensure your actual executable is named 'starwars'

mkdir -p "$INSTALL_DIR"
cp -r * "$INSTALL_DIR"
chmod +x "$INSTALL_DIR/$EXECUTABLE_NAME"

# Create a symbolic link so 'starwars' command works globally
ln -sf "$INSTALL_DIR/$EXECUTABLE_NAME" "$BIN_DIR/$EXECUTABLE_NAME"

echo "Installation complete. You can now run 'starwars' from any terminal."
