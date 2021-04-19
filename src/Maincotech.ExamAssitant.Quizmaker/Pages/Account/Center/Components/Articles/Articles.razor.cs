using System.Collections.Generic;
using Maincotech.Quizmaker.Models;
using Microsoft.AspNetCore.Components;

namespace Maincotech.Quizmaker.Pages.Account.Center
{
    public partial class Articles
    {
        [Parameter] public IList<ListItemDataType> List { get; set; }
    }
}