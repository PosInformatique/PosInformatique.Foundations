# PosInformatique.Foundations.Emailing.Azure

## Changelog

### 1.2.0

#### PosInformatique.Foundations.People.AwesomeAssertions
- Add the support with AwesomeAssertions to assert the FirstName and LastName value objects.

### 1.1.0

- Add the support of .NET 10.0 for all the packages.

#### PosInformatique.Foundations.Emailing
- Add the support to send emails with attachments.
- Add a new overload method `EmailRecipientCollection<TModel>.Add(EmailAddress, TModel)`.

#### PosInformatique.Foundations.Emailing.Azure
- Add the support to send emails with attachments.
- Upgrade the [Microsoft.Extensions.Azure](https://www.nuget.org/packages/Microsoft.Extensions.Azure) dependency to version 1.13.1 to fix security vulnerabilities.

#### PosInformatique.Foundations.Emailing.Graph
- Add the support to send emails with attachments.

#### PosInformatique.Foundations.Text.Templating.Scriban
- Upgrade the [Scriban](https://www.nuget.org/packages/Scriban) dependency to version 7.0.0 to fix security vulnerabilities.

### 1.0.0
- Initial version of the following packages:
  - [PosInformatique.Foundations.EmailAddresses](./src/EmailAddresses/README.md)
  - [PosInformatique.Foundations.EmailAddresses.EntityFramework](./src/EmailAddresses.EntityFramework/README.md)
  - [PosInformatique.Foundations.EmailAddresses.FluentValidation](./src/EmailAddresses.FluentValidation/README.md)
  - [PosInformatique.Foundations.EmailAddresses.Json](./src/EmailAddresses.Json/README.md)
  - [PosInformatique.Foundations.Emailing](./src/Emailing/README.md)
  - [PosInformatique.Foundations.Emailing.Azure](./src/Emailing.Azure/README.md)
  - [PosInformatique.Foundations.Emailing.Graph](./src/Emailing.Graph/README.md)
  - [PosInformatique.Foundations.Emailing.Templates.Razor](./src/Emailing.Templates.Razor/README.md)
  - [PosInformatique.Foundations.MediaTypes](./src/MediaTypes/README.md)
  - [PosInformatique.Foundations.MediaTypes.EntityFramework](./src/MediaTypes.EntityFramework/README.md)
  - [PosInformatique.Foundations.MediaTypes.Json](./src/MediaTypes.Json/README.md)
  - [PosInformatique.Foundations.People](./src/People/README.md)
  - [PosInformatique.Foundations.People.DataAnnotations](./src/People.DataAnnotations/README.md)
  - [PosInformatique.Foundations.People.EntityFramework](./src/People.EntityFramework/README.md)
  - [PosInformatique.Foundations.People.FluentAssertions](./src/People.FluentAssertions/README.md)
  - [PosInformatique.Foundations.People.FluentValidation](./src/People.FluentValidation/README.md)
  - [PosInformatique.Foundations.People.Json](./src/People.Json/README.md)
  - [PosInformatique.Foundations.PhoneNumbers](./src/PhoneNumbers/README.md)
  - [PosInformatique.Foundations.PhoneNumbers.EntityFramework](./src/PhoneNumbers.EntityFramework/README.md)
  - [PosInformatique.Foundations.PhoneNumbers.FluentValidation](./src/PhoneNumbers.FluentValidation/README.md)
  - [PosInformatique.Foundations.PhoneNumbers.Json](./src/PhoneNumbers.Json/README.md)
  - [PosInformatique.Foundations.Text.Templating](./src/Text.Templating/README.md)
  - [PosInformatique.Foundations.Text.Templating.Razor](./src/Text.Templating.Razor/README.md)
  - [PosInformatique.Foundations.Text.Templating.Scriban](./src/Text.Templating.Scriban/README.md)