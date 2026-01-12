#!/bin/bash



if [ "$EUID" -ne 0 ]; then 
    echo "Please run with sudo"
    exit 1
fi

echo "Installing prerequisites..."

# Detect OS
if [ -f /etc/os-release ]; then
    . /etc/os-release
    OS=$NAME
fi

# Update packages
if [[ "$OS" == *"Ubuntu"* ]] || [[ "$OS" == *"Debian"* ]]; then
    apt-get update -qq
    PKG="apt-get"
elif [[ "$OS" == *"Fedora"* ]] || [[ "$OS" == *"Red Hat"* ]] || [[ "$OS" == *"CentOS"* ]]; then
    dnf update -y -q
    PKG="dnf"
else
    echo "Unsupported OS"
    exit 1
fi

# Install .NET 8 SDK
if ! command -v dotnet &> /dev/null; then
    if [[ "$OS" == *"Ubuntu"* ]] || [[ "$OS" == *"Debian"* ]]; then
        wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
        dpkg -i packages-microsoft-prod.deb
        rm packages-microsoft-prod.deb
        apt-get update -qq
        apt-get install -y -qq dotnet-sdk-8.0
    elif [[ "$OS" == *"Fedora"* ]]; then
        dnf install -y -q dotnet-sdk-8.0
    fi
fi

# Docker
read -p "Install Docker? (y/n) " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    if [[ "$OS" == *"Ubuntu"* ]] || [[ "$OS" == *"Debian"* ]]; then
        apt-get install -y -qq ca-certificates curl
        install -m 0755 -d /etc/apt/keyrings
        curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
        chmod a+r /etc/apt/keyrings/docker.asc
        echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | tee /etc/apt/sources.list.d/docker.list > /dev/null
        apt-get update -qq
        apt-get install -y -qq docker-ce docker-ce-cli containerd.io docker-compose-plugin
    elif [[ "$OS" == *"Fedora"* ]]; then
        dnf -y -q install dnf-plugins-core
        dnf config-manager --add-repo https://download.docker.com/linux/fedora/docker-ce.repo
        dnf install -y -q docker-ce docker-ce-cli containerd.io docker-compose-plugin
    fi
    systemctl start docker
    systemctl enable docker
fi

# Install EF tools
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools"
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc

# Restore project
if [ -f "VALHÄUS.sln" ]; then
    dotnet restore VALHÄUS.sln
fi

echo ""
echo "Setup complete!"
echo "Run: dotnet run --project VALHÄUS/VALHAUS.csproj"
