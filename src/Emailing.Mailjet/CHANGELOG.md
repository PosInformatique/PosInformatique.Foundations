1.8.0
  - Added support for the Reply-To contact.
  - Fixed message-level Mailjet errors other than 403 being silently ignored. All message-level errors now throw an `EmailProviderException`.

1.3.0
  - Initial release with Mailjet Emailing provider.
