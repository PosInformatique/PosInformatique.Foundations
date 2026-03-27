1.1.0
  - Upgrade the Scriban dependency to version 7.0.0 to fix security vulnerabilities.
    - [Scriban Affected by Memory Exhaustion (OOM) via Unbounded String Generation (Denial of Service)](https://github.com/advisories/GHSA-5rpf-x9jg-8j5p)
    - [Scriban: Sandbox escape due to TypedObjectAccessorcache bypassing MemberFilter after TemplateContext reuse](https://github.com/advisories/GHSA-5wr9-m6jw-xx44)
    - [Scriban: Denial of Service via Unbounded Cumulative Template Output Bypassing LimitToString](https://github.com/advisories/GHSA-m2p3-hwv5-xpqw)
    - [Scriban has Multiple Denial-of-Service Vectors via Unbounded Resource Consumption During Expression Evaluation](https://github.com/advisories/GHSA-xw6w-9jjh-p9cr)

1.0.0
  - Initial release with the Scriban Text Templating feature.
