using System;
using System.ComponentModel.DataAnnotations;

namespace Maincotech.ExamAssistant.Dtos
{
    public class AppUserDto
    {
        //
        // Summary:
        //     Gets the user ID of this user.
        public string Uid { get; set; }

        //
        // Summary:
        //     Gets the user's display name, if available. Otherwise null.
        public string DisplayName { get; set; }

        //
        // Summary:
        //     Gets the user's email address, if available. Otherwise null.
        [Required]
        public string Email { get; set; }

        //
        // Summary:
        //     Gets the user's phone number, if available. Otherwise null.
        public string PhoneNumber { get; set; }

        //
        // Summary:
        //     Gets the user's photo URL, if available. Otherwise null.
        public string PhotoUrl { get; set; }

        //
        // Summary:
        //     Gets the ID of the identity provider. This returns the constant value firebase.
        public string ProviderId { get; set; }

        //
        // Summary:
        //     Gets a value indicating whether the user's email address is verified or not.
        public bool EmailVerified { get; set; }

        //
        // Summary:
        //     Gets a value indicating whether the user account is disabled or not.
        public bool Disabled { get; set; }

        //
        // Summary:
        //     Gets the user's tenant ID, if available. Otherwise null.
        public string TenantId { get; set; }

        //
        // Summary:
        //     Gets a timestamp representing the date and time that the account was created.
        //     If not available this property is null.
        public DateTime? CreationTimestamp { get; set; }

        //
        // Summary:
        //     Gets a timestamp representing the last time that the user has signed in. If the
        //     user has never signed in this property is null.
        public DateTime? LastSignInTimestamp { get; set; }

        public string Password { get; set; }
    }
}