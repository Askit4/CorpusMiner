# CorpusMiner — Design System

> Canonical UI/design reference for CorpusMiner. Derived from the original `CorpusMiner_Look_and_Feel.md` and the approved visual brand board in `assets/brand/reference/CorpusMiner_Brand_Guide.png`.


## 1. Purpose
CorpusMiner is a research-oriented web application for importing, cleaning, normalizing, deduplicating, exploring, and later mining academic corpora from Scopus and Web of Science.

The interface should feel like a scientific instrument rather than a generic admin portal: precise, calm, modern, transparent, and evidence-driven.

## 2. Brand idea
**CorpusMiner = academic corpus + mining + discovery**

Core concepts:
- Research corpus
- Evidence
- Networks
- Discovery
- Traceability
- Reproducibility
- Scientific rigor

Suggested tagline:
**Explore. Unify. Discover.**

Alternative:
**From corpus to evidence.**

## 3. Logo direction
The visual identity combines:
- An open book or document stack = academic literature
- A magnifying glass = inspection/search
- Connected nodes = networks, bibliometrics, conceptual relationships
- Subtle mining metaphor = discovery rather than literal industrial mining

The logo should work:
- In the top-left navigation
- As a square app icon
- On light and dark backgrounds
- In monochrome

Avoid cartoon styling. Keep it geometric, compact, and professional.

## 4. Visual personality
Keywords:
- Academic
- Scientific
- Modern
- Data-driven
- Trustworthy
- Technical
- Minimal
- Exploratory

Do not make it look like:
- A consumer AI chatbot
- A crypto dashboard
- A generic Bootstrap admin panel
- A flashy startup landing page

## 5. Color system

### Primary
- Deep Blue: `#0F172A`
  - Main navigation
  - Dark headers
  - High-contrast text

- Azure Blue: `#2563EB`
  - Primary actions
  - Active navigation
  - Scopus-related visual accents when appropriate

- Cyan: `#06B6D4`
  - Data highlights
  - Network nodes
  - Secondary emphasis

- Teal: `#14B8A6`
  - Success states
  - Validated/clean records

### Secondary
- Purple: `#8B5CF6`
  - WoS-related visual accents
  - Conceptual/theoretical analysis

- Indigo: `#6366F1`
  - Charts and network layers

- Orange: `#F59E0B`
  - Warnings
  - Possible duplicates
  - Review-required records

### Neutral
- App background: `#F8FAFC`
- Surface/cards: `#FFFFFF`
- Border: `#E2E8F0`
- Primary text: `#1E293B`
- Secondary text: `#64748B`
- Disabled/subtle: `#94A3B8`

## 6. Typography
Preferred font:
- **Inter**

Fallback:
- `system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif`

Weights:
- 400 body
- 500 controls
- 600 section headings
- 700 key metrics / page titles

Typography should be compact and readable. Avoid oversized marketing typography inside the application.

## 7. Layout

### Desktop
Use a left navigation rail + top utility bar + main research workspace.

Recommended structure:

```text
┌───────────────────────────────────────────────────────────────┐
│ CorpusMiner          Search…                    User / Export │
├────────────┬──────────────────────────────────────────────────┤
│ Dashboard  │                                                  │
│ Import     │             Main Research Workspace              │
│ Corpus     │                                                  │
│ Analysis   │                                                  │
│ Networks   │                                                  │
│ Concepts   │                                                  │
│ Reports    │                                                  │
│            │                                                  │
│ Settings   │                                                  │
└────────────┴──────────────────────────────────────────────────┘
```

Navigation width: ~220–250px.

Main content should use generous whitespace and a 12-column responsive grid.

## 8. Core UI principles

### 8.1 Scientific transparency
Every derived result should make its provenance visible.

Examples:
- Source: Scopus / WoS / Both
- DOI
- Original record
- Normalized record
- Duplicate-match reason
- Processing step
- Timestamp / pipeline version when relevant

### 8.2 Progressive disclosure
Do not show every technical field at once.

Default:
- Title
- Authors
- Year
- Source
- DOI
- Keywords
- Abstract preview

Expandable:
- Raw metadata
- Normalized metadata
- references
- affiliations
- deduplication evidence

### 8.3 Data first
Use charts, tables, graphs, and filters before decorative content.

### 8.4 Reproducibility
The UI should make the pipeline feel deterministic:
`Import → Normalize → Deduplicate → Validate → Explore → Mine`

## 9. Dashboard
The dashboard should immediately answer:
- How many raw records?
- How many came from Scopus?
- How many came from WoS?
- How many duplicates?
- How many unique records?
- What years are represented?
- What document types dominate?
- What are the main keywords?

Recommended metric cards:
- Total records
- Scopus
- Web of Science
- Duplicates
- Unique corpus

