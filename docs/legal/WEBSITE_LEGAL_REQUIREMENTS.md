# CorpusMiner - Website Legal Requirements

Status: Product/engineering checklist; public wording requires legal review.

## 1. Required public links

Any public CorpusMiner website, sign-in page, or marketing landing page should provide persistent access to:
- Privacy Policy;
- Terms of Service;
- Cookie Policy when cookies/analytics beyond strictly necessary storage are used;
- Legal Notice / About;
- contact channel;
- corporate attribution: `A product of Askit4`.

Recommended footer pattern:

`CorpusMiner - A product of Askit4 | Privacy | Terms | Cookies | Legal | Contact`

## 2. Copyright notice

Use a dynamic current-year copyright notice after confirming the legal owner/operator name.

Do not hard-code a legal entity name until it is validated.

Example implementation placeholder:

`© {currentYear} [LEGAL ENTITY]. CorpusMiner. All rights reserved.`

## 3. Corporate attribution

Public product surfaces should identify CorpusMiner as a product of Askit4 without replacing the CorpusMiner product identity.

Use the official Askit4 logo only as specified in `docs/design/BRAND_ASSETS.md`.

## 4. Privacy-policy consistency

The privacy policy must match actual implementation. Before release, verify:
- account data collected;
- analytics tools;
- cookies/local storage;
- hosting providers;
- email providers;
- AI/model providers;
- file storage;
- backups;
- retention;
- deletion/export behavior;
- support/contact channel;
- subprocessors and international transfers if relevant.

Do not publish boilerplate that claims processing the application does not perform.

## 5. Consent

If consent is used as a legal basis or required for non-essential cookies/analytics:
- consent must be specific enough to understand;
- optional processing must not be preselected where prohibited;
- refusal must be as accessible as acceptance when required;
- consent state must be recorded appropriately;
- withdrawal must be possible.

## 6. Research-source notice

Where CorpusMiner supports Scopus or Web of Science imports, public wording and UI should make clear that:
- CorpusMiner is an independent product unless a formal partnership exists;
- users remain responsible for rights/licensing applicable to their exports and uploaded material;
- third-party trademarks remain property of their respective owners.

Do not imply endorsement by Elsevier, Scopus, Clarivate, or Web of Science unless contractually authorized.

## 7. User-upload notice

Before upload/processing, terms or contextual UI should establish that users must have the right to upload/process the material and must not use CorpusMiner to violate copyright, confidentiality, database rights, contractual restrictions, or privacy rights.

## 8. AI disclosure

If AI-assisted analysis is introduced, the product should clearly disclose material use of external AI providers where user/corpus content leaves the CorpusMiner-controlled environment.

The privacy policy must identify the categories of content shared and purposes at an appropriate level of specificity.

## 9. Release gate

Before a public production release involving user data, the owner should confirm:
- [ ] legal entity/operator name validated;
- [ ] Privacy Policy reviewed;
- [ ] Terms reviewed;
- [ ] Cookie behavior inventoried;
- [ ] actual subprocessors documented;
- [ ] retention/deletion behavior documented;
- [ ] footer/legal links functional;
- [ ] contact email functional;
- [ ] Askit4 attribution present;
- [ ] CorpusMiner/Askit4 logos use approved assets;
- [ ] third-party source/trademark disclaimer present where appropriate;
- [ ] security/privacy engineering checks completed.
