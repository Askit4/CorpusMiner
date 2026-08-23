#!/usr/bin/env bash
set -euo pipefail

echo "AFDS-AZ: preparing agent CLI tools..."

missing_apt=()
command -v jq >/dev/null 2>&1 || missing_apt+=(jq)
command -v rg >/dev/null 2>&1 || missing_apt+=(ripgrep)

if [[ ${#missing_apt[@]} -gt 0 ]]; then
  sudo apt-get update -qq
  sudo apt-get install -y -qq "${missing_apt[@]}"
fi

packages=()
command -v claude >/dev/null 2>&1 || packages+=("@anthropic-ai/claude-code@latest")
command -v codex >/dev/null 2>&1 || packages+=("@openai/codex@latest")

if [[ ${#packages[@]} -gt 0 ]]; then
  npm install -g --no-fund --no-audit "${packages[@]}"
fi

printf "AGENTS="
for tool in claude codex copilot; do
  if command -v "$tool" >/dev/null 2>&1; then printf "%s:ok " "$tool"; else printf "%s:missing " "$tool"; fi
done
printf "\n"
