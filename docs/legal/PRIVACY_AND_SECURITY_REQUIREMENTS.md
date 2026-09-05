# CorpusMiner - Privacy and Security Requirements

Status: Engineering requirements. Legal review still required for public policies.

## 1. Privacy by design

CorpusMiner must apply:
- data minimization;
- purpose limitation;
- least privilege;
- secure defaults;
- explicit retention rules;
- auditable processing;
- user-access and deletion workflows where applicable;
- separation between product telemetry and research-corpus content.

Do not collect information merely because it may be useful later.

## 2. Data categories to model explicitly

At minimum, distinguish:

### Account data
Examples:
- name;
- email;
- organization;
- authentication identifiers;
- roles and permissions.

### Research corpus data
Examples:
- titles;
- authors;
- affiliations;
- DOI;
- abstracts;
- keywords;
- references;
- source database identifiers;
- imported metadata;
- uploaded files.

Research metadata may contain personal information even when publicly available. Do not assume academic metadata is outside privacy obligations.

### Operational data
Examples:
- audit records;
- job status;
- import timestamps;
- pipeline versions;
- error diagnostics;
- security events.

### Telemetry
Examples:
- page usage;
- performance;
- feature usage;
- browser/device details;
- analytics identifiers.

Telemetry must be independently configurable and must not silently include corpus content.

## 3. Uploaded content

- Validate file type and size server-side.
- Malware-scan uploads when feasible.
- Store uploaded content in private storage by default.
- Use short-lived or scoped access URLs.
- Do not expose raw storage paths publicly.
- Do not index private uploads into a public search index.
- Keep tenant/user access boundaries explicit.
- Do not log full uploaded files or full document contents.

## 4. AI and external processing

Before sending any corpus, document, abstract, metadata, prompt, or user content to an AI/model provider or other third party, document:
- provider;
- purpose;
- exact data categories sent;
- region/location if known;
- retention/training settings if available;
- contractual/data-processing terms;
- whether the user must be informed or consent obtained;
- mechanism to disable the integration when appropriate.

No third-party AI processing should be introduced implicitly through a convenience SDK.

## 5. Secrets and authentication

- Secrets belong in approved secret stores/environment configuration, never Git.
- Never expose server credentials in browser code.
- Apply MFA/SSO where appropriate.
- Enforce server-side authorization for every protected operation.
- Do not rely only on hidden UI controls for authorization.
- Log security-relevant administrative actions without logging sensitive payloads.

## 6. Logging

Do not log by default:
- access tokens;
- refresh tokens;
- passwords;
- API keys;
- full uploaded documents;
- full abstracts or document text unless strictly necessary for a diagnosed incident;
- sensitive personal fields;
- raw prompts containing confidential corpus content.

Use structured logs and redact sensitive fields.

## 7. Retention and deletion

Retention periods must be configurable/documented for:
- user accounts;
- uploaded source files;
- normalized corpus data;
- generated analyses;
- exports;
- audit/security logs;
- telemetry;
- backups.

Deletion must consider primary storage, derived indexes/caches, and scheduled backup expiration. Do not promise immediate irreversible deletion from backups unless architecture actually supports it.

## 8. Export and user rights

Where applicable, provide processes for:
- account information access;
- correction;
- export;
- deletion;
- objection/consent withdrawal for optional processing.

Actual rights depend on applicable law and user relationship; public policy wording must match implemented capabilities.

## 9. Cookies and analytics

- Essential authentication/security cookies may be used where needed.
- Non-essential analytics/marketing cookies require documented review before activation.
- Do not load non-essential trackers before required consent when consent is legally required.
- Keep a machine-readable inventory of cookies/local-storage keys when possible.

## 10. Security baseline

- HTTPS only in production.
- Secure, HttpOnly, SameSite cookies where applicable.
- CSRF protections for cookie-authenticated state-changing requests.
- CSP and secure headers appropriate to the framework.
- Dependency and secret scanning in CI where available.
- Rate limiting for authentication and expensive endpoints.
- Input validation and output encoding.
- Protection against IDOR/BOLA by server-side authorization.
- Backups and restore procedures for production data.

## 11. Applicable-law review

Because Askit4 publicly identifies itself in Costa Rica, legal review should consider Costa Rica's personal-data protection framework, including Law No. 8968 and applicable regulations. Other regimes may apply depending on where users are located and how data is processed.

Engineering must not label the product `GDPR compliant`, `CCPA compliant`, or equivalent without a completed legal/compliance assessment and evidence.
