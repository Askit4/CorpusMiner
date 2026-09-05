# CorpusMiner — Brand Assets

This repository contains the approved visual identity assets for **CorpusMiner**, an Askit4 research-oriented application for academic corpus mining.

AI coding agents MUST use these assets and rules instead of inventing, redrawing, approximating, or replacing the CorpusMiner identity.

## Source of truth

Visual/UI rules:

`/docs/design/DESIGN_SYSTEM.md`

Approved visual reference board:

`/assets/brand/reference/CorpusMiner_Brand_Guide.png`

For application interfaces, `DESIGN_SYSTEM.md` is the canonical source for colors, typography, components, spacing, charts, accessibility, layout, and interaction patterns.

---

## Logos and icons

### Main horizontal logo

Path:

`/assets/brand/logos/corpusminer-logo-horizontal.png`

Use for:
- application header / navigation branding
- login and splash screens
- landing pages
- reports or exports where product identification is required

Rules:
- preserve aspect ratio
- never stretch
- never recolor
- never recreate using plain text or another icon
- never crop the symbol or wordmark unintentionally
- keep enough clear space around the logo

---

### Application icon

Path:

`/assets/brand/logos/corpusminer-app-icon.png`

Use for:
- product/app icon
- compact navigation branding
- PWA icon
- launcher or square icon contexts

The icon combines the book, search/magnifying-glass, document, and network-node concepts of the CorpusMiner identity.

---

### Light application icon

Path:

`/assets/brand/logos/corpusminer-app-icon-light.png`

Use when a light-background icon variant is specifically needed.

Do not use this as the default browser favicon when the dark app icon provides better recognition at small sizes.

---

### Browser favicon

Paths:

`/assets/brand/logos/corpusminer-favicon-32.png`

`/assets/brand/logos/corpusminer-favicon-64.png`

For web applications, CorpusMiner MUST use the official app icon as the browser tab favicon rather than a generic framework icon.

Typical HTML example:

```html
<link rel="icon" type="image/png" sizes="32x32" href="/assets/brand/logos/corpusminer-favicon-32.png" />
<link rel="icon" type="image/png" sizes="64x64" href="/assets/brand/logos/corpusminer-favicon-64.png" />
```

For Next.js, configure the icon through application metadata or the framework favicon convention while preserving the official CorpusMiner icon.

---

## Brand reference board

Path:

`/assets/brand/reference/CorpusMiner_Brand_Guide.png`

Use this image as a **visual reference**, not as a UI screenshot to reproduce pixel-for-pixel.

It documents:
- logo and icon direction
- dashboard visual language
- color palette
- typography
- UI components
- hero/splash style
- illustration style
- data visualization style
- card style
- logo variations

Agents SHOULD inspect this board when creating a new major UI surface or when visual intent is unclear.

---

## Color system

Do NOT sample or approximate colors from screenshots or logo images. Use the values defined in `DESIGN_SYSTEM.md`.

### Primary
- Deep Blue: `#0F172A`
- Azure Blue: `#2563EB`
- Cyan: `#06B6D4`
- Teal: `#14B8A6`

### Secondary
- Purple: `#8B5CF6`
- Indigo: `#6366F1`
- Orange: `#F59E0B`

### Neutral
- App background: `#F8FAFC`
- Surface/cards: `#FFFFFF`
- Border: `#E2E8F0`
- Primary text: `#1E293B`
- Secondary text: `#64748B`
- Disabled/subtle: `#94A3B8`

---

## Typography

Preferred font: **Inter**.

Fallback:

`system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif`

Typical weights:
- 400 body
- 500 controls
- 600 section headings
- 700 key metrics / page titles

Avoid oversized marketing typography inside the research application.

---

## Visual identity rules for AI agents

For ANY work involving UI, frontend, dashboards, forms, login screens, reports, charts, networks, visualizations, or web content, AI coding agents MUST:

1. Read `/docs/design/DESIGN_SYSTEM.md` before implementation.
2. Read `/docs/design/BRAND_ASSETS.md` before implementation.
3. Use official assets under `/assets/brand/`.
4. Use the CorpusMiner app icon as browser favicon.
5. Never invent or substitute a CorpusMiner logo.
6. Never derive brand colors by visually sampling screenshots.
7. Preserve the visual personality: academic, scientific, modern, data-driven, trustworthy, technical, minimal, and exploratory.
8. Prefer scientific clarity and traceability over visual novelty.
9. Keep data visualizations accessible and restrained; no 3D charts or rainbow palettes.
10. Preserve provenance and reproducibility cues in research workflows.
11. Target WCAG AA and never encode meaning by color alone.

---

## Production asset note

The PNG logo/icon files in this repository are approved raster derivatives of the current CorpusMiner visual reference board. If official vector master assets (SVG/PDF/AI) are produced later, add them under `/assets/brand/logos/` and update this document; do not silently replace the visual identity.

## Askit4 corporate endorsement

CorpusMiner is a product of **Askit4 - IT Solutions**.

Official website:

`https://askit4.com/`

Official Askit4 logo in this repository:

`/assets/brand/logos/askit4-logo.png`

### Brand hierarchy

Primary product brand:

`CorpusMiner`

Corporate / endorsement brand:

`Askit4`

Preferred attribution:

**A product of Askit4**

### Use the Askit4 logo for
- login / authentication screens
- public product landing pages
- footer attribution
- About / product information screens
- legal / privacy / terms pages
- corporate ownership attribution

### Do not
- replace the CorpusMiner logo with the Askit4 logo
- use the Askit4 logo as the CorpusMiner browser favicon
- recolor, redraw, distort, crop, or approximate the Askit4 logo
- infer CorpusMiner UI colors from the Askit4 logo
- present CorpusMiner as an independent legal entity unless legal documentation explicitly establishes that status

The CorpusMiner design system remains authoritative for application UI. Askit4 is an endorsement and ownership identity.

### Public corporate reference

The current public Askit4 website identifies the business as `Askit4 - IT Solutions`, provides the contact email `domingo.rojas@askit4.com`, lists the location as Costa Rica, San Jose, Moravia, and displays Legal ID `3-102-794806` and an `ASKITFOR` copyright notice.

Because the public site uses both `Askit4` and `ASKITFOR`, the exact legal entity name that must appear in binding terms and privacy notices MUST be confirmed before production publication. Do not infer that relationship in legal text.