Recommended visuals:
- Publications by year
- Source overlap
- Document type distribution
- Top keywords
- Top journals
- Corpus processing status

## 10. Import experience
The import screen should support two clear drop zones:

**Scopus**
- CSV / supported export formats
- Blue accent

**Web of Science**
- Plain text / Excel / supported export formats
- Purple accent

After upload:
1. Detect source
2. Validate columns
3. Show parsed record count
4. Flag parsing warnings
5. Preview 5–10 records
6. Confirm import

Never silently discard fields.

## 11. Corpus table
The corpus browser is a core screen.

Features:
- Fast search
- Filters
- Sorting
- Column chooser
- Source badge
- Duplicate status
- DOI link
- Detail drawer
- Raw vs normalized toggle

Suggested columns:
- Internal ID
- Title
- First author
- Year
- Journal
- DOI
- Scopus
- WoS
- Duplicate group
- Status

Use virtualized tables if needed.

## 12. Status language
Use explicit status badges:
- Imported
- Normalized
- Duplicate
- Merged
- Needs review
- Validated

Avoid vague labels such as “Processed”.

## 13. Network and concept-map style
Later analysis screens should use graph visualization with restrained styling.

Node examples:
- Actor
- Theory
- Construct
- Technology
- Outcome
- Paper

Suggested semantic shapes:
- Theory = hexagon
- Construct = circle
- Actor = rounded rectangle
- Technology = square
- Paper = small document node

Edges should support:
- Co-occurrence
- Explicitly uses
- Contains construct
- Associated with
- Predicts / influences when later supported

Every edge should be clickable and reveal the underlying papers.

## 14. Charts
Charts should prioritize interpretation.

Rules:
- No 3D charts
- Avoid rainbow palettes
- Use consistent source colors
- Always show labels/tooltips
- Provide downloadable underlying data when feasible
- Use accessible contrast
- Support light background by default

## 15. Cards
Cards should be flat and subtle.

Style:
- White background
- 1px neutral border
- 10–14px radius
- Very subtle shadow or no shadow
- 16–24px inner padding

Do not use excessive gradients inside data cards.

## 16. Buttons
Primary:
- Azure blue
- White text

Secondary:
- White
- Neutral border
- Dark text

Destructive:
- Red only for irreversible actions

Avoid more than one primary action per visual section.

## 17. Iconography
Use a consistent line-icon library such as:
- Lucide
- Heroicons

Recommended icons:
- Upload
- Database
- FileText
- Search
- Network
- GitMerge
- Filter
- ChartNoAxesCombined
- BookOpen
- Settings

## 18. Motion
Keep motion subtle:
- 120–200 ms
- Fade / small translate
- No bouncy effects
- Graph transitions may animate when filters change

## 19. Accessibility
Target WCAG AA:
- Keyboard navigation
- Visible focus states
- Sufficient contrast
- Do not encode meaning by color alone
- Tooltip alternatives
- Semantic table structure

## 20. Responsive behavior
Primary use is desktop research work, but tablet should remain functional.

On smaller screens:
- Collapse left nav
- Stack cards
- Keep tables horizontally scrollable
- Preserve filters in a drawer

## 21. Initial application sections

### Phase 1
- Dashboard
- Import
- Corpus
- Deduplication
- Data quality
- Export

### Phase 2
- Text mining
- Actors/components
- Theories
- Constructs
- Co-occurrence networks
- Concept map

### Phase 3
- Evidence explorer
- Time evolution
- Relationship analysis
- ECTAF workspace

## 22. Design rule for the coding AI
When generating UI, optimize for:
1. Scientific clarity
2. Traceability
3. Data density without clutter
4. Reproducibility
5. Consistent component patterns
6. Fast researcher workflows

When choosing between visual novelty and clarity, always choose clarity.

## 23. Askit4 corporate endorsement

CorpusMiner is presented as a product of **Askit4 - IT Solutions**.

CorpusMiner keeps its own product identity, color system, typography, favicon, and application icon. Askit4 acts as the corporate endorsement / owner brand and must not visually replace the CorpusMiner identity inside the product.

Preferred attribution:

**A product of Askit4**

Recommended locations:
- login / authentication screen
- public product landing page
- application footer
- About / product information screen
- legal, privacy, terms, and copyright areas

Rules:
- The CorpusMiner logo remains the primary product logo.
- The CorpusMiner app icon remains the browser favicon and PWA/app icon.
- Do not use the Askit4 logo as the main navigation logo.
- Do not derive CorpusMiner UI colors from the Askit4 logo.
- Use the official Askit4 asset defined in `docs/design/BRAND_ASSETS.md`.
- Legal attribution must remain visible wherever required by `docs/legal/WEBSITE_LEGAL_REQUIREMENTS.md`.
