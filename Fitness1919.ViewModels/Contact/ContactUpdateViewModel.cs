﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Contact;

namespace Fitness1919.Web.ViewModels.Contact
{
    public class ContactUpdateViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        [Required]
        [Display(Name = PhoneNumberName)]
        [RegularExpression(PhoneNumberExpression)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [RegularExpression(EmailExpression)]
        public string Email { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
    }
}