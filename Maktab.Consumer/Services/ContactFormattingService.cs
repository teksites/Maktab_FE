using MaktabDataContracts.Responses.OtherContacts;
using MaktabDataContracts.Enums;
using Microsoft.Extensions.Localization;

namespace Maktab.Consumer.Services
{
    public interface IContactFormattingService
    {
        /// <summary>
        /// Gets color for the relationship badge
        /// </summary>
        MudBlazor.Color GetRelationshipColor(OtherContactResponse contact);

        /// <summary>
        /// Gets icon for the contact type
        /// </summary>
        string GetContactIcon(OtherContactResponse contact);

        /// <summary>
        /// Formats contact information for display
        /// </summary>
        string FormatContactInfo(OtherContactResponse contact);
    }

    public class ContactFormattingService : IContactFormattingService
    {
        private readonly IStringLocalizer<ContactFormattingService> _localizer;

        public ContactFormattingService(IStringLocalizer<ContactFormattingService> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Gets color for relationship badge based on type
        /// </summary>
        public MudBlazor.Color GetRelationshipColor(OtherContactResponse contact)
        {
            return contact.Relationship switch
            {
                 Relationship.Father => MudBlazor.Color.Primary,
                 Relationship.Mother => MudBlazor.Color.Warning,
                 Relationship.Guardian => MudBlazor.Color.Info,
                 Relationship.Relative => MudBlazor.Color.Secondary,
                 Relationship.FamilyFriend => MudBlazor.Color.Success,
                 Relationship.Teacher => MudBlazor.Color.Tertiary,
                _ => MudBlazor.Color.Default
            };
        }

          /// <summary>
          /// Gets icon for contact display
          /// </summary>
          public string GetContactIcon(OtherContactResponse contact)
          {
               return contact.Relationship switch
               {
                    Relationship.Father => MudBlazor.Icons.Material.Filled.Face,
                    Relationship.Mother => MudBlazor.Icons.Material.Filled.Face4,
                    Relationship.Guardian => MudBlazor.Icons.Material.Filled.Security,
                    Relationship.Relative => MudBlazor.Icons.Material.Filled.Group,
                    Relationship.FamilyFriend => MudBlazor.Icons.Material.Filled.Groups,
                    Relationship.Teacher => MudBlazor.Icons.Material.Filled.School,
                    _ => MudBlazor.Icons.Material.Filled.Person
               };
          }

        /// <summary>
        /// Formats basic contact information
        /// </summary>
        public string FormatContactInfo(OtherContactResponse contact)
        {
            var info = new List<string>();

            if (!string.IsNullOrWhiteSpace(contact.Phone))
                info.Add($"📞 {contact.Phone}");

            //if (!string.IsNullOrWhiteSpace(contact.Email))
            //    info.Add($"✉️ {contact.Email}");

            return string.Join(" • ", info);
        }
    }
}
