#!/bin/bash

set -e

# Detect OS
OS="$(uname -s)"
case "$OS" in
    Linux*)     OS=linux;;
    Darwin*)    OS=macos;;
    CYGWIN*|MINGW*|MSYS*) OS=windows;;
    *)          echo "Unsupported OS: $OS"; exit 1;;
esac

# Install act if missing
if ! command -v act &> /dev/null; then
    echo "Installing act..."
    case "$OS" in
        linux)
            curl -sL https://raw.githubusercontent.com/nektos/act/master/install.sh | sudo bash
            ;;
        macos)
            brew install act
            ;;
        windows)
            choco install act-cli
            ;;
    esac
fi

# Install Docker if missing
if ! command -v docker &> /dev/null; then
    case "$OS" in
        linux)
            echo "Installing Docker..."
            curl -fsSL https://get.docker.com -o get-docker.sh
            sudo sh get-docker.sh
            sudo usermod -aG docker $USER
            echo "⚠️  Docker installed. Please log out and back in to apply group changes, then rerun this script."
            exit 1
            ;;
        macos)
            echo "Please install Docker Desktop: https://www.docker.com/products/docker-desktop"
            exit 1
            ;;
        windows)
            echo "Please install Docker Desktop: https://www.docker.com/products/docker-desktop"
            exit 1
            ;;
    esac
fi

# Ensure Docker is running
if ! docker info &> /dev/null; then
    case "$OS" in
        linux)
            sudo systemctl start docker
            ;;
        macos|windows)
            echo "Please start Docker Desktop and ensure it's running."
            exit 1
            ;;
    esac
fi

#docker login docker.io

mkdir -p ./.cache/artifacts
chmod -R 755 ./.cache/artifacts

# Run the GitHub Actions workflow
act -P macos-latest=-self-hosted \
#    --container-options "-v $(pwd)/.cache/act-artifacts:artifacts" \
    --action-cache-path "$(pwd)/.cache" \
    --verbose \
    --rm=false