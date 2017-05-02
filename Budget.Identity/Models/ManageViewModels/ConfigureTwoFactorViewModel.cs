using System.Collections.Generic;

namespace Budget.Identity.Models.ManageViewModels
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<string> Providers { get; set; }
    }
}
